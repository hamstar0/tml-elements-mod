using System;
using System.ComponentModel;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
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
				Color = Color.Red,
				StrongAgainst = new List<string> { "Cold" },
				WeakAgainst = new List<string> { "Water" },
				DustType = 259,
				DustQuantity = 20,
				DustScale = 0.66f,
				IconTextureItem = new ItemDefinition( ItemID.LivingFireBlock )
			},
			new ElementDefinition {
				Name = "Cold",
				Color = new Color(128, 128, 128),
				StrongAgainst = new List<string> { "Water" },
				WeakAgainst = new List<string> { "Heat" },
				DustType = 16,
				DustQuantity = 20,
				DustScale = 1f,
				IconTextureItem = new ItemDefinition( ItemID.Snowball )
			},
			new ElementDefinition {
				Name = "Water",
				Color = new Color(16, 16, 255),
				StrongAgainst = new List<string> { "Heat" },
				WeakAgainst = new List<string> { "Cold" },
				DustType = 33,
				DustQuantity = 20,
				DustScale = 1f,
				IconTextureItem = new ItemDefinition( ItemID.Sapphire )
			},
		};


		////////////////

		[Range( 0f, 100f )]
		[DefaultValue( 2f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float ElementAfflictDamageMultiplier { get; set; } = 2f;

		[Range( 0f, 100f )]
		[DefaultValue( 0f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float ElementEqualDamageMultiplier { get; set; } = 0f;

		[Range( 0f, 100f )]
		[DefaultValue( 0.25f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float ElementAbsorbDamageMultiplier { get; set; } = 0.25f;


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
