using System;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Utilities;
using Terraria.ModLoader.Config;
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



		////////////////

		public string Name;

		public Color Color;

		public List<string> StrongAgainst = new List<string>();

		public List<string> WeakAgainst = new List<string>();

		[Range( 0f, 100f )]
		[DefaultValue( 1f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float AutoAssignNPCWeight = 1f;

		[Range( 0f, 100f )]
		[DefaultValue( 1f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float AutoAssignItemWeight = 1f;



		////////////////

		public override int GetHashCode() {
			return this.Name?.GetHashCode() ?? 0
				+ this.Color.GetHashCode()
				+ this.StrongAgainst.Sum(d => d.GetHashCode())
				+ this.WeakAgainst.Sum(d => d.GetHashCode());
		}

		public override bool Equals( object obj ) {
			if( !( obj is ElementDefinition ) ) {
				return base.Equals( obj );
			}

			var elemObj = (ElementDefinition)obj;

			return elemObj.Name?.Equals(this.Name) ?? false
				&& elemObj.Color.Equals(this.Color)
				&& elemObj.StrongAgainst.SequenceEqual( this.StrongAgainst )
				&& elemObj.WeakAgainst.SequenceEqual( this.WeakAgainst );
		}
	}
}
