using RecipeBrowserToMagicStorageExtra.Hooks;
using Terraria.ModLoader;

namespace RecipeBrowserToMagicStorageExtra
{
	public class RecipeBrowserToMagicStorageExtra : Mod
	{
		internal static RecipeBrowserToMagicStorageExtra Instance;
		internal static ModHotKey AutoRecallHotKey;

		public RecipeBrowserToMagicStorageExtra()
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