using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.XNA;


namespace Elements {
	partial class ElementsNPC : GlobalNPC {
		public override void DrawEffects( NPC npc, ref Color drawColor ) {
			if( this.AbsorbAnimation > 0 ) {
				foreach( ElementDefinition elemDef in this.AbsorbedElements ) {
					this.DrawElementAbsorb( npc, (float)this.AbsorbAnimation / 30f, elemDef, ref drawColor );
				}
				this.AbsorbAnimation--;
			}

			if( this.AfflictedElements.Count > 0 ) {
				foreach( ElementDefinition elemDef in this.AfflictedElements ) {
					this.DrawElementAfflict( npc, elemDef );
				}
				this.AfflictedElements.Clear();
			}

			if( this.ColorAnimation != null ) {
				XNAColorHelpers.AddGlow( this.ColorAnimation.CurrentColor, drawColor, true );
			}
		}


		////////////////

		private void DrawElementAbsorb( NPC npc, float amount, ElementDefinition elemDef, ref Color drawColor ) {
			drawColor = Color.Lerp( drawColor, elemDef.Color, amount );
		}

		private void DrawElementAfflict( NPC npc, ElementDefinition elemDef ) {
			for( int i=0; i<elemDef.DustQuantity; i++ ) {
				Dust.NewDust( npc.position, npc.width, npc.height, elemDef.DustType, 0f, 0f, 0, elemDef.Color, elemDef.DustScale );
			}
		}
	}
}
