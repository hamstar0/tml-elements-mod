using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using HamstarHelpers.Classes.DataStructures;
using HamstarHelpers.Services.EntityGroups;


namespace Elements {
	partial class ElementsNPC : GlobalNPC {
		private bool AutoInitializeElement( NPC npc ) {
			if( this.AutoInitializeElementOfSpecificNPC(npc) ) {
				return true;
			}
			if( this.AutoInitiailizeElementOfGroupNPC(npc) ) {
				return true;
			}

			return false;
		}

		////

		private bool AutoInitializeElementOfSpecificNPC( NPC npc ) {
			var config = ElementsConfig.Instance;
			var npcDef = new NPCDefinition( npc.type );

			if( !config.AutoAssignedAnyNPC.ContainsKey( npcDef ) ) {
				return false;
			}

			ElementDefinition elemDef = ElementDefinition.PickDefinitionForNPC( config.AutoAssignedAnyNPC[npcDef] );
			if( elemDef != null ) {
				this.Elements.Add( elemDef );
			}

			return true;
		}

		private bool AutoInitiailizeElementOfGroupNPC( NPC npc ) {
			var config = ElementsConfig.Instance;
			IReadOnlySet<string> grpNames;

			if( !EntityGroups.TryGetGroupsPerNPC(npc.type, out grpNames) ) {
				return false;
			}

			float autoChance = -1f;
			foreach( string grpName in grpNames ) {
				if( config.AutoAssignedAnyOfNPCGroup.ContainsKey(grpName) ) {
					autoChance = config.AutoAssignedAnyOfNPCGroup[grpName];
					break;
				}
			}

			if( autoChance == -1f ) {
				return false;
			}

			ElementDefinition elemDef = ElementDefinition.PickDefinitionForNPC( autoChance );
			if( elemDef != null ) {
				this.Elements.Add( elemDef );
			}
			
			return true;
		}
	}
}
