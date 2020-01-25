using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Services.EntityGroups;
using HamstarHelpers.Classes.DataStructures;


namespace Elements {
	partial class ElementsItem : GlobalItem {
		private bool AutoInitializeElement( Item item ) {
			if( this.AutoInitializeElementOfSpecificItem(item) ) {
				return true;
			}
			if( this.AutoInitializeElementOfGroupItem(item) ) {
				return true;
			}

			return false;
		}

		////

		private bool AutoInitializeElementOfSpecificItem( Item item ) {
			var config = ElementsConfig.Instance;
			var itemDef = new ItemDefinition( item.type );

			if( !config.AutoAssignedAnyItem.ContainsKey( itemDef ) ) {
				return false;
			}

			ElementDefinition elemDef = ElementDefinition.PickDefinitionForItem( config.AutoAssignedAnyItem[itemDef] );
			if( elemDef != null ) {
				this.Elements.Add( elemDef );
			}

			return true;
		}

		private bool AutoInitializeElementOfGroupItem( Item item ) {
			var config = ElementsConfig.Instance;
			IReadOnlySet<string> grpNames;

			if( !EntityGroups.TryGetGroupsPerItem( item.type, out grpNames ) ) {
				return false;
			}

			float autoChance = -1f;
			foreach( string grpName in grpNames ) {
				if( config.AutoAssignedAnyOfItemGroup.ContainsKey( grpName ) ) {
					autoChance = config.AutoAssignedAnyOfItemGroup[grpName];
					break;
				}
			}

			if( autoChance == -1f ) {
				return false;
			}

			ElementDefinition elemDef = ElementDefinition.PickDefinitionForItem( autoChance );
			if( elemDef != null ) {
				this.Elements.Add( elemDef );
			}

			return true;
		}
	}
}
