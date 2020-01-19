using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.IO;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Services.AnimatedColor;


namespace Elements {
	partial class ElementsItem : GlobalItem {
		public static bool CanHaveElements( Item item ) {
			return item?.active == true && item.damage > 0;
		}

		public static void InitializeElement( Item item ) {
			var config = ElementsConfig.Instance;
			var myitem = item.GetGlobalItem<ElementsItem>();
			var itemDef = new ItemDefinition( item.type );
			
			myitem.IsInitialized = true;

			if( config.AutoAssignedItems.ContainsKey( itemDef ) ) {
				ElementDefinition elemDef = ElementDefinition.PickDefinitionForItem( config.AutoAssignedItems[itemDef] );
				myitem.Elements.Add( elemDef );
			}
		}



		////////////////

		public override bool CloneNewInstances => false;
		public override bool InstancePerEntity => true;


		////////////////

		public bool IsInitialized { get; private set; } = false;
		public ISet<ElementDefinition> Elements { get; private set; } = new HashSet<ElementDefinition>();


		////////////////

		private AnimatedColors ColorAnimation;



		////////////////

		public override void SetDefaults( Item item ) {
			if( !this.IsInitialized ) {
				this.Initialize( item );
			}
		}

		////

		private void Initialize( Item item ) {
			ElementsItem.InitializeElement( item );
			this.InitializeColorAnimation();
		}

		private void InitializeColorAnimation() {
			var colors = new List<Color>();

			foreach( ElementDefinition elemDef in this.Elements ) {
				colors.Add( elemDef.Color );
				colors.Add( Color.Transparent );
			}

			this.ColorAnimation = AnimatedColors.Create( 15, colors.ToArray() );
		}


		////

		public override bool NeedsSaving( Item item ) {
			return ElementsItem.CanHaveElements( item );
		}

		public override void Load( Item item, TagCompound tag ) {
			if( !tag.ContainsKey("is_initialized") ) {
				return;
			}

			int elemDefCount = tag.GetInt( "elem_def_count" );

			for( int i=0; i<elemDefCount; i++ ) {
				string defName = tag.GetString( "elem_def_" + i );
				ElementDefinition def = ElementDefinition.GetElementByName( defName );
				if( def == null ) {
					LogHelpers.Warn( "Missing element definition "+defName );
					continue;
				}

				this.Elements.Add( def );
			}
		}

		public override TagCompound Save( Item item ) {
			var tag = new TagCompound {
				{ "is_initialized", this.IsInitialized },
				{ "elem_def_count", this.Elements.Count }
			};

			int i = 0;
			foreach( ElementDefinition elemDef in this.Elements ) {
				tag[ "elem_def_"+i ] = elemDef.Name;
				i++;
			}

			return tag;
		}

		////////////////

		public override void NetSend( Item item, BinaryWriter writer ) {
			writer.Write( (bool)this.IsInitialized );
			writer.Write( (int)this.Elements.Count );

			foreach( ElementDefinition elemDef in this.Elements ) {
				writer.Write( elemDef.Name );
			}
		}

		public override void NetReceive( Item item, BinaryReader reader ) {
			this.Elements.Clear();
			this.IsInitialized = reader.ReadBoolean();

			int count = reader.ReadInt32();

			for( int i=0; i<count; i++ ) {
				string elemName = reader.ReadString();
				this.Elements.Add( ElementDefinition.GetElementByName(elemName) );
			}
		}


		////////////////

		public override void UpdateInventory( Item item, Player player ) {
			if( !this.IsInitialized ) {
				this.Initialize( item );
			}
		}
	}
}
