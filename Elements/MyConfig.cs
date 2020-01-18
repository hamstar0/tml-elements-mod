using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader.Config;
using HamstarHelpers.Classes.UI.ModConfig;
using HamstarHelpers.Services.Configs;
using HamstarHelpers.Services.EntityGroups.Definitions;
using System.ComponentModel;


namespace Elements {
	class MyFloatInputElement : FloatInputElement { }




	public partial class ElementsConfig : StackableModConfig {
		public static ElementsConfig Instance => ModConfigStack.GetMergedConfigs<ElementsConfig>();



		////////////////
		
		public override ConfigScope Mode => ConfigScope.ServerSide;



		////////////////

		public List<ElementDefinition> Elements = new List<ElementDefinition> {
			new ElementDefinition {
				Name = "Heat",
				Color = Color.Red,
				StrongAgainst = new List<string> { "Cold" },
				WeakAgainst = new List<string> { "Water" }
			},
			new ElementDefinition {
				Name = "Cold",
				Color = Color.Cyan,
				StrongAgainst = new List<string> { "Water" },
				WeakAgainst = new List<string> { "Heat" }
			},
			new ElementDefinition {
				Name = "Water",
				Color = Color.Blue,
				StrongAgainst = new List<string> { "Heat" },
				WeakAgainst = new List<string> { "Cold" }
			},
		};


		////////////////

		[Range( 0f, 100f )]
		[DefaultValue( 2f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float ElementStrengthDamageMultiplier = 2f;

		[Range( 0f, 100f )]
		[DefaultValue( 0.25f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float ElementWeaknessDamageMultiplier = 0.25f;


		////////////////

		public Dictionary<ItemDefinition, float> AutoAssignedItems = new Dictionary<ItemDefinition, float>();

		public Dictionary<NPCDefinition, float> AutoAssignedNPCs = new Dictionary<NPCDefinition, float>();

		public Dictionary<string, float> AutoAssignedItemGroups = new Dictionary<string, float> {
			{ ItemGroupIDs.AnyWeapon, 0.5f }
		};

		public Dictionary<string, float> AutoAssignedNPCGroups = new Dictionary<string, float> {
			{ NPCGroupIDs.AnyHostileNPC, 0.5f }
		};
	}
}
