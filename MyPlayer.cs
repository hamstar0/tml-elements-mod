using System;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;


namespace Elements {
	class ElementsPlayer : ModPlayer {
		public override void ModifyHitByProjectile( Projectile proj, ref int damage, ref bool crit ) {
			var heldItem = this.player.HeldItem;
			if( heldItem?.active != true ) { return; }

			var myitem = heldItem.GetGlobalItem<ElementsItem>();
			var myproj = proj.GetGlobalProjectile<ElementsProjectile>();

			if( ElementsProjectile.CanHaveElements(proj) ) {
				myproj.SetElements( proj, myitem.Elements );
			}
		}
	}
}
