using System;
using System.Linq;
using Terraria;
using Terraria.Utilities;
using HamstarHelpers.Helpers.TModLoader;


namespace Elements {
	public partial class ElementDefinition {
		public static ElementDefinition GetElementByName( string name ) {
			return ElementsConfig.Instance.Elements.FirstOrDefault( def => def.Name.Equals( name ) );
		}

		////////////////

		public static float GetTotalElementNPCWeight() {
			return ElementsConfig.Instance.Elements.Sum( kv => kv.AutoAssignNPCWeight );
		}

		public static float GetTotalElementItemWeight() {
			return ElementsConfig.Instance.Elements.Sum( kv => kv.AutoAssignItemWeight );
		}


		////////////////

		public static ElementDefinition PickDefinitionForNPC( float chance ) {
			UnifiedRandom rand = TmlHelpers.SafelyGetRand();
			if( rand.NextFloat() > chance ) {
				return null;
			}

			var elements = ElementsConfig.Instance.Elements;
			float totalWeight = ElementDefinition.GetTotalElementNPCWeight();
			float weight = rand.NextFloat() * totalWeight;

			float countedWeights = 0f;
			for( int i = 0; i < elements.Count; i++ ) {
				countedWeights += elements[i].AutoAssignNPCWeight;
				if( weight < countedWeights ) {
					return elements[i];
				}
			}

			return null;
		}

		public static ElementDefinition PickDefinitionForItem( float chance ) {
			UnifiedRandom rand = TmlHelpers.SafelyGetRand();
			if( rand.NextFloat() > chance ) {
				return null;
			}

			var elements = ElementsConfig.Instance.Elements;
			float totalWeight = ElementDefinition.GetTotalElementItemWeight();
			float weight = rand.NextFloat() * totalWeight;

			float countedWeights = 0f;
			for( int i = 0; i < elements.Count; i++ ) {
				countedWeights += elements[i].AutoAssignItemWeight;
				if( weight < countedWeights ) {
					return elements[i];
				}
			}

			return null;
		}
	}
}
