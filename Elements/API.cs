using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Classes.Loadable;


namespace Elements {
	public class ElementsAPI : ILoadable {
		public static void AddItemInitializer( Func<Item, bool?> initializer ) {
			ModContent.GetInstance<ElementsAPI>().PreItemInitializers.Add( initializer );
		}

		public static void AddNPCInitializer( Func<NPC, bool?> initializer ) {
			ModContent.GetInstance<ElementsAPI>().PreNPCInitializers.Add( initializer );
		}


		////////////////

		internal static bool? PreItemInitialize( Item item ) {
			bool willAutoInit = false;

			foreach( var initializer in ModContent.GetInstance<ElementsAPI>().PreItemInitializers ) {
				bool? initState = initializer.Invoke( item );
				if( !initState.HasValue ) {
					continue;
				}

				willAutoInit = false;

				if( !initState.Value ) {
					break;
				}
			}

			return willAutoInit;
		}

		internal static bool PreNPCInitialize( NPC npc ) {
			bool willAutoInit = false;

			foreach( var initializer in ModContent.GetInstance<ElementsAPI>().PreNPCInitializers ) {
				bool? initState = initializer.Invoke( npc );
				if( !initState.HasValue ) {
					continue;
				}

				willAutoInit = false;

				if( !initState.Value ) {
					break;
				}
			}

			return willAutoInit;
		}



		////////////////

		internal IList<Func<Item, bool?>> PreItemInitializers = new List<Func<Item, bool?>>();

		internal IList<Func<NPC, bool?>> PreNPCInitializers = new List<Func<NPC, bool?>>();



		////////////////

		void ILoadable.OnModsLoad() { }

		void ILoadable.OnModsUnload() { }

		void ILoadable.OnPostModsLoad() { }
	}
}
