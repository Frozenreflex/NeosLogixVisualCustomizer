using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BaseX;
using CloudX.Shared;
using CodeX;
using FrooxEngine;
using FrooxEngine.LogiX.Input;
using FrooxEngine.UIX;
using HarmonyLib;
using NeosModLoader;

namespace LogixVisualCustomizer
{
    public class LogixVisualCustomizer : NeosMod
    {
        public static readonly Type[] NeosEnumTypes;
        public static readonly Type[] NeosPrimitiveAndEnumTypes;
        public static readonly Type[] NeosPrimitiveTypes;
        public static readonly MethodInfo PrimitiveMemberEditorOnReset =
            typeof(PrimitiveMemberEditor).GetMethod("OnReset", AccessTools.allDeclared);
        public static readonly MethodInfo RefEditorRemovePressed =
            typeof(RefEditor).GetMethod("RemovePressed", AccessTools.allDeclared);
        public static readonly MethodInfo RefEditorSetReference =
            typeof(RefEditor).GetMethod("SetReference", AccessTools.allDeclared);
        public static ModConfiguration Config;
        private static readonly float4 DefaultSlices = new float4(0, 0, 1, 1);

        [AutoRegisterConfigKey]
        internal static ModConfigurationKey<float4> BackgroundHorizontalSlicesKey =
            new ModConfigurationKey<float4>("BackgroundHorizontalSlices",
                "Background Horizontal Slices",
                () => new float4(0, .5f, .5f, 1));

        [AutoRegisterConfigKey]
        internal static ModConfigurationKey<TextureFilterMode> BackgroundSpriteFilterKey =
            new ModConfigurationKey<TextureFilterMode>("BackgroundSpriteFilter", "Background Sprite Filter",
                () => TextureFilterMode.Anisotropic);

        [AutoRegisterConfigKey]
        internal static ModConfigurationKey<Uri> BackgroundSpriteUriKey = new ModConfigurationKey<Uri>(
            "BackgroundSpriteUri", "",
            () => new Uri("neosdb:///1e64bbda2fb62373fd3b82ae4f96a60daebaff81d690c96bbe03d10871221209.png"), true);
        
        [AutoRegisterConfigKey]
        internal static ModConfigurationKey<string> BackgroundSpriteUriStringKey =
            new ModConfigurationKey<string>("BackgroundSpriteUriString", "Background Sprite URI",
                () => "neosdb:///1e64bbda2fb62373fd3b82ae4f96a60daebaff81d690c96bbe03d10871221209.png");

        [AutoRegisterConfigKey]
        internal static ModConfigurationKey<float4> BackgroundVerticalSlicesKey =
            new ModConfigurationKey<float4>("BackgroundVerticalSlices",
                "Background Vertical Slices",
                () => new float4(0, .5f, .5f, 1));

        [AutoRegisterConfigKey]
        internal static ModConfigurationKey<float4> BorderHorizontalSlicesKey =
            new ModConfigurationKey<float4>("BorderHorizontalSlicesKey",
                "Border Horizontal Slices",
                () => new float4(0, .5f, .5f, 1));

        [AutoRegisterConfigKey]
        internal static ModConfigurationKey<TextureFilterMode> BorderSpriteFilterKey =
            new ModConfigurationKey<TextureFilterMode>("BorderSpriteFilter", "Border Sprite Filter",
                () => TextureFilterMode.Anisotropic);

        [AutoRegisterConfigKey]
        internal static ModConfigurationKey<Uri> BorderSpriteUriKey = new ModConfigurationKey<Uri>("BorderSpriteUri",
            "",
            () => new Uri("neosdb:///518299baeefe744aa609c9b2c77c5930b6593c051b38eba116ff9177e8200a4f.png"), true);
        
        [AutoRegisterConfigKey]
        internal static ModConfigurationKey<string> BorderSpriteUriStringKey =
            new ModConfigurationKey<string>("BorderSpriteUriString", "Border Sprite URI",
                () => "neosdb:///518299baeefe744aa609c9b2c77c5930b6593c051b38eba116ff9177e8200a4f.png");

