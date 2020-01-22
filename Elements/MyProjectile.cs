using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;


namespace Elements {
	class ElementsProjectile : GlobalProjectile {
		public static bool CanHaveElements( Projectile proj ) {
			return proj.damage > 0;
		}



		////////////////

		public override bool CloneNewInstances => false;
		public override bool InstancePerEntity => true;


		////////////////

		public bool IsInitialized { get; internal set; } = false;
		public ISet<ElementDefinition> Elements { get; internal set; } = new HashSet<ElementDefinition>();



		////////////////

		public override bool PreAI( Projectile projectile ) {
			if( !this.IsInitialized ) {
				if( projectile.npcProj ) {
					NPC npc = Main.npc[projectile.owner];
					if( npc?.active == true ) {
						this.SetElementsFrom( projectile, npc );
					}
				} else {
					Player plr = Main.player[ projectile.owner ];
					if( plr?.active == true ) {
						Item heldItem = plr.HeldItem;
						if( heldItem?.active == true && !heldItem.IsAir ) {
							this.SetElementsFrom( projectile, heldItem );
						}
					}
				}
			}

			return base.PreAI( projectile );
		}



		////////////////

		public void SetElementsFrom( Projectile proj, Item item ) {
			var myitem = item.GetGlobalItem<ElementsItem>();
			if( myitem.IsInitialized ) {
				this.SetElements( proj, myitem.Elements );
			}
		}
		
		public void SetElementsFrom( Projectile proj, NPC npc ) {
			var mynpc = npc.GetGlobalNPC<ElementsNPC>();
			if( mynpc.IsInitialized ) {
				this.SetElements( proj, mynpc.Elements );
			}
		}

		public void SetElements( Projectile proj, ISet<ElementDefinition> elements ) {
			var myproj = proj.GetGlobalProjectile<ElementsProjectile>();

			myproj.IsInitialized = true;
			myproj.Elements = elements;
		}
	}
}
