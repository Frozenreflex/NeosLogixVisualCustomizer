using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.LogiX.Input;
using FrooxEngine.UIX;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LogixVisualCustomizer
{
    internal static class EnumInputPatch
    {
        private static readonly MethodInfo OnGenerateVisualPatch =
            typeof(EnumInputPatch).GetMethod(nameof(OnGenerateVisualPostfix), AccessTools.all);
        public static void Patch(Harmony harmony)
        {
            var baseType = typeof(EnumInput<>);
            foreach (var type in LogixVisualCustomizer.NeosEnumTypes)
                harmony.Patch(baseType.MakeGenericType(type).GetMethod("OnGenerateVisual",
                        AccessTools.allDeclared),
                    postfix: new HarmonyMethod(OnGenerateVisualPatch));
        }
        private static void OnGenerateVisualPostfix(Slot root)
        {
            var buttons = root.GetComponentsInChildren<Button>(LogixVisualCustomizer.ButtonFilter).ToArray();
            foreach (var button in buttons) button.Customize();
            root.ForeachComponentInChildren<Text>(VisualCustomizing.CustomizeDisplay);
        }
    }
}