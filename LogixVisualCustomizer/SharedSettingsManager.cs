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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        //the name is a lie, also this is inlined because i'm too lazy to move stuff around
        public static void DriveFromSharedSetting<T>(this IField<T> field, ModConfigurationKey<T> configurationKey,
            ModConfiguration config = null)
        {
            // :/
            field.Value = config.GetValue(configurationKey);
        }
    }
}