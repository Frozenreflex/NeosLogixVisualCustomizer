/*
using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogixVisualCustomizer
{
    internal static class SettingOverrides
    {
        public enum Settings
        {
            InputBackgroundColor,
            InputBorderColor,
            NodeBackgroundColor,
            NodeBorderColor,
            TextColor
        }
        //this class now does nothing
        public static void OverrideWith<T>(this Sync<T> field, Settings setting)
        {
            if (typeof(T) != typeof(color)) return;
            var toField = field as Sync<color>;
            color value;
            switch (setting)
            {
                case Settings.InputBackgroundColor:
                    value = LogixVisualCustomizer.InputBackgroundColor;
                    break;
                case Settings.InputBorderColor:
                    value = LogixVisualCustomizer.InputBorderColor;
                    break;
                case Settings.NodeBackgroundColor:
                    value = LogixVisualCustomizer.NodeBackgroundColor;
                    break;
                case Settings.NodeBorderColor:
                    value = LogixVisualCustomizer.NodeBorderColor;
                    break;
                case Settings.TextColor:
                    value = LogixVisualCustomizer.TextColor;
                    break;
                default:
                    return;
            }
            toField.Value = value;
        }
    }
}
*/