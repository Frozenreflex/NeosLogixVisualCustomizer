using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;
using NeosModLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogixVisualCustomizer
{
    internal static class Assets
    {
        public static IAssetProvider<ITexture2D> GetBackgroundTexture(this World world)
        {
            const string key = "CustomBackground_Texture";

            return world.GetOrCreateTexture(key, LogixVisualCustomizer.BackgroundSpriteUriKey, LogixVisualCustomizer.BackgroundSpriteFilterKey);
        }
        public static IAssetProvider<ITexture2D> GetBorderTexture(this World world)
        {
            const string key = "CustomBorder_Texture";

            return world.GetOrCreateTexture(key, LogixVisualCustomizer.BorderSpriteUriKey, LogixVisualCustomizer.BorderSpriteFilterKey);
        }
        public static SpriteProvider GetBottomInputBackgroundProvider(this World world)
        {
            const string key = "BottomInputBackground_SpriteProvider";

            return world.GetOrCreateSpriteProvider(key,
                world.GetBackgroundTexture(),
                LogixVisualCustomizer.IndividualInputs
                    ? LogixVisualCustomizer.FullBackgroundRect
                    : LogixVisualCustomizer.BottomBackgroundRect,
                LogixVisualCustomizer.IndividualInputs
                    ? LogixVisualCustomizer.FullBackgroundBorders
                    : LogixVisualCustomizer.BottomBackgroundBorders,
                LogixVisualCustomizer.InputBackgroundScale);
        }
        public static SpriteProvider GetBottomInputBorderProvider(this World world)
        {
            const string key = "BottomInputBorder_SpriteProvider";

            return world.GetOrCreateSpriteProvider(key,
                world.GetBorderTexture(),
                LogixVisualCustomizer.IndividualInputs
                    ? LogixVisualCustomizer.FullBorderRect
                    : LogixVisualCustomizer.BottomBorderRect,
                LogixVisualCustomizer.IndividualInputs
                    ? LogixVisualCustomizer.FullBorderBorders
                    : LogixVisualCustomizer.BottomBorderBorders,
                LogixVisualCustomizer.InputBorderScale);
        }
        public static Slot GetCustomizerAssets(this World world)
        {
            var key = $"LogixCustomizerAssets_{LogixVisualCustomizer.UserRandom}";

            if (world.AssetsSlot.Find(key) is Slot slot)
                return slot;

            slot = world.AssetsSlot.AddSlot(key);
            slot.AttachComponent<AssetOptimizationBlock>();

            return slot;
        }

        public static SpriteProvider GetFullInputBackgroundProvider(this Worker worker) =>
            worker.World.GetFullInputBackgroundProvider();
        public static SpriteProvider GetFullInputBackgroundProvider(this World world)
        {
            const string key = "FullInputBackground_SpriteProvider";

            return world.GetOrCreateSpriteProvider(key,
                world.GetBackgroundTexture(),
                LogixVisualCustomizer.FullBackgroundRect,
                LogixVisualCustomizer.FullBackgroundBorders,
                LogixVisualCustomizer.InputBackgroundScale);
        }

        public static SpriteProvider GetFullInputBorderProvider(this Worker worker) =>
            worker.World.GetFullInputBorderProvider();
        public static SpriteProvider GetFullInputBorderProvider(this World world)
        {
            const string key = "FullInputBorder_SpriteProvider";

            return world.GetOrCreateSpriteProvider(key,
                world.GetBorderTexture(),
                LogixVisualCustomizer.FullBorderRect,
                LogixVisualCustomizer.FullBorderBorders,
                LogixVisualCustomizer.InputBorderScale);
        }
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
        public static SpriteProvider GetHorizontalMiddleInputBackgroundProvider(this World world)
        {
            const string key = "HorizontalMiddleInputBackground_SpriteProvider";

            return world.GetOrCreateSpriteProvider(key,
                world.GetBackgroundTexture(),
                LogixVisualCustomizer.IndividualInputs
                    ? LogixVisualCustomizer.FullBackgroundRect
                    : LogixVisualCustomizer.HorizontalMiddleBackgroundRect,
                LogixVisualCustomizer.IndividualInputs
                    ? LogixVisualCustomizer.FullBackgroundBorders
                    : LogixVisualCustomizer.HorizontalMiddleBackgroundBorders,
                LogixVisualCustomizer.InputBackgroundScale);
        }
        public static SpriteProvider GetHorizontalMiddleInputBackgroundProvider(this Worker worker) =>
            worker.World.GetHorizontalMiddleInputBackgroundProvider();
        public static SpriteProvider GetHorizontalMiddleInputBorderProvider(this Worker worker) =>
            worker.World.GetHorizontalMiddleInputBorderProvider();
        public static SpriteProvider GetHorizontalMiddleInputBorderProvider(this World world)
        {
            const string key = "HorizontalMiddleInputBorder_SpriteProvider";

            return world.GetOrCreateSpriteProvider(key,
                world.GetBorderTexture(),
                LogixVisualCustomizer.IndividualInputs
                    ? LogixVisualCustomizer.FullBorderRect
                    : LogixVisualCustomizer.HorizontalMiddleBorderRect,
                LogixVisualCustomizer.IndividualInputs
                    ? LogixVisualCustomizer.FullBorderBorders
                    : LogixVisualCustomizer.HorizontalMiddleBorderBorders,
                LogixVisualCustomizer.InputBorderScale);
        }
        public static SpriteProvider GetLeftInputBackgroundProvider(this Worker worker) =>
            worker.World.GetLeftInputBackgroundProvider();
        public static SpriteProvider GetLeftInputBackgroundProvider(this World world)
        {
            const string key = "LeftInputBackground_SpriteProvider";

            return world.GetOrCreateSpriteProvider(key,
                world.GetBackgroundTexture(),
                LogixVisualCustomizer.IndividualInputs
                    ? LogixVisualCustomizer.FullBackgroundRect
                    : LogixVisualCustomizer.LeftBackgroundRect,
                LogixVisualCustomizer.IndividualInputs
                    ? LogixVisualCustomizer.FullBackgroundBorders
                    : LogixVisualCustomizer.LeftBackgroundBorders,
                LogixVisualCustomizer.InputBackgroundScale);
        }
        public static SpriteProvider GetLeftInputBorderProvider(this Worker worker) =>
            worker.World.GetLeftInputBorderProvider();
        public static SpriteProvider GetLeftInputBorderProvider(this World world)
        {
            const string key = "LeftInputBorder_SpriteProvider";

            return world.GetOrCreateSpriteProvider(key,
                world.GetBorderTexture(),
                LogixVisualCustomizer.IndividualInputs
                    ? LogixVisualCustomizer.FullBorderRect
                    : LogixVisualCustomizer.LeftBorderRect,
                LogixVisualCustomizer.IndividualInputs
                    ? LogixVisualCustomizer.FullBorderBorders
                    : LogixVisualCustomizer.LeftBorderBorders,
                LogixVisualCustomizer.InputBorderScale);
        }
        public static SpriteProvider GetNodeBackgroundProvider(this Worker worker) =>
            worker.World.GetNodeBackgroundProvider();
        public static SpriteProvider GetNodeBackgroundProvider(this World world)
        {
            const string key = "NodeBackground_SpriteProvider";

            return world.GetOrCreateSpriteProvider(key,
                world.GetBackgroundTexture(),
                LogixVisualCustomizer.FullBackgroundRect,
                LogixVisualCustomizer.FullBackgroundBorders,
                LogixVisualCustomizer.NodeBackgroundScale);
        }
        public static SpriteProvider GetNodeBorderProvider(this Worker worker) => worker.World.GetNodeBorderProvider();
        public static SpriteProvider GetNodeBorderProvider(this World world)
        {
            const string key = "NodeBorder_SpriteProvider";

            return world.GetOrCreateSpriteProvider(key,
                world.GetBorderTexture(),
                LogixVisualCustomizer.FullBorderRect,
                LogixVisualCustomizer.FullBorderBorders,
                LogixVisualCustomizer.NodeBackgroundScale);
        }
        public static SpriteProvider GetRightInputBackgroundProvider(this World world)
        {
            const string key = "RightInputBackground_SpriteProvider";

            return world.GetOrCreateSpriteProvider(key,
                world.GetBackgroundTexture(),
                LogixVisualCustomizer.IndividualInputs
                    ? LogixVisualCustomizer.FullBackgroundRect
                    : LogixVisualCustomizer.RightBackgroundRect,
                LogixVisualCustomizer.IndividualInputs
                    ? LogixVisualCustomizer.FullBackgroundBorders
                    : LogixVisualCustomizer.RightBackgroundBorders,
                LogixVisualCustomizer.InputBackgroundScale);
        }
        public static SpriteProvider GetRightInputBorderProvider(this World world)
        {
            const string key = "RightInputBorder_SpriteProvider";

            return world.GetOrCreateSpriteProvider(key,
                world.GetBorderTexture(),
                LogixVisualCustomizer.IndividualInputs
                    ? LogixVisualCustomizer.FullBorderRect
                    : LogixVisualCustomizer.RightBorderRect,
                LogixVisualCustomizer.IndividualInputs
                    ? LogixVisualCustomizer.FullBorderBorders
                    : LogixVisualCustomizer.RightBorderBorders,
                LogixVisualCustomizer.InputBorderScale);
        }
        public static SpriteProvider GetTopInputBackgroundProvider(this World world)
        {
            const string key = "TopInputBackground_SpriteProvider";

            return world.GetOrCreateSpriteProvider(key,
                world.GetBackgroundTexture(),
                LogixVisualCustomizer.IndividualInputs
                    ? LogixVisualCustomizer.FullBackgroundRect
                    : LogixVisualCustomizer.TopBackgroundRect,
                LogixVisualCustomizer.IndividualInputs
                    ? LogixVisualCustomizer.FullBackgroundBorders
                    : LogixVisualCustomizer.TopBackgroundBorders,
                LogixVisualCustomizer.InputBackgroundScale);
        }
        public static SpriteProvider GetTopInputBorderProvider(this World world)
        {
            const string key = "TopInputBorder_SpriteProvider";

            return world.GetOrCreateSpriteProvider(key,
                world.GetBorderTexture(),
                LogixVisualCustomizer.IndividualInputs
                    ? LogixVisualCustomizer.FullBorderRect
                    : LogixVisualCustomizer.TopBorderRect,
                LogixVisualCustomizer.IndividualInputs
                    ? LogixVisualCustomizer.FullBorderBorders
                    : LogixVisualCustomizer.TopBorderBorders,
                LogixVisualCustomizer.InputBorderScale);
        }

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
        public static SpriteProvider GetVerticalMiddleInputBackgroundProvider(this World world)
        {
            const string key = "VerticalMiddleInputBackground_SpriteProvider";

            return world.GetOrCreateSpriteProvider(key,
                world.GetBackgroundTexture(),
                LogixVisualCustomizer.IndividualInputs
                    ? LogixVisualCustomizer.FullBackgroundRect
                    : LogixVisualCustomizer.VerticalMiddleBackgroundRect,
                LogixVisualCustomizer.IndividualInputs
                    ? LogixVisualCustomizer.FullBackgroundBorders
                    : LogixVisualCustomizer.VerticalMiddleBackgroundBorders,
                LogixVisualCustomizer.InputBackgroundScale);
        }
        public static SpriteProvider GetVerticalMiddleInputBorderProvider(this World world)
        {
            const string key = "VerticalMiddleInputBorder_SpriteProvider";

            return world.GetOrCreateSpriteProvider(key,
                world.GetBorderTexture(),
                LogixVisualCustomizer.IndividualInputs
                    ? LogixVisualCustomizer.FullBorderRect
                    : LogixVisualCustomizer.VerticalMiddleBorderRect,
                LogixVisualCustomizer.IndividualInputs
                    ? LogixVisualCustomizer.FullBorderBorders
                    : LogixVisualCustomizer.VerticalMiddleBorderBorders,
                LogixVisualCustomizer.InputBorderScale);
        }

        private static void EnsureSettings(this SpriteProvider sprite, IAssetProvider<ITexture2D> texture, Rect localRect, float4 localBorders, float localScale)
        {
            (sprite.Texture.ActiveLink as SyncElement)?.Component.Destroy();

            sprite.Texture.Target = texture;
            sprite.Rect.Value = localRect;
            sprite.Borders.Value = localBorders;
            sprite.Scale.Value = localScale;
        }

        private static SpriteProvider GetOrCreateSpriteProvider(this World world, string key, IAssetProvider<ITexture2D> texture, Rect localRect, float4 localBorders, float localScale)
        {
            key = $"{LogixVisualCustomizer.UserRandom}_{key}";
            if (world.KeyOwner(key) is SpriteProvider sprite) return sprite;

            sprite = world.GetCustomizerAssets().AttachComponent<SpriteProvider>();
            sprite.EnsureSettings(texture, localRect, localBorders, localScale);

            sprite.AssignKey(key);

            return sprite;
        }

        private static StaticTexture2D GetOrCreateTexture(this World world, string key, ModConfigurationKey<Uri> uriConfigurationKey, ModConfigurationKey<TextureFilterMode> filterConfigurationKey)
        {
            key = $"LogixCustomizer_{LogixVisualCustomizer.UserRandom}_{key}";
            if (world.KeyOwner(key) is StaticTexture2D texture) return texture;

            texture = world.GetCustomizerAssets().AttachComponent<StaticTexture2D>();
            texture.URL.DriveFromSharedSetting(uriConfigurationKey, LogixVisualCustomizer.Config);
            texture.FilterMode.DriveFromSharedSetting(filterConfigurationKey, LogixVisualCustomizer.Config);
            texture.WrapModeU.Value = TextureWrapMode.Clamp;
            texture.WrapModeV.Value = TextureWrapMode.Clamp;
            texture.AssignKey(key);

            return texture;
        }
    }
}