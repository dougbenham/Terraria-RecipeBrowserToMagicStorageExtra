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
        internal static ModKeybind AutoRecallHotKey;

        public override void Load()
        {
            AutoRecallHotKey = KeybindLoader.RegisterKeybind(this, "Find in Storage", "LeftControl");

            RecipeBrowserHook.Load();
        }

        public override void Unload()
        {
            AutoRecallHotKey = null;

            RecipeBrowserHook.Unload();
        }
    }
}