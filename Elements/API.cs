using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Classes.Loadable;


namespace Elements {
	public class ElementsAPI : ILoadable {
		/// <summary>
		/// Adds an item initializer. Return `false` to abort all initialization and auto-initialization, `null` to use defaults
		/// (auto-initialize, if no other initializers interject), and `true` only skip auto-initialization.
		/// </summary>
		/// <param name="initializer"></param>
		public static void AddItemInitializer( Func<Item, bool?> initializer ) {
			ModContent.GetInstance<ElementsAPI>().PreItemInitializers.Add( initializer );
		}

		/// <summary>
		/// Adds an NPC initializer. Return `false` to abort all initialization and auto-initialization, `null` to use defaults
		/// (auto-initialize, if no other initializers interject), and `true` only skip auto-initialization.
		/// </summary>
		/// <param name="initializer"></param>
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
