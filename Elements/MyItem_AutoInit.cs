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
			var config = ElementsConfig.Instance;
			var itemDef = new ItemDefinition( item.type );

			if( config.AutoAssignedItems.ContainsKey( itemDef ) ) {
				ElementDefinition elemDef = ElementDefinition.PickDefinitionForItem( config.AutoAssignedItems[itemDef] );
				if( elemDef != null ) {
					this.Elements.Add( elemDef );
					return true;
				}
			}

			IReadOnlySet<string> grpNames;
			if( EntityGroups.TryGetGroupsPerItem(item.type, out grpNames) ) {
				float autoChance = -1f;

				foreach( string grpName in grpNames ) {
					if( config.AutoAssignedItemGroups.ContainsKey(grpName) ) {
						autoChance = config.AutoAssignedItemGroups[grpName];
						break;
					}
				}

				if( autoChance != -1f ) {
					ElementDefinition elemDef = ElementDefinition.PickDefinitionForItem( autoChance );

					if( elemDef != null ) {
						this.Elements.Add( elemDef );
						return true;
					}
				}
			}

			return false;
		}
	}
}
