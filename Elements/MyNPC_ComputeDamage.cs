using System;
using System.Collections.Generic;
using Terraria.ModLoader;


namespace Elements {
	partial class ElementsNPC : GlobalNPC {
		public static int ComputeDamage(
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

			foreach( ElementDefinition itemElemDef in itemElements ) {
				foreach( ElementDefinition npcElemDef in npcElements ) {
					float dmg;

					if( npcElemDef.WeakAgainst.Contains(itemElemDef.Name) ) {
//Main.NewText( "0a npc of "+npcElemDef.Name+" afflicts against "+itemElemDef.Name );
						afflictedElements.Add( itemElemDef );
						dmg = baseDamage * config.ElementAfflictDamageMultiplier;
					} else if( npcElemDef.StrongAgainst.Contains(itemElemDef.Name) ) {
						absorbedElements.Add( itemElemDef );
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
	}
}
