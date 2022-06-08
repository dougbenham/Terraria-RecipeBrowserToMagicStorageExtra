using RecipeBrowserToMagicStorage.Hooks;
using Terraria.ModLoader;

namespace RecipeBrowserToMagicStorage
{
	public class RecipeBrowserToMagicStorage : Mod
	{
		internal static RecipeBrowserToMagicStorage Instance;
		internal static ModHotKey AutoRecallHotKey;

		public RecipeBrowserToMagicStorage()
		{
			Instance = this;
		}

        public override void Load()
        {
            AutoRecallHotKey = RegisterHotKey("Find in Storage", "LeftControl");

            RecipeBrowserHook.Load();
        }

        public override void Unload()
        {
            AutoRecallHotKey = null;

            RecipeBrowserHook.Unload();
        }
    }
}