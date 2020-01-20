using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

		public override void PostDraw( NPC npc, SpriteBatch sb, Color drawColor ) {
			if( ElementsConfig.Instance.DebugModeInfo ) {
				int i = 0;
				foreach( ElementDefinition elemDef in this.Elements ) {
					var rect = new Rectangle(
						(int)(npc.position.X - Main.screenPosition.X) + (i*4),
						(int)(npc.position.Y - Main.screenPosition.Y),
						4,
						4
					);
					Color color = elemDef.Color;

					sb.Draw( Main.magicPixel, rect, null, color, 0f, default(Vector2), SpriteEffects.None, 1f );
					i++;
				}
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
