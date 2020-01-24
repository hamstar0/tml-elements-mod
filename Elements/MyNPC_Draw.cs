using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.XNA;
using HamstarHelpers.Helpers.Debug;
using Terraria.ID;


namespace Elements {
	partial class ElementsNPC : GlobalNPC {
		public override void DrawEffects( NPC npc, ref Color drawColor ) {
			if( this.AfflictedElements.Count > 0 ) {
				foreach( ElementDefinition elemDef in this.AfflictedElements ) {
					this.DrawElementAfflict( npc, elemDef );
				}
				this.AfflictedElements.Clear();
			}

			if( this.ColorAnimation != null ) {
				Color glowColor = XNAColorHelpers.AddGlow( Color.Transparent, this.ColorAnimation.CurrentColor, true );
				glowColor *= 0.35f;

				this.DrawGlow( npc, glowColor );
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

			if( this.AbsorbAnimation > 0 ) {
				Color color = Color.Transparent;
				float percent = (float)this.AbsorbAnimation / 120f;
				percent *= 0.75f;
				
				foreach( ElementDefinition elemDef in this.AbsorbedElements ) {
					Color newColor = elemDef.Color;
					newColor.A = 255;

					color = Color.Lerp( color, newColor, percent );
				}

				this.DrawGlow( npc, color );

				this.AbsorbAnimation--;
			}
		}


		////////////////
		
		private void DrawGlow( NPC npc, Color color ) {
			Texture2D tex = ModContent.GetTexture( "Terraria/Projectile_" + ProjectileID.StardustTowerMark );
			float npcDim = ( npc.width + npc.height ) / 2;
			float scale = ( npcDim / (float)tex.Width ) * 2.25f;

			Main.spriteBatch.Draw(
				texture: tex,
				position: ( npc.Center - Main.screenPosition ) - ( new Vector2( tex.Width / 2, tex.Height / 2 ) * scale ),
				sourceRectangle: null,
				color: color,
				rotation: 0f,
				origin: default( Vector2 ),//new Vector2(tex.Width / 2, tex.Height / 2) * scale,
				scale: scale,
				effects: SpriteEffects.None,
				layerDepth: 1f
			);
		}


		////////////////

		private void DrawElementAfflict( NPC npc, ElementDefinition elemDef ) {
			for( int i=0; i<elemDef.DustQuantity; i++ ) {
				Dust.NewDust( npc.position, npc.width, npc.height, elemDef.DustType, 0f, 0f, 0, elemDef.Color, elemDef.DustScale );
			}
		}
	}
}
