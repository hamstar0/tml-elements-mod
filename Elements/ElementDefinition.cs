using System;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader.Config;


namespace Elements {
	public partial class ElementDefinition {
		public string Name { get; set; }

		public Color Color { get; set; }

		public List<string> StrongAgainst { get; set; } = new List<string>();

		public List<string> WeakAgainst { get; set; } = new List<string>();

		[Range( 0f, 100f )]
		[DefaultValue( 1f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float AutoAssignNPCWeight { get; set; } = 1f;

		[Range( 0f, 100f )]
		[DefaultValue( 1f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float AutoAssignItemWeight { get; set; } = 1f;

		[Range( 0, 280 )]
		public int DustType { get; set; }

		[Range( 0, 200 )]
		[DefaultValue( 20 )]
		public int DustQuantity { get; set; }

		[Range( 0f, 15f )]
		[DefaultValue( 1f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float DustScale { get; set; }

		public ItemDefinition IconTextureItem { get; set; }



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
