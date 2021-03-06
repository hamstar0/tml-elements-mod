﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Items.Attributes;


namespace Elements {
	partial class ElementsItem : GlobalItem {
		public override void ModifyTooltips( Item item, List<TooltipLine> tooltips ) {
			//"Elements: " + string.Join( ", ", this.Elements?.Select( e => e.Name ) ?? new string[] { } )
			if( !this.IsInitialized || this.Elements == null || this.Elements.Count == 0 ) {
				return;
			}

			int i = 0;
			foreach( ElementDefinition elemDef in this.Elements ) {
				var tip = new TooltipLine(
					this.mod,
					"Elements_"+i,
					"Has "+elemDef.Name+" affinity. Strong against "+string.Join(", ", elemDef.StrongAgainst)+"."
				);
				tip.overrideColor = Color.Lerp( elemDef.IconColor, Color.White, 0.25f );

				if( !ItemInformationAttributeHelpers.ApplyTooltipAt(tooltips, tip, VanillaTooltipName.Tooltip) ) {
					if( !ItemInformationAttributeHelpers.ApplyTooltipAt(tooltips, tip, VanillaTooltipName.PrefixDamage) ) {
						ItemInformationAttributeHelpers.ApplyTooltipAt( tooltips, tip, VanillaTooltipName.Knockback );
						//	tooltips.Add( tip );
					}
				}
				i++;
			}
		}


		////////////////

		public override void PostDrawInInventory(
					Item item,
					SpriteBatch sb,
					Vector2 position,
					Rectangle frame,
					Color drawColor,
					Color itemColor,
					Vector2 origin,
					float scale ) {
			int i = 0;
			foreach( ElementDefinition elemDef in this.Elements ) {
				this.DrawElementIcon( item, elemDef, sb, position, frame, scale, i++ );
			}
		}

		/*public override void PostDrawInWorld(
					Item item,
					SpriteBatch spriteBatch,
					Color lightColor,
					Color alphaColor,
					float rotation,
					float scale,
					int whoAmI ) {
			Texture2D spoiledTex = StarvationMod.Instance.GetTexture( "Items/RotItem" );
			if( spoiledTex == null ) {
				return false;
			}

			float halfSpoilWid = (float)spoiledTex.Width * scale * 0.5f;
			float halfSpoilHei = (float)spoiledTex.Height * scale * 0.5f;
			float posX = centerPos.X - halfSpoilWid;
			float posY = centerPos.Y - halfSpoilHei;

			var pos = new Vector2( posX, posY ) - Main.screenPosition;
			var srcRect = new Rectangle( 0, 0, spoiledTex.Width, spoiledTex.Height );

			sb.Draw( spoiledTex, pos, srcRect, color, 0f, default( Vector2 ), scale, SpriteEffects.None, 1f );
		}*/


		////

		private void DrawElementIcon(
					Item item,
					ElementDefinition elemDef,
					SpriteBatch sb,
					Vector2 position,
					Rectangle frame,
					float scale,
					int index ) {
			Texture2D iconTex = Main.itemTexture[ elemDef.IconTextureItem.Type ];

			float posX = position.X + (scale * (float)frame.Width * 0.5f);
			float posY = position.Y + (scale * (float)frame.Height * 0.5f);
			posX += ((index * 8f) - 16f);
			posY += 8f;

			var pos = new Vector2( posX, posY );
			Color color = Color.White;
			float iconScale = (12f / (float)iconTex.Width) * 0.75f;//scale

			sb.Draw( iconTex, pos, null, color, 0f, default(Vector2), iconScale, SpriteEffects.None, 1f );
		}
	}
}
