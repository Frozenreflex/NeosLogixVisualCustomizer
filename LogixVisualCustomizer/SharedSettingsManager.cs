using FrooxEngine;
using HarmonyLib;
using NeosModLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LogixVisualCustomizer
{
    internal static class SharedSettingsManager
    {
        //the name is a lie
        public static void DriveFromSharedSetting<T>(this IField<T> field, ModConfigurationKey<T> configurationKey,
            ModConfiguration config = null)
        {
            if (configurationKey != null) field.Value = config.GetValue(configurationKey);
        }
    }
}