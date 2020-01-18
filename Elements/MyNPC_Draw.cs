using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;


namespace Elements {
	partial class ElementsNPC : GlobalNPC {
		public override void DrawEffects( NPC npc, ref Color drawColor ) {
			if( this.AbsorbAnimation > 0 ) {
				foreach( ElementDefinition elemDef in this.AbsorbedElements ) {
					this.DrawElementAbsorb( (float)this.AbsorbAnimation / 30f, elemDef.Color, ref drawColor );
				}
				this.AbsorbAnimation--;
			}

			if( this.AfflictedElements.Count > 0 ) {
				foreach( ElementDefinition elemDef in this.AfflictedElements ) {
					this.DrawElementAfflict( elemDef.Color );
				}
				this.AfflictedElements.Clear();
			}
		}


		////////////////

		private void DrawElementAbsorb( float amount, Color color, ref Color drawColor ) {
			drawColor = Color.Lerp( drawColor, color, amount );
		}

		private void DrawElementAfflict( Color color ) {
		}
	}
}
