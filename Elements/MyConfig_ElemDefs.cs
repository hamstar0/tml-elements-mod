using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader.Config;
using HamstarHelpers.Services.Configs;


namespace Elements {
	public partial class ElementsConfig : StackableModConfig {
		public static ElementDefinition PhysicalElement => ElementsConfig.Instance.Elements[0];



		////////////////

		public List<ElementDefinition> Elements { get; set; } = new List<ElementDefinition> {
			new ElementDefinition {	// Warning: Physical 'element' MUST always exist!
				Name = ElementDefinition.PhysicalName,
				IconColor = new Color(128, 128, 128),
				GlowColor = Color.White,
				StrongAgainst = new List<string> { ElementDefinition.ColdName },
				WeakAgainst = new List<string> { ElementDefinition.WaterName },
				AutoAssignItemWeight = 0f,
				AutoAssignNPCWeight = 0f,
				DustType = -1,
				DustQuantity = 0,
				DustScale = 1f,
				DustColor = Color.White,
				IconTextureItem = new ItemDefinition( ItemID.ApprenticeBait )
			},
			new ElementDefinition {
				Name = ElementDefinition.HeatName,
				IconColor = Color.Red,
				GlowColor = Color.Red,
				StrongAgainst = new List<string> { ElementDefinition.ColdName, ElementDefinition.PhysicalName },
				WeakAgainst = new List<string> { ElementDefinition.WaterName },
				AutoAssignItemWeight = 1f,
				AutoAssignNPCWeight = 1f,
				DustType = 259,
				DustQuantity = 20,
				DustScale = 0.66f,
				DustColor = Color.White,
				IconTextureItem = new ItemDefinition( ItemID.LivingFireBlock )
			},
			new ElementDefinition {
				Name = ElementDefinition.ColdName,
				IconColor = new Color(160, 200, 255),
				GlowColor = new Color(160, 200, 255),
				StrongAgainst = new List<string> { ElementDefinition.WaterName },
				WeakAgainst = new List<string> { ElementDefinition.HeatName, ElementDefinition.PhysicalName },
				AutoAssignItemWeight = 1f,
				AutoAssignNPCWeight = 1f,
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
				AutoAssignItemWeight = 1f,
				AutoAssignNPCWeight = 1f,
				DustType = 33,
				DustQuantity = 20,
				DustScale = 1.5f,
				DustColor = Color.White,
				IconTextureItem = new ItemDefinition( ItemID.Sapphire )
			},
		};
	}
}
