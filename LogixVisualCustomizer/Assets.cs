using BaseX;
using FrooxEngine;
using NeosModLoader;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace LogixVisualCustomizer
{
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
        private static ReferenceMultiplexer<IAssetProvider<ITexture2D>> LastTextures;
        private static ReferenceMultiplexer<SpriteProvider> LastSprites;
        private static Dictionary<string, string> UserHash = new Dictionary<string, string>();
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
            if (!UserHash.TryGetValue(world.LocalUser.UserID, out var hash))
            {
                //this doesn't need to be completely cryptographically secure or anything like that,
                //but blatantly showing the userID does feel kinda wrong to me
                hash = BitConverter.ToString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(user)))
                    .Replace("-", string.Empty).Substring(0, 8);
                //cache the hash so we don't compute a new one every time
                UserHash.Add(user, hash);
            }
            var key = $"LogixCustomizerAssets_{hash}";
            
            if (world.AssetsSlot.Find(key) is Slot slot)
                return slot;

            slot = world.AssetsSlot.AddSlot(key);
            slot.AttachComponent<AssetOptimizationBlock>();
            slot.AttachComponent<ReferenceMultiplexer<IAssetProvider<ITexture2D>>>().References
                .AddRange(new IAssetProvider<ITexture2D>[Enum.GetNames(typeof(TextureAssetKey)).Length]);
            slot.AttachComponent<ReferenceMultiplexer<SpriteProvider>>().References
                .AddRange(new SpriteProvider[Enum.GetNames(typeof(SpriteAssetKey)).Length]);
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

        public static void GetHorizontalInputProviders(this Worker worker, int index, int inputs,
            out SpriteProvider inputBackground, out SpriteProvider inputBorder) =>
            worker.World.GetHorizontalInputProviders(index, inputs, out inputBackground, out inputBorder);
        public static void GetHorizontalInputProviders(this World world, int index, int total, out SpriteProvider inputBackground, out SpriteProvider inputBorder)
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

        private static void EnsureSettings(this SpriteProvider sprite, IAssetProvider<ITexture2D> texture, Rect localRect, float4 localBorders, float localScale)
        {
            //(sprite.Texture.ActiveLink as SyncElement)?.Component.Destroy();

            sprite.Texture.Target = texture;
            sprite.Rect.Value = localRect;
            sprite.Borders.Value = localBorders;
            sprite.Scale.Value = localScale;
        }
        private static SpriteProvider GetOrCreateSpriteProvider(this World world, SpriteAssetKey key, IAssetProvider<ITexture2D> texture, Rect localRect, float4 localBorders, float localScale)
        {
            world.EnsureWorld();
            var index = (int) key;
            var sprite = LastSprites.References[index];
            if (sprite != null)
            {
                sprite.EnsureSettings(texture, localRect, localBorders, localScale);
                return sprite;
            }

            sprite = world.GetCustomizerAssets().AttachComponent<SpriteProvider>();
            sprite.EnsureSettings(texture, localRect, localBorders, localScale);
            LastSprites.References.GetElement(index).Target = sprite;

            return sprite;
        }
        private static void EnsureSettings(this StaticTexture2D texture, ModConfigurationKey<Uri> uriConfigurationKey, ModConfigurationKey<TextureFilterMode> filterConfigurationKey)
        {
            texture.URL.DriveFromSharedSetting(uriConfigurationKey, LogixVisualCustomizer.Config);
            texture.FilterMode.DriveFromSharedSetting(filterConfigurationKey, LogixVisualCustomizer.Config);
            texture.WrapModeU.Value = TextureWrapMode.Clamp;
            texture.WrapModeV.Value = TextureWrapMode.Clamp;
        }
        private static StaticTexture2D GetOrCreateTexture(this World world, TextureAssetKey key, ModConfigurationKey<Uri> uriConfigurationKey, ModConfigurationKey<TextureFilterMode> filterConfigurationKey)
        {
            world.EnsureWorld();
            var index = (int) key;
            var texture = (StaticTexture2D)LastTextures.References[index];
            if (texture != null)
            {
                texture.EnsureSettings(uriConfigurationKey, filterConfigurationKey);
                return texture;
            }
            texture = world.GetCustomizerAssets().AttachComponent<StaticTexture2D>();
            texture.EnsureSettings(uriConfigurationKey, filterConfigurationKey);
            LastTextures.References.GetElement(index).Target = texture;
            return texture;
        }

        private static void EnsureWorld(this World world)
        {
            if (LastWorld != null && world.SessionId == LastWorld.SessionId) return;
            LastWorld = world;
            var assets = LastWorld.GetCustomizerAssets();
            LastTextures = assets.GetComponent<ReferenceMultiplexer<IAssetProvider<ITexture2D>>>();
            LastSprites = assets.GetComponent<ReferenceMultiplexer<SpriteProvider>>();
        }
    }
}