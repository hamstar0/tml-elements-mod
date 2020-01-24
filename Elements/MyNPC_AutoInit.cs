using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using HamstarHelpers.Classes.DataStructures;
using HamstarHelpers.Services.EntityGroups;


namespace Elements {
	partial class ElementsNPC : GlobalNPC {
		private bool AutoInitializeElement( NPC npc ) {
			var config = ElementsConfig.Instance;
			var npcDef = new NPCDefinition( npc.type );

			if( config.AutoAssignedNPCs.ContainsKey( npcDef ) ) {
				ElementDefinition elemDef = ElementDefinition.PickDefinitionForNPC( config.AutoAssignedNPCs[npcDef] );
				this.Elements.Add( elemDef );
			}

			IReadOnlySet<string> grpNames;
			if( EntityGroups.TryGetGroupsPerNPC( npc.netID, out grpNames ) ) {
				float autoChance = -1f;
				foreach( string grpName in grpNames ) {
					if( config.AutoAssignedNPCGroups.ContainsKey( grpName ) ) {
						autoChance = config.AutoAssignedNPCGroups[grpName];
						break;
					}
				}

				if( autoChance != -1f ) {
					ElementDefinition elemDef = ElementDefinition.PickDefinitionForNPC( autoChance );

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
