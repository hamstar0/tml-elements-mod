using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using HamstarHelpers.Services.AnimatedColor;
using HamstarHelpers.Classes.DataStructures;
using HamstarHelpers.Services.EntityGroups;
using Elements.Protocols;


namespace Elements {
	partial class ElementsNPC : GlobalNPC {
		public static bool CanHaveElements( NPC npc ) {
			return npc?.active != true && (npc.townNPC || npc.dontTakeDamage);
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

		private bool InitializeWithSync( NPC npc ) {
			if( Main.gameMenu ) {
				return false;
			}

			this.IsInitialized = true;

			if( this.InitializeElement( npc) ) {
				this.InitializeColorAnimation();

				if( Main.netMode == 2 ) {
					NPCElementsProtocol.Broadcast( npc.whoAmI );
				}
			}

			return true;
		}

		private bool InitializeElement( NPC npc ) {
			var config = ElementsConfig.Instance;
			var npcDef = new NPCDefinition( npc.type );

			if( config.AutoAssignedNPCs.ContainsKey( npcDef ) ) {
				ElementDefinition elemDef = ElementDefinition.PickDefinitionForNPC( config.AutoAssignedNPCs[npcDef] );
				this.Elements.Add( elemDef );
			}

			IReadOnlySet<string> grpNames;
			if( EntityGroups.TryGetGroupsPerNPC( npc.netID, out grpNames ) ) {
				float autoChance = -1f;
				foreach( string grpName in grpNames ) {
					if( config.AutoAssignedNPCGroups.ContainsKey( grpName ) ) {
						autoChance = config.AutoAssignedNPCGroups[grpName];
						break;
					}
				}

				if( autoChance != -1f ) {
					ElementDefinition elemDef = ElementDefinition.PickDefinitionForNPC( autoChance );
					if( elemDef != null ) {
						this.Elements.Add( elemDef );
						return true;
					}
				}
			}

			return false;
		}

		private void InitializeColorAnimation() {
			var colors = new List<Color>();

			foreach( ElementDefinition elemDef in this.Elements ) {
				colors.Add( elemDef.Color );
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