        [AutoRegisterConfigKey]
        internal static ModConfigurationKey<float4> BorderVerticalSlicesKey =
            new ModConfigurationKey<float4>("BorderVerticalSlices",
                "Border Vertical Slices",
                () => new float4(0, .5f, .5f, 1));

        [AutoRegisterConfigKey]
        internal static ModConfigurationKey<bool> EnableCustomLogixVisualsKey =
            new ModConfigurationKey<bool>("EnableCustomLogixVisuals",
                "Enabled", () => true);

        [AutoRegisterConfigKey]
        internal static ModConfigurationKey<bool> EnableLeftNullButtonKey =
            new ModConfigurationKey<bool>("EnableLeftNullButton", "Left Null Buttons",
                () => true);

        [AutoRegisterConfigKey]
        internal static ModConfigurationKey<bool> IndividualInputsKey =
            new ModConfigurationKey<bool>("IndividualInputs",
                "Split Inputs",
                () => false);

        [AutoRegisterConfigKey]
        internal static ModConfigurationKey<color> InputBackgroundColorKey =
            new ModConfigurationKey<color>("InputBackgroundColor", "Node Input Background Color",
                () => new color(.26f));

        [AutoRegisterConfigKey]
        internal static ModConfigurationKey<float> InputBackgroundScaleKey =
            new ModConfigurationKey<float>("InputBackgroundScale", "Node Input Background Scale",
                () => .03f);

        [AutoRegisterConfigKey]
        internal static ModConfigurationKey<color> InputBorderColorKey =
            new ModConfigurationKey<color>("InputBorderColor", "Node Input Border Color",
                () => new color(.16f));

        [AutoRegisterConfigKey]
        internal static ModConfigurationKey<float> InputBorderScaleKey =
            new ModConfigurationKey<float>("InputBorderScale", "Node Input Border Scale",
                () => .03f);

        [AutoRegisterConfigKey]
        internal static ModConfigurationKey<color> NodeBackgroundColorKey =
            new ModConfigurationKey<color>("NodeBackgroundColor", "Node Background Color",
                () => new color(.22f));

        [AutoRegisterConfigKey]
        internal static ModConfigurationKey<float> NodeBackgroundScaleKey =
            new ModConfigurationKey<float>("NodeBackgroundScale", "Node Background Scale",
                () => .03f);

        [AutoRegisterConfigKey]
        internal static ModConfigurationKey<color> NodeBorderColorKey =
            new ModConfigurationKey<color>("NodeBorderColor", "Node Border Color", () => new color(.54f));

        [AutoRegisterConfigKey]
        internal static ModConfigurationKey<float> NodeBorderScaleKey =
            new ModConfigurationKey<float>("NodeBorderScale", "Node Border Scale", () => .03f);

        [AutoRegisterConfigKey]
        internal static ModConfigurationKey<color> TextColorKey =
            new ModConfigurationKey<color>("TextColor", "Text Color", () => new color(.95f));
        
        [AutoRegisterConfigKey]
        internal static ModConfigurationKey<Uri> TextFontUriKey =
            new ModConfigurationKey<Uri>("TextMainFontUri", "",
                () => new Uri("neosdb:///08a5db276a5e8a6a30ae8af3618356d093e288776f043849d1d01a9bcb12fc37.ttf"), true);
        [AutoRegisterConfigKey]
        internal static ModConfigurationKey<Uri> TextFontSecondaryUriKey =
            new ModConfigurationKey<Uri>("TextSecondaryFontUri", "",
                () => NeosAssets.Graphics.Fonts.Noto_Emoji.NotoEmoji_Regular, true);
        [AutoRegisterConfigKey]
        internal static ModConfigurationKey<Uri> TextFontTertiaryUriKey =
            new ModConfigurationKey<Uri>("TextTertiaryFontUri", "",
                () => NeosAssets.Graphics.Fonts.Noto_Sans.NotoSansCJKjp_Medium, true);
        [AutoRegisterConfigKey]
        internal static ModConfigurationKey<Uri> TextFontQuaternaryUriKey =
            new ModConfigurationKey<Uri>("TextQuaternaryFontUri", "", () => null, true);
        
