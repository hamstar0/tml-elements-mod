using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace Elements {
	partial class ElementsNPC : GlobalNPC {
		public static int ComputeDamage(
					NPC npc,
					int damage,
					ISet<ElementDefinition> itemElements,
					ISet<ElementDefinition> npcElements,
					out ISet<ElementDefinition> absorbedElements,
					out ISet<ElementDefinition> afflictedElements ) {
			absorbedElements = new HashSet<ElementDefinition>();
			afflictedElements = new HashSet<ElementDefinition>();

			var config = ElementsConfig.Instance;
			float baseDamage = damage;
			float newDamage = damage;

			if( itemElements.Count == 0 ) {
				itemElements = new HashSet<ElementDefinition> { ElementsConfig.PhysicalElement };
			}
			if( npcElements.Count == 0 ) {
				npcElements = new HashSet<ElementDefinition> { ElementsConfig.PhysicalElement };
			}

			foreach( ElementDefinition itemElemDef in itemElements ) {
				foreach( ElementDefinition npcElemDef in npcElements ) {
					float dmg;

					if( npcElemDef.WeakAgainst.Contains(itemElemDef.Name) ) {
						afflictedElements.Add( itemElemDef );
						dmg = baseDamage * config.ElementAfflictDamageMultiplier;
//Main.NewText( "0a "+npc.TypeName+" of "+npcElemDef.Name+" afflicted by "+itemElemDef.Name+" at "+dmg+"("+baseDamage+")" );
					} else if( npcElemDef.StrongAgainst.Contains(itemElemDef.Name) ) {
						absorbedElements.Add( itemElemDef );
						dmg = baseDamage * config.ElementAbsorbDamageMultiplier;
//Main.NewText( "0b! "+npc.TypeName+" of "+npcElemDef.Name+" absorbs "+itemElemDef.Name+" at "+dmg+"("+baseDamage+")" );
					} else if( npcElemDef.Name.Equals(itemElemDef.Name) ) {
//Main.NewText( "0c... "+npc.TypeName+" of "+npcElemDef.Name+" neutral against "+itemElemDef.Name );
						dmg = baseDamage * config.ElementEqualDamageMultiplier;
					} else {
//Main.NewText( "0d "+npc.TypeName+" of "+npcElemDef.Name+" ? against "+itemElemDef.Name );
						continue;
					}

					newDamage += dmg - baseDamage;
				}
			}

			return (int)Math.Max( newDamage, 0 );
		}
	}
}
