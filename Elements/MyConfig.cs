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
				Name = ElementDefinition.PhysicalName,
				IconColor = new Color(128, 128, 128),
				GlowColor = Color.White,
				StrongAgainst = new List<string> { ElementDefinition.ColdName },
				WeakAgainst = new List<string> { ElementDefinition.WaterName },
				DustType = 259,
				DustQuantity = 20,
				DustScale = 0.66f,
				DustColor = Color.White,
				IconTextureItem = new ItemDefinition( ItemID.LivingFireBlock )
			},
			new ElementDefinition {
				Name = ElementDefinition.HeatName,
				IconColor = Color.Red,
				GlowColor = Color.Red,
				StrongAgainst = new List<string> { ElementDefinition.ColdName },
				WeakAgainst = new List<string> { ElementDefinition.WaterName },
				DustType = 259,
				DustQuantity = 20,
				DustScale = 0.66f,
				DustColor = Color.White,
				IconTextureItem = new ItemDefinition( ItemID.LivingFireBlock )
			},
			new ElementDefinition {
				Name = ElementDefinition.ColdName,
				IconColor = Color.White,
				GlowColor = new Color(128, 128, 128),
				StrongAgainst = new List<string> { ElementDefinition.WaterName },
				WeakAgainst = new List<string> { ElementDefinition.HeatName, ElementDefinition.PhysicalName },
				DustType = 16,
				DustQuantity = 20,
				DustScale = 1.5f,
				DustColor = Color.White,
				IconTextureItem = new ItemDefinition( ItemID.Snowball )
			},
			new ElementDefinition {
				Name = ElementDefinition.WaterName,
				IconColor = Color.Blue,
				GlowColor = new Color(16, 16, 255),
				StrongAgainst = new List<string> { ElementDefinition.HeatName, ElementDefinition.PhysicalName },
				WeakAgainst = new List<string> { ElementDefinition.ColdName },
				DustType = 33,
				DustQuantity = 20,
				DustScale = 1.5f,
				DustColor = Color.White,
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
