using System;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader.Config;


namespace Elements {
	public partial class ElementDefinition {
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
