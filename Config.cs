using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace RecipeBrowserToMagicStorageExtra
{
#pragma warning restore 0649
    [Label("Config")]
    public class Config : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        public static Config Instance;

        [Label("By Hotkey")]
        [Tooltip("True - Changes only if a hot key is pressed. False - Changes always except if a hot key is pressed.")]
        [DefaultValue(true)]
        public bool ByHotKey;
    }

#pragma warning disable 0649
}
