using System;
using System.ComponentModel;
using System.Collections.Generic;
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

		public Dictionary<ItemDefinition, float> AutoAssignedAnyItem { get; set; } = new Dictionary<ItemDefinition, float>();

		public Dictionary<NPCDefinition, float> AutoAssignedAnyNPC { get; set; } = new Dictionary<NPCDefinition, float>();

		public Dictionary<string, float> AutoAssignedAnyOfItemGroup { get; set; } = new Dictionary<string, float> {
			{ ItemGroupIDs.AnyWeapon, 0.5f }
		};

		public Dictionary<string, float> AutoAssignedAnyOfNPCGroup { get; set; } = new Dictionary<string, float> {
			{ NPCGroupIDs.AnyHostileNPC, 0.5f }
		};
	}
}
