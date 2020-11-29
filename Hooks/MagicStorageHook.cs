using System;
using MagicStorage;
using MagicStorage.Components;
using RecipeBrowserToMagicStorage.Utils;
using Terraria;
using Terraria.ModLoader;

namespace RecipeBrowserToMagicStorage.Hooks
{
    public static class MagicStorageReflection
    {
        public static void SetMagicStorageFilterName(string name)
        {
            Type type = null;
            switch (GetCurrentOpenedStorageType())
            {
                case StorageType.None:
                    return;
                case StorageType.Crafting:
                    type = typeof(CraftingGUI);
                    break;
                case StorageType.Storage:
                    type = typeof(StorageGUI);
                    break;
            }

            var searchBar = ReflectionUtils.GetField<UISearchBar>(null, "searchBar", type);
            if (searchBar == null) 
                return;

            ReflectionUtils.SetField(searchBar, "text", name);
            ReflectionUtils.SetField(searchBar, "cursorPosition", name.Length);
            StorageGUI.RefreshItems();
        }

        private static StorageType GetCurrentOpenedStorageType()
        {
            var storageAccess = Main.player[Main.myPlayer].GetModPlayer<StoragePlayer>().ViewingStorage();

            if (!Main.playerInventory || storageAccess.X < 0 || storageAccess.Y < 0)
                return StorageType.None;

            var modTile = TileLoader.GetTile(Main.tile[storageAccess.X, storageAccess.Y].type);
            var heart = (modTile as StorageAccess)?.GetHeart(storageAccess.X, storageAccess.Y);
            if (heart == null)
                return StorageType.None;

            return modTile is CraftingAccess ? StorageType.Crafting : StorageType.Storage;
        }

        private enum StorageType
        {
            None,
            Crafting,
            Storage
        }

    }
}