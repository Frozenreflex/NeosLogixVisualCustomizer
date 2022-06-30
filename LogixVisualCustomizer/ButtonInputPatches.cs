﻿using BaseX;
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
    [HarmonyPatch]
    internal static class ButtonInputPatches
    {
        private static readonly Type ImpulseInputType = typeof(ImpulseInput);

        [HarmonyPostfix]
        private static void OnGenerateVisualPostfix(LogixNode __instance, Slot root)
        {
            var buttons = root.GetComponentsInChildren<Button>(LogixVisualCustomizer.ButtonFilter).ToArray();

            if (__instance.GetType() == ImpulseInputType)
            {
                buttons[0].Customize();
                return;
            }
            buttons.CustomizeVertical();
        }

        [HarmonyTargetMethods]
        private static IEnumerable<MethodBase> TargetMethods()
        {
            return LogixVisualCustomizer.GenerateMethodTargets(
                "OnGenerateVisual",
                typeof(BoolInput), typeof(Bool2Input),
                typeof(Bool3Input), typeof(Bool4Input),
                ImpulseInputType);
        }
    }
}