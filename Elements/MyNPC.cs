using Elements.Protocols;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;


namespace Elements {
	class ElementsNPC : GlobalNPC {
		public static bool CanHaveElement( NPC npc ) {
			return npc?.active != true && (npc.townNPC || npc.dontTakeDamage);
		}


		////////////////
		
		public static void InitializeElementWithSync( NPC npc ) {
			var config = ElementsConfig.Instance;
			var mynpc = npc.GetGlobalNPC<ElementsNPC>();
			var npcDef = new NPCDefinition( npc.type );

			mynpc.IsInitialized = true;

			if( config.AutoAssignedNPCs.ContainsKey( npcDef ) ) {
				ElementDefinition elemDef = ElementDefinition.PickDefinitionForNPC( config.AutoAssignedNPCs[npcDef] );
				mynpc.Elements.Add( elemDef );
			}

			if( Main.netMode == 2 ) {
				NPCElementsProtocol.Broadcast( npc.whoAmI );
			}
		}


		////////////////

		public static int ComputeDamage( int damage, ISet<ElementDefinition> itemDefs, ISet<ElementDefinition> npcDefs ) {
			float baseDamage = damage;
			float newDamage = damage;

			foreach( ElementDefinition itemElemDef in itemDefs ) {
				foreach( ElementDefinition npcElemDef in npcDefs ) {
					float dmg;
					
					if( itemElemDef.WeakAgainst.Contains( npcElemDef.Name ) ) {
						dmg = baseDamage * ElementsConfig.Instance.ElementStrengthDamageMultiplier;
					} else if( itemElemDef.StrongAgainst.Contains( npcElemDef.Name ) ) {
						dmg = baseDamage * ElementsConfig.Instance.ElementWeaknessDamageMultiplier;
					} else {
						continue;
					}

					newDamage += dmg - baseDamage;
				}
			}

			return (int)newDamage;
		}



		////////////////

		public override bool InstancePerEntity => true;


		////////////////

		public bool IsInitialized { get; internal set; } = false;
		public ISet<ElementDefinition> Elements { get; internal set; } = new HashSet<ElementDefinition>();



		////////////////

		public override bool PreAI( NPC npc ) {
			if( !this.IsInitialized ) {
				ElementsNPC.InitializeElementWithSync( npc );
			}
			return base.PreAI( npc );
		}



		////////////////

		public override void ModifyHitByItem( NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit ) {
			var mynpc = npc.GetGlobalNPC<ElementsNPC>();
			var myitem = item.GetGlobalItem<ElementsItem>();

			damage = ElementsNPC.ComputeDamage( damage, myitem.Elements, mynpc.Elements );
		}

		public override void ModifyHitByProjectile( NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection ) {
			var mynpc = npc.GetGlobalNPC<ElementsNPC>();
			var myproj = projectile.GetGlobalProjectile<ElementsProjectile>();

			damage = ElementsNPC.ComputeDamage( damage, myproj.Elements, mynpc.Elements );
		}
	}
}
