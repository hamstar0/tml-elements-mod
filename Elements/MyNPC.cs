using Elements.Protocols;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;


namespace Elements {
	partial class ElementsNPC : GlobalNPC {
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

		public static int ComputeDamage(
					int damage,
					ISet<ElementDefinition> itemDefs,
					ISet<ElementDefinition> npcDefs,
					out ISet<ElementDefinition> absorbElements,
					out ISet<ElementDefinition> afflictElements,
					out int attackAbsorb ) {
			absorbElements = new HashSet<ElementDefinition>();
			afflictElements = new HashSet<ElementDefinition>();
			float baseDamage = damage;
			float newDamage = damage;

			foreach( ElementDefinition itemElemDef in itemDefs ) {
				foreach( ElementDefinition npcElemDef in npcDefs ) {
					float dmg;
					
					if( itemElemDef.WeakAgainst.Contains( npcElemDef.Name ) ) {
						afflictElements.Add( itemElemDef );
						dmg = baseDamage * ElementsConfig.Instance.ElementWeaknessDamageMultiplier;
					} else if( itemElemDef.StrongAgainst.Contains( npcElemDef.Name ) ) {
						absorbElements.Add( itemElemDef );
						dmg = baseDamage * ElementsConfig.Instance.ElementStrengthDamageMultiplier;
					} else {
						continue;
					}

					newDamage += dmg - baseDamage;
				}
			}

			attackAbsorb = damage > newDamage
				? -1
				: damage < newDamage
				? 1
				: 0;
			return (int)newDamage;
		}



		////////////////

		public override bool InstancePerEntity => true;


		////////////////

		public bool IsInitialized { get; internal set; } = false;
		public ISet<ElementDefinition> Elements { get; internal set; } = new HashSet<ElementDefinition>();


		////////////////

		private int AbsorbAnimation = 0;

		private ISet<ElementDefinition> AbsorbedElements;
		private ISet<ElementDefinition> AfflictedElements;



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
			int attackWeakness;

			damage = ElementsNPC.ComputeDamage(
				damage,
				myitem.Elements,
				mynpc.Elements,
				out this.AbsorbedElements,
				out this.AfflictedElements,
				out attackWeakness
			);
			
			if( attackWeakness < 0 ) {
				this.AbsorbAnimation = 30;
			}
		}

		public override void ModifyHitByProjectile( NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection ) {
			var mynpc = npc.GetGlobalNPC<ElementsNPC>();
			var myproj = projectile.GetGlobalProjectile<ElementsProjectile>();
			int attackWeakness;
			
			damage = ElementsNPC.ComputeDamage(
				damage,
				myproj.Elements,
				mynpc.Elements,
				out this.AbsorbedElements,
				out this.AfflictedElements,
				out attackWeakness
			);

			if( attackWeakness < 0 ) {
				this.AbsorbAnimation = 30;
			}
		}
	}
}
