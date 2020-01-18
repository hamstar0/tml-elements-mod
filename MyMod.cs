using Terraria.ModLoader;


namespace Elements {
	public class ElementsMod : Mod {
		public static string GithubUserName => "hamstar0";
		public static string GithubProjectName => "tml-resistances-mod";


		////////////////

		public static ElementsMod Instance { get; private set; }



		////////////////

		public ElementsMod() {
			ElementsMod.Instance = this;
		}

		////////////////

		public override void Load() {
		}

		////

		public override void Unload() {
			ElementsMod.Instance = null;
		}
	}
}