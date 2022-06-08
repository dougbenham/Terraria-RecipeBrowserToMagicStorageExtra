using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using MagicStorageExtra;
using MagicStorageExtra.Components;
using MagicStorageExtra.UI;
using RecipeBrowserToMagicStorageExtra.Utils;
using Terraria;
using Terraria.ModLoader;

namespace RecipeBrowserToMagicStorageExtra.Hooks
{
    public static class MagicStorageReflection
    {
        public static void SetMagicStorageFilterName(string name)
        {
            Type type = null;
            var openedStorageType = GetCurrentOpenedStorageType();
            switch (openedStorageType)
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
            {
	            RecipeBrowserToMagicStorageExtra.Instance.Logger.Error("Couldn't find search bar on " + type?.Name);
	            return;
            }

            ReflectionUtils.SetProperty(searchBar, "Text", name);
            ReflectionUtils.SetField(searchBar, "cursorPosition", name.Length);
            StorageGUI.RefreshItems();

            if (openedStorageType == StorageType.Crafting)
            {
                var thread = new Thread(SelectFirstAvailableRecipe);
                thread.Start(name);
            }
        }

        private static void SelectFirstAvailableRecipe(object data)
        {
            try
            {
                var type = typeof(CraftingGUI);
                while (ReflectionUtils.GetField<bool>(null, "threadRunning", type))
                    Thread.Sleep(10);

                var threadRecipes = ReflectionUtils.GetField<List<Recipe>>(null, "threadRecipes", type);
                var threadRecipesAvailable = ReflectionUtils.GetField<List<bool>>(null, "threadRecipeAvailable", type);
                if (threadRecipes == null || threadRecipesAvailable == null)
                {
	                RecipeBrowserToMagicStorageExtra.Instance.Logger.Error("Couldn't find threadRecipes");
	                return;
                }

                var threadRecipesValid = new List<Recipe>();
                var threadRecipesAvailableValid = new List<bool>();
                var name = (string)data;

                for (var i = 0; i < threadRecipes.Count; i++)
                {
                    if (threadRecipes[i].createItem.Name != name)
                        continue;

                    threadRecipesValid.Add(threadRecipes[i]);
                    threadRecipesAvailableValid.Add(threadRecipesAvailable[i]);
                }

                var index = threadRecipesAvailableValid.IndexOf(true);
                var selectRecipe = index != -1 ? threadRecipesValid[index] : threadRecipesValid.FirstOrDefault();
                SelectRecipe(selectRecipe);
            }
            catch (Exception ex)
            {
	            RecipeBrowserToMagicStorageExtra.Instance.Logger.Error(null, ex);
            }
        }

        private static void SelectRecipe(Recipe selectRecipe)
        {
	        var method = typeof(CraftingGUI).GetMethod("SetSelectedRecipe", BindingFlags.NonPublic | BindingFlags.Static);
	        if (method == null)
	        {
		        RecipeBrowserToMagicStorageExtra.Instance.Logger.Error("Couldn't find SetSelectedRecipe");
		        return;
	        }
            method.Invoke(null, new object[] { selectRecipe });
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