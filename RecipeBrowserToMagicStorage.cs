using System;
using System.Reflection;
using MagicStorage;
using MagicStorage.Components;
using MonoMod.Cil;
using MonoMod.RuntimeDetour.HookGen;
using RecipeBrowserToMagicStorage.Hooks;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace RecipeBrowserToMagicStorage
{
	public class RecipeBrowserToMagicStorage : Mod
    {
        internal static ModHotKey AutoRecallHotKey;

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