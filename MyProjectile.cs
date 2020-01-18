using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;


namespace Elements {
	class ElementsProjectile : GlobalProjectile {
		public static bool CanHaveElements( Projectile proj ) {
			return proj.damage > 0;
		}



		////////////////

		public override bool CloneNewInstances => false;


		////////////////
		
		public bool IsInitialized { get; internal set; } = false;
		public ISet<ElementDefinition> Elements { get; internal set; } = new HashSet<ElementDefinition>();



		////////////////

		public void SetElements( Projectile proj, ISet<ElementDefinition> elements ) {
			var myproj = proj.GetGlobalProjectile<ElementsProjectile>();

			myproj.IsInitialized = true;
			myproj.Elements = elements;
		}
	}
}
