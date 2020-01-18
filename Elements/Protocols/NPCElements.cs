using System;
using System.Linq;
using System.Collections.Generic;
using Terraria;
using HamstarHelpers.Classes.Protocols.Packet.Interfaces;


namespace Elements.Protocols {
	class NPCElementsProtocol : PacketProtocolSentToEither {
		public static void Broadcast( int npcWho ) {
			var protocol = new NPCElementsProtocol();

			if( protocol.SetNpc(npcWho) ) {
				protocol.SendToClient( -1, -1 );
			}
		}



		////////////////

		private int NpcWho;
		private string[] ElementNames;



		////////////////

		private NPCElementsProtocol() { }


		////////////////

		protected override bool ReceiveRequestWithServer( int fromWho ) {
			this.SyncAllNpcsToClient( fromWho );
			return true;
		}

		protected override void ReceiveOnServer( int fromWho ) { }

		////////////////

		protected override void ReceiveOnClient() {
			NPC npc = Main.npc[this.NpcWho];
			var mynpc = npc.GetGlobalNPC<ElementsNPC>();

			mynpc.IsInitialized = true;
			mynpc.Elements = new HashSet<ElementDefinition>(
				this.ElementNames
				.Select( name => ElementDefinition.GetElementByName(name) )
			);
		}


		////////////////

		private bool SetNpc( int npcWho ) {
			NPC npc = Main.npc[npcWho];
			if( !ElementsNPC.CanHaveElement(npc) ) {
				return false;
			}

			var mynpc = npc.GetGlobalNPC<ElementsNPC>();

			this.NpcWho = npcWho;
			this.ElementNames = mynpc.Elements.Select( def => def.Name ).ToArray();
			return true;
		}

		private void SyncAllNpcsToClient( int toWho ) {
			for( int i = 0; i < Main.npc.Length; i++ ) {
				if( this.SetNpc(i) ) {
					this.SendToClient( toWho, -1 );
				}
			}
		}
	}
}
