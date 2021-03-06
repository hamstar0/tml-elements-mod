﻿using System;
using System.IO;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Services.Hooks.LoadHooks;


namespace Elements {
	partial class ElementsItem : GlobalItem {
		public static bool CanHaveElements( Item item ) {
			return item?.active == true && item.damage > 0;
		}



		////////////////

		//public override bool CloneNewInstances => false;
		public override bool InstancePerEntity => true;


		////////////////

		public bool IsInitialized { get; private set; } = false;
		public ISet<ElementDefinition> Elements { get; private set; } = new HashSet<ElementDefinition>();



		////////////////

		public override GlobalItem NewInstance( Item item ) {
			var clone = (ElementsItem)base.NewInstance( item );
			clone.IsInitialized = this.IsInitialized;
			clone.Elements.UnionWith( this.Elements );

			return clone;
		}

		////

		 private bool AwaitsInitialization = false;

		private void Initialize( Item item ) {
			if( this.AwaitsInitialization || this.IsInitialized || !ElementsItem.CanHaveElements(item) ) { return; }
			this.AwaitsInitialization = true;

			LoadHooks.AddWorldLoadOnceHook( () => {
				this.AwaitsInitialization = false;
				this.IsInitialized = true;
				
				if( ElementsAPI.PreItemInitialize(item) ) {
					this.AutoInitializeElement( item );
				}
			} );
		}


		////////////////

		public override bool NeedsSaving( Item item ) {
			return ElementsItem.CanHaveElements( item )
				&& this != ModContent.GetInstance<ElementsItem>();
		}


		public override void Load( Item item, TagCompound tag ) {
			this.Elements.Clear();
			this.IsInitialized = false;

			if( !tag.ContainsKey( "elem_count" ) ) {
				return;
			}

			int elemDefCount = tag.GetInt( "elem_count" );

			if( ElementsConfig.Instance.DebugModeInfo && elemDefCount > 0 ) {
				LogHelpers.Log( item.HoverName+" with # elements: "+elemDefCount+" (reset? "+ElementsConfig.Instance.DebugModeReset+")" );
			}
			if( ElementsConfig.Instance.DebugModeReset ) {
				return;
			}

			for( int i=0; i<elemDefCount; i++ ) {
				string defName = tag.GetString( "elem_" + i );
				ElementDefinition def = ElementDefinition.GetElementByName( defName );
				if( def == null ) {
					LogHelpers.Warn( "Missing element definition "+defName );
					continue;
				}

				this.Elements.Add( def );
			}

			this.IsInitialized = true;

			//if( this.Elements.Count > 0 && !Main.dedServ ) {
			//	this.InitializeColorAnimation();
			//}
		}

		public override TagCompound Save( Item item ) {
			var tag = new TagCompound {
				{ "elem_count", this.Elements.Count }
			};

			int i = 0;
			foreach( ElementDefinition elemDef in this.Elements ) {
				tag["elem_" + i ] = elemDef.Name;
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

			//if( this.Elements.Count > 0 ) {
			//	this.InitializeColorAnimation();
			//}
		}


		////////////////

		public override void UpdateInventory( Item item, Player player ) {
			if( !this.IsInitialized ) {
				this.Initialize( item );
			}
		}

		public override void Update( Item item, ref float gravity, ref float maxFallSpeed ) {
			if( !this.IsInitialized ) {
				this.Initialize( item );
			}
		}
	}
}
