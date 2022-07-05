using BaseX;
using FrooxEngine;
using NeosModLoader;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace LogixVisualCustomizer
{
    internal enum FontAssetKey
    {
        Main,
    }
    internal enum TextureAssetKey
    {
        BackgroundTexture,
        BorderTexture,
    }

    internal enum SpriteAssetKey
    {
        BottomInputBackground,
        BottomInputBorder,
        FullInputBackground,
        FullInputBorder,
        HorizontalMiddleBackground,
        HorizontalMiddleBorder,
        LeftInputBackground,
        LeftInputBorder,
        NodeBackground,
        NodeBorder,
        RightInputBackground,
        RightInputBorder,
        TopInputBackground,
        TopInputBorder,
        VerticalMiddleBackground,
        VerticalMiddleBorder
    }
    internal static class Assets
    {
        private static World LastWorld;
        private static Slot LastSlot;
        private static SyncRefList<IAssetProvider<ITexture2D>> LastTextures;
        //private static ReferenceMultiplexer<IAssetProvider<ITexture2D>> LastTextures;
        private static SyncRefList<SpriteProvider> LastSprites;
        //private static ReferenceMultiplexer<SpriteProvider> LastSprites;
        private static SyncRefList<FontChain> LastFonts;
        //private static ReferenceMultiplexer<FontChain> LastFonts;
        private static ValueField<int> LastIdentifier;
        private static Dictionary<string, string> UserHashDictionary = new Dictionary<string, string>();
        private static int CurrentIdentifier => 2;

        private static string GetUserHash(string user)
        {
            if (UserHashDictionary.TryGetValue(user, out var hash)) return hash;
            //this doesn't need to be completely cryptographically secure or anything like that,
            //but blatantly showing the userID does feel kinda wrong to me
            hash = BitConverter.ToString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(user)))
                .Replace("-", string.Empty).Substring(0, 8);
            //cache the hash so we don't compute a new one every time
            UserHashDictionary.Add(user, hash);
            return hash;
        }
        public static IAssetProvider<ITexture2D> GetBackgroundTexture(this World world) => world.GetOrCreateTexture(
            TextureAssetKey.BackgroundTexture, LogixVisualCustomizer.BackgroundSpriteUriKey,
            LogixVisualCustomizer.BackgroundSpriteFilterKey);

        public static IAssetProvider<ITexture2D> GetBorderTexture(this World world) => world.GetOrCreateTexture(
            TextureAssetKey.BorderTexture, LogixVisualCustomizer.BorderSpriteUriKey,
            LogixVisualCustomizer.BorderSpriteFilterKey);

        public static SpriteProvider GetBottomInputBackgroundProvider(this World world) =>
            world.GetOrCreateSpriteProvider(SpriteAssetKey.BottomInputBackground,
                world.GetBackgroundTexture(),
                LogixVisualCustomizer.IndividualInputs
                    ? LogixVisualCustomizer.FullBackgroundRect
                    : LogixVisualCustomizer.BottomBackgroundRect,
                LogixVisualCustomizer.IndividualInputs
                    ? LogixVisualCustomizer.FullBackgroundBorders
                    : LogixVisualCustomizer.BottomBackgroundBorders,
                LogixVisualCustomizer.InputBackgroundScale);

        public static SpriteProvider GetBottomInputBorderProvider(this World world) =>
            world.GetOrCreateSpriteProvider(SpriteAssetKey.BottomInputBorder,
                world.GetBorderTexture(),
                LogixVisualCustomizer.IndividualInputs
                    ? LogixVisualCustomizer.FullBorderRect
                    : LogixVisualCustomizer.BottomBorderRect,
                LogixVisualCustomizer.IndividualInputs
                    ? LogixVisualCustomizer.FullBorderBorders
                    : LogixVisualCustomizer.BottomBorderBorders,
                LogixVisualCustomizer.InputBorderScale);

        public static Slot GetCustomizerAssets(this World world)
        {
            var user = world.LocalUser.UserID;
            var hash = GetUserHash(user);
            var key = $"LogixCustomizerAssets_{hash}";
            
            if (world.AssetsSlot.Find(key) is Slot slot)
                return slot;

            return world.CreateAssetSlot(hash);
        }
        private static Slot CreateAssetSlot(this World world, string key)
        {
            var slot = world.AssetsSlot.AddSlot($"LogixCustomizerAssets_{key}");
            slot.AttachComponent<ValueField<int>>().Value.Value = CurrentIdentifier;
            slot.AttachComponent<AssetOptimizationBlock>();
            slot.AttachComponent<ReferenceMultiplexer<IAssetProvider<ITexture2D>>>().References
                .AddRange(new IAssetProvider<ITexture2D>[Enum.GetValues(typeof(TextureAssetKey)).Length]);
            slot.AttachComponent<ReferenceMultiplexer<SpriteProvider>>().References
                .AddRange(new SpriteProvider[Enum.GetValues(typeof(SpriteAssetKey)).Length]);
            slot.AttachComponent<ReferenceMultiplexer<FontChain>>().References
                .AddRange(new FontChain[Enum.GetValues(typeof(FontAssetKey)).Length]);
            return slot;
        }

        public static SpriteProvider GetFullInputBackgroundProvider(this Worker worker) =>
            worker.World.GetFullInputBackgroundProvider();
        public static SpriteProvider GetFullInputBackgroundProvider(this World world) =>
            world.GetOrCreateSpriteProvider(SpriteAssetKey.FullInputBackground,
                world.GetBackgroundTexture(),
                LogixVisualCustomizer.FullBackgroundRect,
                LogixVisualCustomizer.FullBackgroundBorders,
                LogixVisualCustomizer.InputBackgroundScale);

        public static SpriteProvider GetFullInputBorderProvider(this Worker worker) =>
            worker.World.GetFullInputBorderProvider();
        public static SpriteProvider GetFullInputBorderProvider(this World world) =>
            world.GetOrCreateSpriteProvider(SpriteAssetKey.FullInputBorder,
                world.GetBorderTexture(),
                LogixVisualCustomizer.FullBorderRect,
                LogixVisualCustomizer.FullBorderBorders,
                LogixVisualCustomizer.InputBorderScale);

        public static IAssetProvider<FontSet> GetFont(this Worker worker) => worker.World.GetFont();

        public static IAssetProvider<FontSet> GetFont(this World world) => world.GetOrCreateFont(FontAssetKey.Main,
            LogixVisualCustomizer.TextFontUriKey, LogixVisualCustomizer.TextFontGlyphEmSizeKey,
            LogixVisualCustomizer.TextFontSecondaryUriKey, LogixVisualCustomizer.TextFontTertiaryUriKey,
            LogixVisualCustomizer.TextFontQuaternaryUriKey);

        public static void GetHorizontalInputProviders(this Worker worker, int index, int inputs,
            out SpriteProvider inputBackground, out SpriteProvider inputBorder) =>
            worker.World.GetHorizontalInputProviders(index, inputs, out inputBackground, out inputBorder);
        public static void GetHorizontalInputProviders(this World world, int index, int total,
            out SpriteProvider inputBackground, out SpriteProvider inputBorder)
        {
            if (index == 0)
            {
                if (total == 1)
                {
                    inputBackground = world.GetFullInputBackgroundProvider();
                    inputBorder = world.GetFullInputBorderProvider();
                }
                else
                {
                    inputBackground = world.GetLeftInputBackgroundProvider();
                    inputBorder = world.GetLeftInputBorderProvider();
                }
            }
            else if (index == total - 1)
            {
                inputBackground = world.GetRightInputBackgroundProvider();
                inputBorder = world.GetRightInputBorderProvider();
            }
            else
            {
                inputBackground = world.GetHorizontalMiddleInputBackgroundProvider();
                inputBorder = world.GetHorizontalMiddleInputBorderProvider();
            }
        }
        public static SpriteProvider GetHorizontalMiddleInputBackgroundProvider(this World world) =>
            world.GetOrCreateSpriteProvider(SpriteAssetKey.HorizontalMiddleBackground,
                world.GetBackgroundTexture(),
                LogixVisualCustomizer.IndividualInputs
                    ? LogixVisualCustomizer.FullBackgroundRect
                    : LogixVisualCustomizer.HorizontalMiddleBackgroundRect,
                LogixVisualCustomizer.IndividualInputs
                    ? LogixVisualCustomizer.FullBackgroundBorders
                    : LogixVisualCustomizer.HorizontalMiddleBackgroundBorders,
                LogixVisualCustomizer.InputBackgroundScale);

        public static SpriteProvider GetHorizontalMiddleInputBackgroundProvider(this Worker worker) =>
            worker.World.GetHorizontalMiddleInputBackgroundProvider();
        public static SpriteProvider GetHorizontalMiddleInputBorderProvider(this Worker worker) =>
            worker.World.GetHorizontalMiddleInputBorderProvider();
        public static SpriteProvider GetHorizontalMiddleInputBorderProvider(this World world) =>
            world.GetOrCreateSpriteProvider(SpriteAssetKey.HorizontalMiddleBorder,
                world.GetBorderTexture(),
                LogixVisualCustomizer.IndividualInputs
                    ? LogixVisualCustomizer.FullBorderRect
                    : LogixVisualCustomizer.HorizontalMiddleBorderRect,
                LogixVisualCustomizer.IndividualInputs
                    ? LogixVisualCustomizer.FullBorderBorders
                    : LogixVisualCustomizer.HorizontalMiddleBorderBorders,
                LogixVisualCustomizer.InputBorderScale);

        public static SpriteProvider GetLeftInputBackgroundProvider(this Worker worker) =>
            worker.World.GetLeftInputBackgroundProvider();
        public static SpriteProvider GetLeftInputBackgroundProvider(this World world) =>
            world.GetOrCreateSpriteProvider(SpriteAssetKey.LeftInputBackground,
                world.GetBackgroundTexture(),
                LogixVisualCustomizer.IndividualInputs
                    ? LogixVisualCustomizer.FullBackgroundRect
                    : LogixVisualCustomizer.LeftBackgroundRect,
                LogixVisualCustomizer.IndividualInputs
                    ? LogixVisualCustomizer.FullBackgroundBorders
                    : LogixVisualCustomizer.LeftBackgroundBorders,
                LogixVisualCustomizer.InputBackgroundScale);

        public static SpriteProvider GetLeftInputBorderProvider(this Worker worker) =>
            worker.World.GetLeftInputBorderProvider();
        public static SpriteProvider GetLeftInputBorderProvider(this World world) =>
            world.GetOrCreateSpriteProvider(SpriteAssetKey.LeftInputBorder,
                world.GetBorderTexture(),
                LogixVisualCustomizer.IndividualInputs
                    ? LogixVisualCustomizer.FullBorderRect
                    : LogixVisualCustomizer.LeftBorderRect,
                LogixVisualCustomizer.IndividualInputs
                    ? LogixVisualCustomizer.FullBorderBorders
                    : LogixVisualCustomizer.LeftBorderBorders,
                LogixVisualCustomizer.InputBorderScale);

        public static SpriteProvider GetNodeBackgroundProvider(this Worker worker) =>
            worker.World.GetNodeBackgroundProvider();
        public static SpriteProvider GetNodeBackgroundProvider(this World world) =>
            world.GetOrCreateSpriteProvider(SpriteAssetKey.NodeBackground,
                world.GetBackgroundTexture(),
                LogixVisualCustomizer.FullBackgroundRect,
                LogixVisualCustomizer.FullBackgroundBorders,
                LogixVisualCustomizer.NodeBackgroundScale);
        public static SpriteProvider GetNodeBorderProvider(this Worker worker) => worker.World.GetNodeBorderProvider();
        public static SpriteProvider GetNodeBorderProvider(this World world) =>
            world.GetOrCreateSpriteProvider(SpriteAssetKey.NodeBorder,
                world.GetBorderTexture(),
                LogixVisualCustomizer.FullBorderRect,
                LogixVisualCustomizer.FullBorderBorders,
                LogixVisualCustomizer.NodeBackgroundScale);

        public static SpriteProvider GetRightInputBackgroundProvider(this World world) =>
            world.GetOrCreateSpriteProvider(SpriteAssetKey.RightInputBackground,
                world.GetBackgroundTexture(),
                LogixVisualCustomizer.IndividualInputs
                    ? LogixVisualCustomizer.FullBackgroundRect
                    : LogixVisualCustomizer.RightBackgroundRect,
                LogixVisualCustomizer.IndividualInputs
                    ? LogixVisualCustomizer.FullBackgroundBorders
                    : LogixVisualCustomizer.RightBackgroundBorders,
                LogixVisualCustomizer.InputBackgroundScale);

        public static SpriteProvider GetRightInputBorderProvider(this World world) =>
            world.GetOrCreateSpriteProvider(SpriteAssetKey.RightInputBorder,
                world.GetBorderTexture(),
                LogixVisualCustomizer.IndividualInputs
                    ? LogixVisualCustomizer.FullBorderRect
                    : LogixVisualCustomizer.RightBorderRect,
                LogixVisualCustomizer.IndividualInputs
                    ? LogixVisualCustomizer.FullBorderBorders
                    : LogixVisualCustomizer.RightBorderBorders,
                LogixVisualCustomizer.InputBorderScale);

        public static SpriteProvider GetTopInputBackgroundProvider(this World world) =>
            world.GetOrCreateSpriteProvider(SpriteAssetKey.TopInputBackground,
                world.GetBackgroundTexture(),
                LogixVisualCustomizer.IndividualInputs
                    ? LogixVisualCustomizer.FullBackgroundRect
                    : LogixVisualCustomizer.TopBackgroundRect,
                LogixVisualCustomizer.IndividualInputs
                    ? LogixVisualCustomizer.FullBackgroundBorders
                    : LogixVisualCustomizer.TopBackgroundBorders,
                LogixVisualCustomizer.InputBackgroundScale);

        public static SpriteProvider GetTopInputBorderProvider(this World world) =>
            world.GetOrCreateSpriteProvider(SpriteAssetKey.TopInputBorder,
                world.GetBorderTexture(),
                LogixVisualCustomizer.IndividualInputs
                    ? LogixVisualCustomizer.FullBorderRect
                    : LogixVisualCustomizer.TopBorderRect,
                LogixVisualCustomizer.IndividualInputs
                    ? LogixVisualCustomizer.FullBorderBorders
                    : LogixVisualCustomizer.TopBorderBorders,
                LogixVisualCustomizer.InputBorderScale);

        public static void GetVerticalInputProviders(this Worker worker, int index, int inputs,
            out SpriteProvider inputBackground, out SpriteProvider inputBorder) =>
            worker.World.GetVerticalInputProviders(index, inputs, out inputBackground, out inputBorder);

        public static void GetVerticalInputProviders(this World world, int index, int total,
            out SpriteProvider inputBackground, out SpriteProvider inputBorder)
        {
            if (index == 0)
            {
                if (total == 1)
                {
                    inputBackground = world.GetLeftInputBackgroundProvider();
                    inputBorder = world.GetLeftInputBorderProvider();
                }
                else
                {
                    inputBackground = world.GetTopInputBackgroundProvider();
                    inputBorder = world.GetTopInputBorderProvider();
                }
            }
            else if (index == total - 1)
            {
                inputBackground = world.GetBottomInputBackgroundProvider();
                inputBorder = world.GetBottomInputBorderProvider();
            }
            else
            {
                inputBackground = world.GetVerticalMiddleInputBackgroundProvider();
                inputBorder = world.GetVerticalMiddleInputBorderProvider();
            }
        }
        public static SpriteProvider GetVerticalMiddleInputBackgroundProvider(this World world) =>
            world.GetOrCreateSpriteProvider(SpriteAssetKey.VerticalMiddleBackground,
                world.GetBackgroundTexture(),
                LogixVisualCustomizer.IndividualInputs
                    ? LogixVisualCustomizer.FullBackgroundRect
                    : LogixVisualCustomizer.VerticalMiddleBackgroundRect,
                LogixVisualCustomizer.IndividualInputs
                    ? LogixVisualCustomizer.FullBackgroundBorders
                    : LogixVisualCustomizer.VerticalMiddleBackgroundBorders,
                LogixVisualCustomizer.InputBackgroundScale);

        public static SpriteProvider GetVerticalMiddleInputBorderProvider(this World world) =>
            world.GetOrCreateSpriteProvider(SpriteAssetKey.VerticalMiddleBorder,
                world.GetBorderTexture(),
                LogixVisualCustomizer.IndividualInputs
                    ? LogixVisualCustomizer.FullBorderRect
                    : LogixVisualCustomizer.VerticalMiddleBorderRect,
                LogixVisualCustomizer.IndividualInputs
                    ? LogixVisualCustomizer.FullBorderBorders
                    : LogixVisualCustomizer.VerticalMiddleBorderBorders,
                LogixVisualCustomizer.InputBorderScale);

        private static void EnsureSettings(this SpriteProvider sprite, IAssetProvider<ITexture2D> texture,
            Rect localRect, float4 localBorders, float localScale)
        {
            sprite.Texture.Target = texture;
            sprite.Rect.Value = localRect;
            sprite.Borders.Value = localBorders;
            sprite.Scale.Value = localScale;
        }
        private static SpriteProvider GetOrCreateSpriteProvider(this World world, SpriteAssetKey key,
            IAssetProvider<ITexture2D> texture, Rect localRect, float4 localBorders, float localScale)
        {
            world.EnsureAssets();
            var index = (int) key;
            var sprite = LastSprites[index];
            if (sprite != null)
            {
                sprite.EnsureSettings(texture, localRect, localBorders, localScale);
                return sprite;
            }

            sprite = world.GetCustomizerAssets().AttachComponent<SpriteProvider>();
            sprite.EnsureSettings(texture, localRect, localBorders, localScale);
            LastSprites.GetElement(index).Target = sprite;

            return sprite;
        }

        private static IAssetProvider<FontSet> GetOrCreateFont(this World world, FontAssetKey key,
            ModConfigurationKey<Uri> uriKey, ModConfigurationKey<int> glyphConfigKey,
            ModConfigurationKey<Uri> uriKey2 = null, ModConfigurationKey<Uri> uriKey3 = null, 
            ModConfigurationKey<Uri> uriKey4 = null)
        {
            world.EnsureAssets();
            var index = (int) key;
            var font = LastFonts[index];
            if (font != null)
            {
                font.EnsureSettings(uriKey, glyphConfigKey, uriKey2, uriKey3, uriKey4);
                return font;
            }

            var assets = world.GetCustomizerAssets();
            font = assets.AttachComponent<FontChain>();
            
            var main = assets.AttachComponent<StaticFont>();
            font.MainFont.Target = main;
            for (var i = 0; i < 3; i++)
            {
                var fontpart = assets.AttachComponent<StaticFont>();
                font.FallbackFonts.Add(fontpart);
            }
            font.EnsureSettings(uriKey, glyphConfigKey, uriKey2, uriKey3, uriKey4);
            LastFonts.GetElement(index).Target = font;
            
            return font;
        }
        private static void EnsureSettings(this StaticTexture2D texture, ModConfigurationKey<Uri> uriConfigurationKey,
            ModConfigurationKey<TextureFilterMode> filterConfigurationKey)
        {
            texture.URL.DriveFromSharedSetting(uriConfigurationKey, LogixVisualCustomizer.Config);
            texture.FilterMode.DriveFromSharedSetting(filterConfigurationKey, LogixVisualCustomizer.Config);
            texture.WrapModeU.Value = TextureWrapMode.Clamp;
            texture.WrapModeV.Value = TextureWrapMode.Clamp;
        }

        private static void EnsureSettings(this FontChain font, ModConfigurationKey<Uri> uriKey,
            ModConfigurationKey<int> glyphConfigKey, ModConfigurationKey<Uri> uriKey2 = null,
            ModConfigurationKey<Uri> uriKey3 = null, ModConfigurationKey<Uri> uriKey4 = null)
        {
            var main = (StaticFont)font.MainFont;
            var keys = new[]
            {
                uriKey2, uriKey3, uriKey4
            };
            main.URL.DriveFromSharedSetting(uriKey, LogixVisualCustomizer.Config);
            main.GlyphEmSize.DriveFromSharedSetting(glyphConfigKey, LogixVisualCustomizer.Config);
            for (var i = 0; i < font.FallbackFonts.Count; i++)
            {
                var f = (StaticFont)font.FallbackFonts[i];
                f.URL.DriveFromSharedSetting(keys[i], LogixVisualCustomizer.Config);
                f.GlyphEmSize.DriveFromSharedSetting(glyphConfigKey, LogixVisualCustomizer.Config);
            }
        }
        private static StaticTexture2D GetOrCreateTexture(this World world, TextureAssetKey key,
            ModConfigurationKey<Uri> uriConfigurationKey, ModConfigurationKey<TextureFilterMode> filterConfigurationKey)
        {
            world.EnsureAssets();
            var index = (int) key;
            var texture = (StaticTexture2D)LastTextures[index];
            if (texture != null)
            {
                texture.EnsureSettings(uriConfigurationKey, filterConfigurationKey);
                return texture;
            }
            texture = world.GetCustomizerAssets().AttachComponent<StaticTexture2D>();
            texture.EnsureSettings(uriConfigurationKey, filterConfigurationKey);
            LastTextures.GetElement(index).Target = texture;
            return texture;
        }

        private static void EnsureAssets(this World world)
        {
            if (LastWorld != null && world.SessionId == LastWorld.SessionId && LastSlot != null) return;
            LastWorld = world;
            var assets = LastWorld.GetCustomizerAssets();
            assets.UpdateLast();
            if (LastIdentifier != default && LastIdentifier.Value.Value == CurrentIdentifier) return;
            assets.Destroy(); //older version needs to be removed to allow new version
            //i prefer nodes created with older versions losing their assets over newer nodes getting nothing and
            //causing errors in logs
            assets = world.CreateAssetSlot(GetUserHash(world.LocalUser.UserID));
            assets.UpdateLast();
        }
        private static void UpdateLast(this Slot slot)
        {
            LastSlot = slot;
            LastIdentifier = slot.GetComponent<ValueField<int>>();
            LastTextures = slot.GetComponent<ReferenceMultiplexer<IAssetProvider<ITexture2D>>>()?.References;
            LastSprites = slot.GetComponent<ReferenceMultiplexer<SpriteProvider>>()?.References;
            LastFonts = slot.GetComponent<ReferenceMultiplexer<FontChain>>()?.References;
        }
    }
}