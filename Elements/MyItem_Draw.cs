using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.XNA;


namespace Elements {
	partial class ElementsItem : GlobalItem {
		public override void ModifyTooltips( Item item, List<TooltipLine> tooltips ) {
			if( this.IsInitialized && ElementsConfig.Instance.DebugModeInfo ) {
				var tip = new TooltipLine( this.mod, "ElementsDebug", "Elements: "+string.Join(", ", this.Elements.Select(e=>e.Name)) );
				tooltips.Add( tip );
			}
		}


		////////////////

		public override Color? GetAlpha( Item item, Color lightColor ) {
			if( !this.IsInitialized || this.ColorAnimation == null ) {
				return null;
			}

			return XNAColorHelpers.AddGlow( this.ColorAnimation.CurrentColor, lightColor, true );
		}


		/*public override void PostDrawInInventory(
					Item item,
					SpriteBatch spriteBatch,
					Vector2 position,
					Rectangle frame,
					Color drawColor,
					Color itemColor,
					Vector2 origin,
					float scale ) {
		}

		public override void PostDrawInWorld(
					Item item,
					SpriteBatch spriteBatch,
					Color lightColor,
					Color alphaColor,
					float rotation,
					float scale,
					int whoAmI ) {
		}*/
	}
}