        [AutoRegisterConfigKey]
        internal static ModConfigurationKey<string> TextFontUriKeyString =
            new ModConfigurationKey<string>("TextMainFontUriString", "Text Primary Font URI",
                () => "neosdb:///08a5db276a5e8a6a30ae8af3618356d093e288776f043849d1d01a9bcb12fc37.ttf");
        [AutoRegisterConfigKey]
        internal static ModConfigurationKey<string> TextFontSecondaryUriKeyString =
            new ModConfigurationKey<string>("TextSecondaryFontUriString", "Text Secondary Font URI",
                () => NeosAssets.Graphics.Fonts.Noto_Emoji.NotoEmoji_Regular.ToString());
        [AutoRegisterConfigKey]
        internal static ModConfigurationKey<string> TextFontTertiaryUriKeyString =
            new ModConfigurationKey<string>("TextTertiaryFontUriString", "Text Tertiary Font URI",
                () => NeosAssets.Graphics.Fonts.Noto_Sans.NotoSansCJKjp_Medium.ToString());
        [AutoRegisterConfigKey]
        internal static ModConfigurationKey<string> TextFontQuaternaryUriKeyString =
            new ModConfigurationKey<string>("TextQuaternaryFontUriString", "Text Quaternary Font URI", () => "");
        
        [AutoRegisterConfigKey]
        internal static ModConfigurationKey<int> TextFontGlyphEmSizeKey =
            new ModConfigurationKey<int>("TextFontGlyphEmSizeKey", "Text Font GlyphEm Size",
                () => 64);

