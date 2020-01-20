using System;
using System.ComponentModel;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader.Config;
using HamstarHelpers.Classes.UI.ModConfig;
using HamstarHelpers.Services.Configs;
using HamstarHelpers.Services.EntityGroups.Definitions;


namespace Elements {
	class MyFloatInputElement : FloatInputElement { }




	public partial class ElementsConfig : StackableModConfig {
		public static ElementsConfig Instance => ModConfigStack.GetMergedConfigs<ElementsConfig>();



		////////////////
		
		public override ConfigScope Mode => ConfigScope.ServerSide;



		////////////////

		public bool DebugModeInfo { get; set; } = false;
		public bool DebugModeReset { get; set; } = false;

		public List<ElementDefinition> Elements { get; set; } = new List<ElementDefinition> {
			new ElementDefinition {
				Name = "Heat",
				Color = new Color(255, 255, 128),
				StrongAgainst = new List<string> { "Cold" },
				WeakAgainst = new List<string> { "Water" },
				DustType = 259,
				DustQuantity = 20,
				DustScale = 0.66f
			},
			new ElementDefinition {
				Name = "Cold",
				Color = Color.Cyan,
				StrongAgainst = new List<string> { "Water" },
				WeakAgainst = new List<string> { "Heat" },
				DustType = 16,
				DustQuantity = 20,
				DustScale = 1f
			},
			new ElementDefinition {
				Name = "Water",
				Color = Color.Blue,
				StrongAgainst = new List<string> { "Heat" },
				WeakAgainst = new List<string> { "Cold" },
				DustType = 33,
				DustQuantity = 20,
				DustScale = 1f
			},
		};


		////////////////

		[Range( 0f, 100f )]
		[DefaultValue( 2f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float ElementStrengthDamageMultiplier { get; set; } = 2f;

		[Range( 0f, 100f )]
		[DefaultValue( 0.25f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float ElementWeaknessDamageMultiplier { get; set; } = 0.25f;


		////////////////

		public Dictionary<ItemDefinition, float> AutoAssignedItems { get; set; } = new Dictionary<ItemDefinition, float>();

		public Dictionary<NPCDefinition, float> AutoAssignedNPCs { get; set; } = new Dictionary<NPCDefinition, float>();

		public Dictionary<string, float> AutoAssignedItemGroups { get; set; } = new Dictionary<string, float> {
			{ ItemGroupIDs.AnyWeapon, 1f }//0.5f
		};

		public Dictionary<string, float> AutoAssignedNPCGroups { get; set; } = new Dictionary<string, float> {
			{ NPCGroupIDs.AnyHostileNPC, 1f }//0.5f
		};
	}
}
