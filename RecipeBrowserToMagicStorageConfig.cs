using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader.Config;

namespace RecipeBrowserToMagicStorage
{
#pragma warning restore 0649
    [Label("Config")]
    public class RecipeBrowserToMagicStorageConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        public static RecipeBrowserToMagicStorageConfig Instance;

        [Label("Use Hotkey")]
        [Tooltip("Changes filter only if a hot key is pressed.")]
        [DefaultValue(true)]
        public bool ByHotKey;
    }

#pragma warning disable 0649
}