        public override string Author => "Banane9, Fro Zen";
        public override string Link => "https://github.com/Frozenreflex/NeosLogixVisualCustomizer";
        public override string Name => "LogixVisualCustomizer";
        public override string Version => "1.0.1-1";
        internal static float4 BackgroundHorizontalSlices =>
            UseBackground ? Config.GetValue(BackgroundHorizontalSlicesKey) : DefaultSlices;
        internal static Uri BackgroundSpriteUri => UseBackground ? Config.GetValue(BackgroundSpriteUriKey) : null;
        internal static float4 BackgroundVerticalSlices =>
            UseBackground ? Config.GetValue(BackgroundVerticalSlicesKey) : DefaultSlices;
        internal static float4 BorderHorizontalSlices =>
            UseBorder ? Config.GetValue(BorderHorizontalSlicesKey) : DefaultSlices;
        internal static Uri BorderSpriteUri => UseBorder ? Config.GetValue(BorderSpriteUriKey) : null;
        internal static float4 BorderVerticalSlices =>
            UseBorder ? Config.GetValue(BorderVerticalSlicesKey) : DefaultSlices;
        internal static float4 BottomBackgroundBorders =>
            Slices.GetBottomBorders(BackgroundVerticalSlices, BackgroundHorizontalSlices);
        internal static Rect BottomBackgroundRect =>
            Slices.GetBottomRect(BackgroundVerticalSlices, BackgroundHorizontalSlices);
        internal static float4 BottomBorderBorders =>
            Slices.GetBottomBorders(BorderVerticalSlices, BorderHorizontalSlices);
        internal static Rect BottomBorderRect => Slices.GetBottomRect(BorderVerticalSlices, BorderHorizontalSlices);
        internal static bool EnableCustomLogixVisuals => Config.GetValue(EnableCustomLogixVisualsKey);
        internal static bool EnableLeftNullButton => Config.GetValue(EnableLeftNullButtonKey);
        internal static float4 FullBackgroundBorders =>
            Slices.GetFullBorders(BackgroundVerticalSlices, BackgroundHorizontalSlices);
        internal static Rect FullBackgroundRect =>
            Slices.GetFullRect(BackgroundVerticalSlices, BackgroundHorizontalSlices);
        internal static float4 FullBorderBorders => Slices.GetFullBorders(BorderVerticalSlices, BorderHorizontalSlices);
        internal static Rect FullBorderRect => Slices.GetFullRect(BorderVerticalSlices, BorderHorizontalSlices);
        internal static float4 HorizontalMiddleBackgroundBorders =>
            Slices.GetHorizontalMiddleBorders(BackgroundHorizontalSlices);
        internal static Rect HorizontalMiddleBackgroundRect =>
            Slices.GetHorizontalMiddleRect(BackgroundVerticalSlices, BackgroundHorizontalSlices);
        internal static float4 HorizontalMiddleBorderBorders =>
            Slices.GetHorizontalMiddleBorders(BorderHorizontalSlices);
        internal static Rect HorizontalMiddleBorderRect =>
            Slices.GetHorizontalMiddleRect(BorderVerticalSlices, BorderHorizontalSlices);
        internal static bool IndividualInputs => Config.GetValue(IndividualInputsKey);
        internal static color InputBackgroundColor => Config.GetValue(InputBackgroundColorKey);
        internal static float InputBackgroundScale => Config.GetValue(InputBackgroundScaleKey);
        internal static color InputBorderColor => Config.GetValue(InputBorderColorKey);
        internal static float InputBorderScale => Config.GetValue(InputBorderScaleKey);
        internal static float4 LeftBackgroundBorders =>
            Slices.GetLeftBorders(BackgroundVerticalSlices, BackgroundHorizontalSlices);
        internal static Rect LeftBackgroundRect =>
            Slices.GetLeftRect(BackgroundVerticalSlices, BackgroundHorizontalSlices);
        internal static float4 LeftBorderBorders => Slices.GetLeftBorders(BorderVerticalSlices, BorderHorizontalSlices);
        internal static Rect LeftBorderRect => Slices.GetLeftRect(BorderVerticalSlices, BorderHorizontalSlices);
        internal static color NodeBackgroundColor => Config.GetValue(NodeBackgroundColorKey);
        internal static float NodeBackgroundScale => Config.GetValue(NodeBackgroundScaleKey);
        internal static color NodeBorderColor => Config.GetValue(NodeBorderColorKey);
        internal static float NodeBorderScale => Config.GetValue(NodeBorderScaleKey);
        internal static float4 RightBackgroundBorders =>
            Slices.GetRightBorders(BackgroundVerticalSlices, BackgroundHorizontalSlices);
        internal static Rect RightBackgroundRect =>
            Slices.GetRightRect(BackgroundVerticalSlices, BackgroundHorizontalSlices);
        internal static float4 RightBorderBorders =>
            Slices.GetRightBorders(BorderVerticalSlices, BorderHorizontalSlices);
        internal static Rect RightBorderRect => Slices.GetRightRect(BorderVerticalSlices, BorderHorizontalSlices);
        internal static color TextColor => Config.GetValue(TextColorKey);
        internal static float4 TopBackgroundBorders =>
            Slices.GetTopBorders(BackgroundVerticalSlices, BackgroundHorizontalSlices);
        internal static Rect TopBackgroundRect =>
            Slices.GetTopRect(BackgroundVerticalSlices, BackgroundHorizontalSlices);
        internal static float4 TopBorderBorders => Slices.GetTopBorders(BorderVerticalSlices, BorderHorizontalSlices);
        internal static Rect TopBorderRect => Slices.GetTopRect(BorderVerticalSlices, BorderHorizontalSlices);
        internal static bool UseBackground => Config.GetValue(BackgroundSpriteUriKey) != null;
        internal static bool UseBorder => Config.GetValue(BorderSpriteUriKey) != null;
        internal static float4 VerticalMiddleBackgroundBorders =>
            Slices.GetVerticalMiddleBorders(BackgroundVerticalSlices);
        internal static Rect VerticalMiddleBackgroundRect =>
            Slices.GetVerticalMiddleRect(BackgroundVerticalSlices, BackgroundHorizontalSlices);
        internal static float4 VerticalMiddleBorderBorders => Slices.GetVerticalMiddleBorders(BorderVerticalSlices);
        internal static Rect VerticalMiddleBorderRect =>
            Slices.GetVerticalMiddleRect(BorderVerticalSlices, BorderHorizontalSlices);

