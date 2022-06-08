using System;
using System.Reflection;
using MonoMod.RuntimeDetour.HookGen;
using RecipeBrowserToMagicStorageExtra.Utils;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace RecipeBrowserToMagicStorageExtra.Hooks
{
    public static class RecipeBrowserHook
    {
        private const string ModName = "RecipeBrowser";
        private const string TypeName1 = "UIRecipeSlot";
        private const string TypeName2 = "UIIngredientSlot";
        private const string TypeName3 = "UIItemCatalogueItemSlot";

        private const string MethodName = "Click";

        private delegate void HookClick(object self, UIMouseEvent e);

        private static MethodInfo[] RecipeSlotOnClickMethods { get; set; } = new MethodInfo[3];
        private static string[] TypeNames { get; set; } = new string[3];

        public static void Load()
        {
            TypeNames = new[] {TypeName1, TypeName2, TypeName3};

            var recipeBrowserAssembly = ModLoader.GetMod(ModName)?.GetType().Assembly;
            var recipeSlotTypes = new Type[TypeNames.Length];
            for (var i = 0; i < recipeSlotTypes.Length; i++)
                recipeSlotTypes[i] = ReflectionUtils.FindType(recipeBrowserAssembly, TypeNames[i]);

            RecipeSlotOnClickMethods = new MethodInfo[TypeNames.Length];
            for (var i = 0; i < RecipeSlotOnClickMethods.Length; i++)
                RecipeSlotOnClickMethods[i] = ReflectionUtils.GetMethodInfo(recipeSlotTypes[i], MethodName);

            Register();
        }
        public static void Unload()
        {
            UnRegister();
            RecipeSlotOnClickMethods = null;
            TypeNames = null;
        }

        private static void UnRegister()
        {
            for (var i = 0; i < RecipeSlotOnClickMethods.Length; i++)
                if (RecipeSlotOnClickMethods[i] != null)
                    HookEndpointManager.Remove(RecipeSlotOnClickMethods[i], (HookClick)OnClickHook);
        }

        private static void Register()
        {
            for (var i = 0; i < RecipeSlotOnClickMethods.Length; i++)
                if (RecipeSlotOnClickMethods[i] != null)
                    HookEndpointManager.Add(RecipeSlotOnClickMethods[i], (HookClick) OnClickHook);
        }
        private static void InvokeBase(object self, UIMouseEvent e)
        {
            UnRegister();

            var type = self.GetType();
            var method =  ReflectionUtils.GetMethodInfo(type, MethodName);
            method?.Invoke(self, new[] { e });

            Register();
        }

        private static void OnClickHook(object self, UIMouseEvent e)
        {
            InvokeBase(self, e);

            if ((!Config.Instance.ByHotKey ||
                 !RecipeBrowserToMagicStorageExtra.AutoRecallHotKey.Current) &&
                (Config.Instance.ByHotKey ||
                 RecipeBrowserToMagicStorageExtra.AutoRecallHotKey.Current)) 
                return;

            var item = ReflectionUtils.GetField<Item>(e.Target, "item");
            if (item != null)
                MagicStorageReflection.SetMagicStorageFilterName(item.Name);
        }
    }
}