using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Services.AnimatedColor;
using HamstarHelpers.Services.Hooks.LoadHooks;
using Elements.Protocols;


namespace Elements {
	partial class ElementsNPC : GlobalNPC {
		public static bool CanHaveElements( NPC npc ) {
			return npc?.active == true && !npc.townNPC && !npc.dontTakeDamage;
		}


		////////////////

		public static int ComputeDamage(
					int damage,
					ISet<ElementDefinition> itemDefs,
					ISet<ElementDefinition> npcDefs,
					out ISet<ElementDefinition> absorbElements,
					out ISet<ElementDefinition> afflictElements ) {
			absorbElements = new HashSet<ElementDefinition>();
			afflictElements = new HashSet<ElementDefinition>();

			var config = ElementsConfig.Instance;
			float baseDamage = damage;
			float newDamage = damage;

			foreach( ElementDefinition itemElemDef in itemDefs ) {
				foreach( ElementDefinition npcElemDef in npcDefs ) {
					float dmg;

					if( npcElemDef.WeakAgainst.Contains(itemElemDef.Name) ) {
//Main.NewText( "0a npc of "+npcElemDef.Name+" afflicts against "+itemElemDef.Name );
						afflictElements.Add( itemElemDef );
						dmg = baseDamage * config.ElementAfflictDamageMultiplier;
					} else if( npcElemDef.StrongAgainst.Contains(itemElemDef.Name) ) {
						absorbElements.Add( itemElemDef );
						dmg = baseDamage * config.ElementAbsorbDamageMultiplier;
//Main.NewText( "0BB npc of "+npcElemDef.Name+" absorbs against "+itemElemDef.Name+" ("+absorbElements.Count+")" );
					} else if( npcElemDef.Name.Equals(itemElemDef.Name) ) {
//Main.NewText( "0c npc of "+npcElemDef.Name+" neutral against "+itemElemDef.Name );
						dmg = baseDamage * config.ElementEqualDamageMultiplier;
					} else {
//Main.NewText( "0d npc of "+npcElemDef.Name+" ? against "+itemElemDef.Name );
						continue;
					}

					newDamage += dmg - baseDamage;
				}
			}

			return (int)Math.Max( newDamage, 0 );
		}



		////////////////

		public override bool InstancePerEntity => true;


		////

		public bool IsInitialized { get; internal set; } = false;
		public ISet<ElementDefinition> Elements { get; internal set; } = new HashSet<ElementDefinition>();


		////

		private int AbsorbAnimation = 0;

		private ISet<ElementDefinition> AbsorbedElements = new HashSet<ElementDefinition>();
		private ISet<ElementDefinition> AfflictedElements = new HashSet<ElementDefinition>();


		////

		private AnimatedColors ColorAnimation;



		////////////////
		
		 private bool AwaitsInitialization = false;

		private void InitializeWithSync( NPC npc ) {
			if( this.AwaitsInitialization || this.IsInitialized || !ElementsNPC.CanHaveElements(npc) ) { return; }
			this.AwaitsInitialization = true;

			LoadHooks.AddPostWorldLoadOnceHook( () => {
				this.IsInitialized = true;
				this.AwaitsInitialization = false;

				bool? willAutoInit = ElementsAPI.PreNPCInitialize( npc );

				if( (willAutoInit.HasValue && willAutoInit.Value) || this.AutoInitializeElement( npc ) ) {
					if( Main.netMode == 2 ) {
						NPCElementsProtocol.Broadcast( npc.whoAmI );
					} else {
						this.InitializeColorAnimation();
					}
				}
			} );
		}

		private void InitializeColorAnimation() {
			var colors = new List<Color>();

			foreach( ElementDefinition elemDef in this.Elements ) {
				colors.Add( elemDef.GlowColor );
				colors.Add( Color.Transparent );
			}

			if( colors.Count > 0 ) {
				this.ColorAnimation = AnimatedColors.Create( 15, colors.ToArray() );
			}
		}


		////////////////

		public override bool PreAI( NPC npc ) {
			if( !this.IsInitialized ) {
				this.InitializeWithSync( npc );
			}
			return base.PreAI( npc );
		}



		////////////////

		public override void ModifyHitByItem( NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit ) {
			var myitem = item.GetGlobalItem<ElementsItem>();
			var mynpc = npc.GetGlobalNPC<ElementsNPC>();

			this.ApplyHit( ref damage, myitem.Elements, mynpc.Elements );
		}

		public override void ModifyHitByProjectile( NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection ) {
			var myproj = projectile.GetGlobalProjectile<ElementsProjectile>();
			var mynpc = npc.GetGlobalNPC<ElementsNPC>();

			this.ApplyHit( ref damage, myproj.Elements, mynpc.Elements );
		}

		////

		private void ApplyHit( ref int damage, ISet<ElementDefinition> attackElements, ISet<ElementDefinition> targetElements ) {
			ISet<ElementDefinition> absorbedElems, afflictedElems;

			damage = ElementsNPC.ComputeDamage(
				damage,
				attackElements,
				targetElements,
				out absorbedElems,
				out afflictedElems
			);

			if( absorbedElems.Count > 0 ) {
				this.AbsorbedElements = absorbedElems;
				this.AbsorbAnimation = 120;
			} else if( afflictedElems.Count > 0 ) {
				this.AfflictedElements = afflictedElems;
			}
		}
	}
}