        internal static Dictionary<ModConfigurationKey, ModConfigurationKey<Uri>> UriKeys =
            new Dictionary<ModConfigurationKey, ModConfigurationKey<Uri>>
            {
                {BackgroundSpriteUriStringKey, BackgroundSpriteUriKey},
                {BorderSpriteUriStringKey, BorderSpriteUriKey},
                {TextFontUriKeyString, TextFontUriKey},
                {TextFontSecondaryUriKeyString, TextFontSecondaryUriKey},
                {TextFontTertiaryUriKeyString, TextFontTertiaryUriKey},
                {TextFontQuaternaryUriKeyString, TextFontQuaternaryUriKey},
            };

        static LogixVisualCustomizer()
        {
            var traverse = Traverse.Create(typeof(GenericTypes));

            NeosEnumTypes = AccessTools.GetTypesFromAssembly(typeof(EnumInput<>).Assembly)
                                .Concat(AccessTools.GetTypesFromAssembly(typeof(float4).Assembly))
                                .Concat(AccessTools.GetTypesFromAssembly(typeof(AudioDistanceSpace).Assembly))
                                .Concat(AccessTools.GetTypesFromAssembly(typeof(WrapMode).Assembly))
                                .Concat(AccessTools.GetTypesFromAssembly(typeof(SessionAccessLevel).Assembly))
                                .Distinct()
                                .Where(type => type.IsEnum && !type.IsNested)
                                .ToArray();

            NeosPrimitiveTypes = traverse.Field<Type[]>("neosPrimitives").Value
                                    .Where(type => type.Name != "String")
                                    .AddItem(typeof(dummy))
                                    .AddItem(typeof(object))
                                    .ToArray();

            NeosPrimitiveAndEnumTypes = traverse.Field<Type[]>("neosPrimitivesAndEnums").Value
                                            .Where(type => type.Name != "String")
                                            .AddItem(typeof(dummy))
                                            .AddItem(typeof(object))
                                            .ToArray();
        }

        public static bool ButtonFilter(Button button) => button.ColorDrivers.Count > 0;

        public static IEnumerable<MethodBase> GenerateGenericMethodTargets(IEnumerable<Type> genericTypes,
            string methodName, params Type[] baseTypes) =>
            GenerateGenericMethodTargets(genericTypes, methodName, (IEnumerable<Type>) baseTypes);

        public static IEnumerable<MethodBase> GenerateGenericMethodTargets(IEnumerable<Type> genericTypes,
            string methodName, IEnumerable<Type> baseTypes) =>
            GenerateMethodTargets(methodName,
                genericTypes.SelectMany(type => baseTypes.Select(baseType => baseType.MakeGenericType(type))));

        public static IEnumerable<MethodBase> GenerateMethodTargets(string methodName, params Type[] baseTypes) =>
            GenerateMethodTargets(methodName, (IEnumerable<Type>) baseTypes);

        public static IEnumerable<MethodBase> GenerateMethodTargets(string methodName, IEnumerable<Type> baseTypes) =>
            baseTypes.Select(type => type.GetMethod(methodName, AccessTools.all)).Where(m => m != null);

        public override void OnEngineInit()
        {
            Config = GetConfiguration();
            Config.OnThisConfigurationChanged += OnConfigurationChanged;
            Config.Save(true);

            var harmony = new Harmony($"{Author}.{Name}");
            harmony.PatchAll();
            TextFieldPatch.Patch(harmony);
            EnumInputPatch.Patch(harmony);
        }
        
        private void OnConfigurationChanged(ConfigurationChangedEvent changeEvent)
        {
            if (!UriKeys.TryGetValue(changeEvent.Key, out var value)) return;
            if (!Uri.TryCreate((string) Config.GetValue(changeEvent.Key), UriKind.Absolute, out var uri)) uri = null;
            Config.Set(value, uri);
        }
    }
}