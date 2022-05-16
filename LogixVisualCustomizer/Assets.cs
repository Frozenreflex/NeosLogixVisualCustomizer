﻿using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogixVisualCustomizer
{
    internal static class Assets
    {
        private const TextureFilterMode DefaultFilterMode = TextureFilterMode.Point;
        private static readonly Uri DefaultUri = new Uri("neosdb:///c58b6a6570f385c8ca45895b3ecd530a14d2fd77a5c8f54b579ff79f9204419a.webp");

        public static IAssetProvider<ITexture2D> GetBackgroundTexture(this World world)
        {
            const string key = "LogixCustomizer_CustomBackground_Texture";

            return world.GetOrCreateTexture(key, !LogixVisualCustomizer.UseBackground, LogixVisualCustomizer.BackgroundSpriteUri);
        }

        public static IAssetProvider<ITexture2D> GetBackgroundTexture(this Worker worker)
        {
            return worker.World.GetBackgroundTexture();
        }

        public static IAssetProvider<ITexture2D> GetBorderTexture(this Worker worker)
        {
            return worker.World.GetBorderTexture();
        }

        public static IAssetProvider<ITexture2D> GetBorderTexture(this World world)
        {
            const string key = "LogixCustomizer_CustomBorder_Texture";

            return world.GetOrCreateTexture(key, !LogixVisualCustomizer.UseBorder, LogixVisualCustomizer.BorderSpriteUri);
        }

        public static SpriteProvider GetBottomInputBackgroundProvider(this Worker worker)
        {
            return worker.World.GetBottomInputBackgroundProvider();
        }

        public static SpriteProvider GetBottomInputBackgroundProvider(this World world)
        {
            const string key = "LogixCustomizer_BottomInputBackground_SpriteProvider";

            return world.GetOrCreateSpriteProvider(key,
                world.GetBackgroundTexture(),
                LogixVisualCustomizer.IndividualInputs ? LogixVisualCustomizer.FullBackgroundRect : LogixVisualCustomizer.BottomBackgroundRect,
                LogixVisualCustomizer.IndividualInputs ? LogixVisualCustomizer.FullBackgroundBorders : LogixVisualCustomizer.BottomBackgroundBorders,
                LogixVisualCustomizer.InputBackgroundScale);
        }

        public static SpriteProvider GetBottomInputBorderProvider(this Worker worker)
        {
            return worker.World.GetBottomInputBorderProvider();
        }

        public static SpriteProvider GetBottomInputBorderProvider(this World world)
        {
            const string key = "LogixCustomizer_BottomInputBorder_SpriteProvider";

            return world.GetOrCreateSpriteProvider(key,
                world.GetBorderTexture(),
                LogixVisualCustomizer.IndividualInputs ? LogixVisualCustomizer.FullBorderRect : LogixVisualCustomizer.BottomBorderRect,
                LogixVisualCustomizer.IndividualInputs ? LogixVisualCustomizer.FullBorderBorders : LogixVisualCustomizer.BottomBorderBorders,
                LogixVisualCustomizer.InputBorderScale);
        }

        public static Slot GetCustomizerAssets(this Worker worker)
        {
            return worker.World.GetCustomizerAssets();
        }

        public static Slot GetCustomizerAssets(this World world)
        {
            const string key = "LogixCustomizerAssets";

            if (world.AssetsSlot.Find(key) is Slot slot)
                return slot;

            slot = world.AssetsSlot.AddSlot(key);
            slot.AttachComponent<AssetOptimizationBlock>();

            return slot;
        }

        public static SpriteProvider GetFullInputBackgroundProvider(this Worker worker)
        {
            return worker.World.GetFullInputBackgroundProvider();
        }

        public static SpriteProvider GetFullInputBackgroundProvider(this World world)
        {
            const string key = "LogixCustomizer_FullInputBackground_SpriteProvider";

            return world.GetOrCreateSpriteProvider(key,
                world.GetBackgroundTexture(),
                LogixVisualCustomizer.FullBackgroundRect,
                LogixVisualCustomizer.FullBackgroundBorders,
                LogixVisualCustomizer.InputBackgroundScale);
        }

        public static SpriteProvider GetFullInputBorderProvider(this Worker worker)
        {
            return worker.World.GetFullInputBorderProvider();
        }

        public static SpriteProvider GetFullInputBorderProvider(this World world)
        {
            const string key = "LogixCustomizer_FullInputBorder_SpriteProvider";

            return world.GetOrCreateSpriteProvider(key,
                world.GetBorderTexture(),
                LogixVisualCustomizer.FullBorderRect,
                LogixVisualCustomizer.FullBorderBorders,
                LogixVisualCustomizer.InputBorderScale);
        }

        public static void GetHorizontalInputProviders(this Worker worker, int index, int inputs, out SpriteProvider inputBackground, out SpriteProvider inputBorder)
        {
            worker.World.GetHorizontalInputProviders(index, inputs, out inputBackground, out inputBorder);
        }

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
            const string key = "LogixCustomizer_HorizontalMiddleInputBackground_SpriteProvider";

            return world.GetOrCreateSpriteProvider(key,
                world.GetBackgroundTexture(),
                LogixVisualCustomizer.IndividualInputs ? LogixVisualCustomizer.FullBackgroundRect : LogixVisualCustomizer.HorizontalMiddleBackgroundRect,
                LogixVisualCustomizer.IndividualInputs ? LogixVisualCustomizer.FullBackgroundBorders : LogixVisualCustomizer.HorizontalMiddleBackgroundBorders,
                LogixVisualCustomizer.InputBackgroundScale);
        }

        public static SpriteProvider GetHorizontalMiddleInputBackgroundProvider(this Worker worker)
        {
            return worker.World.GetHorizontalMiddleInputBackgroundProvider();
        }

        public static SpriteProvider GetHorizontalMiddleInputBorderProvider(this Worker worker)
        {
            return worker.World.GetHorizontalMiddleInputBorderProvider();
        }

        public static SpriteProvider GetHorizontalMiddleInputBorderProvider(this World world)
        {
            const string key = "LogixCustomizer_HorizontalMiddleInputBorder_SpriteProvider";

            return world.GetOrCreateSpriteProvider(key,
                world.GetBorderTexture(),
                LogixVisualCustomizer.IndividualInputs ? LogixVisualCustomizer.FullBorderRect : LogixVisualCustomizer.HorizontalMiddleBorderRect,
                LogixVisualCustomizer.IndividualInputs ? LogixVisualCustomizer.FullBorderBorders : LogixVisualCustomizer.HorizontalMiddleBorderBorders,
                LogixVisualCustomizer.InputBorderScale);
        }

        public static SpriteProvider GetLeftInputBackgroundProvider(this Worker worker)
        {
            return worker.World.GetLeftInputBackgroundProvider();
        }

        public static SpriteProvider GetLeftInputBackgroundProvider(this World world)
        {
            const string key = "LogixCustomizer_LeftInputBackground_SpriteProvider";

            return world.GetOrCreateSpriteProvider(key,
                world.GetBackgroundTexture(),
                LogixVisualCustomizer.IndividualInputs ? LogixVisualCustomizer.FullBackgroundRect : LogixVisualCustomizer.LeftBackgroundRect,
                LogixVisualCustomizer.IndividualInputs ? LogixVisualCustomizer.FullBackgroundBorders : LogixVisualCustomizer.LeftBackgroundBorders,
                LogixVisualCustomizer.InputBackgroundScale);
        }

        public static SpriteProvider GetLeftInputBorderProvider(this Worker worker)
        {
            return worker.World.GetLeftInputBorderProvider();
        }

        public static SpriteProvider GetLeftInputBorderProvider(this World world)
        {
            const string key = "LogixCustomizer_LeftInputBorder_SpriteProvider";

            return world.GetOrCreateSpriteProvider(key,
                world.GetBorderTexture(),
                LogixVisualCustomizer.IndividualInputs ? LogixVisualCustomizer.FullBorderRect : LogixVisualCustomizer.LeftBorderRect,
                LogixVisualCustomizer.IndividualInputs ? LogixVisualCustomizer.FullBorderBorders : LogixVisualCustomizer.LeftBorderBorders,
                LogixVisualCustomizer.InputBorderScale);
        }

        public static SpriteProvider GetNodeBackgroundProvider(this Worker worker)
        {
            return worker.World.GetNodeBackgroundProvider();
        }

        public static SpriteProvider GetNodeBackgroundProvider(this World world)
        {
            const string key = "LogixCustomizer_NodeBackground_SpriteProvider";

            return world.GetOrCreateSpriteProvider(key,
                world.GetBackgroundTexture(),
                LogixVisualCustomizer.IndividualInputs ? LogixVisualCustomizer.FullBackgroundRect : LogixVisualCustomizer.FullBackgroundRect,
                LogixVisualCustomizer.FullBackgroundBorders,
                LogixVisualCustomizer.NodeBackgroundScale);
        }

        public static SpriteProvider GetNodeBorderProvider(this Worker worker)
        {
            return worker.World.GetNodeBorderProvider();
        }

        public static SpriteProvider GetNodeBorderProvider(this World world)
        {
            const string key = "LogixCustomizer_NodeBorder_SpriteProvider";

            return world.GetOrCreateSpriteProvider(key,
                world.GetBorderTexture(),
                LogixVisualCustomizer.IndividualInputs ? LogixVisualCustomizer.FullBorderRect : LogixVisualCustomizer.FullBorderRect,
                LogixVisualCustomizer.FullBorderBorders,
                LogixVisualCustomizer.NodeBackgroundScale);
        }

        public static SpriteProvider GetRightInputBackgroundProvider(this Worker worker)
        {
            return worker.World.GetRightInputBackgroundProvider();
        }

        public static SpriteProvider GetRightInputBackgroundProvider(this World world)
        {
            const string key = "LogixCustomizer_RightInputBackground_SpriteProvider";

            return world.GetOrCreateSpriteProvider(key,
                world.GetBackgroundTexture(),
                LogixVisualCustomizer.IndividualInputs ? LogixVisualCustomizer.FullBackgroundRect : LogixVisualCustomizer.RightBackgroundRect,
                LogixVisualCustomizer.IndividualInputs ? LogixVisualCustomizer.FullBackgroundBorders : LogixVisualCustomizer.RightBackgroundBorders,
                LogixVisualCustomizer.InputBackgroundScale);
        }

        public static SpriteProvider GetRightInputBorderProvider(this Worker worker)
        {
            return worker.World.GetRightInputBorderProvider();
        }

        public static SpriteProvider GetRightInputBorderProvider(this World world)
        {
            const string key = "LogixCustomizer_RightInputBorder_SpriteProvider";

            return world.GetOrCreateSpriteProvider(key,
                world.GetBorderTexture(),
                LogixVisualCustomizer.IndividualInputs ? LogixVisualCustomizer.FullBorderRect : LogixVisualCustomizer.RightBorderRect,
                LogixVisualCustomizer.IndividualInputs ? LogixVisualCustomizer.FullBorderBorders : LogixVisualCustomizer.RightBorderBorders,
                LogixVisualCustomizer.InputBorderScale);
        }

        public static SpriteProvider GetTopInputBackgroundProvider(this Worker worker)
        {
            return worker.World.GetTopInputBackgroundProvider();
        }

        public static SpriteProvider GetTopInputBackgroundProvider(this World world)
        {
            const string key = "LogixCustomizer_TopInputBackground_SpriteProvider";

            return world.GetOrCreateSpriteProvider(key,
                world.GetBackgroundTexture(),
                LogixVisualCustomizer.IndividualInputs ? LogixVisualCustomizer.FullBackgroundRect : LogixVisualCustomizer.TopBackgroundRect,
                LogixVisualCustomizer.IndividualInputs ? LogixVisualCustomizer.FullBackgroundBorders : LogixVisualCustomizer.TopBackgroundBorders,
                LogixVisualCustomizer.InputBackgroundScale);
        }

        public static SpriteProvider GetTopInputBorderProvider(this Worker worker)
        {
            return worker.World.GetTopInputBorderProvider();
        }

        public static SpriteProvider GetTopInputBorderProvider(this World world)
        {
            const string key = "LogixCustomizer_TopInputBorder_SpriteProvider";

            return world.GetOrCreateSpriteProvider(key,
                world.GetBorderTexture(),
                LogixVisualCustomizer.IndividualInputs ? LogixVisualCustomizer.FullBorderRect : LogixVisualCustomizer.TopBorderRect,
                LogixVisualCustomizer.IndividualInputs ? LogixVisualCustomizer.FullBorderBorders : LogixVisualCustomizer.TopBorderBorders,
                LogixVisualCustomizer.InputBorderScale);
        }

        public static void GetVerticalInputProviders(this Worker worker, int index, int inputs, out SpriteProvider inputBackground, out SpriteProvider inputBorder)
        {
            worker.World.GetVerticalInputProviders(index, inputs, out inputBackground, out inputBorder);
        }

        public static void GetVerticalInputProviders(this World world, int index, int total, out SpriteProvider inputBackground, out SpriteProvider inputBorder)
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

        public static SpriteProvider GetVerticalMiddleInputBackgroundProvider(this Worker worker)
        {
            return worker.World.GetVerticalMiddleInputBackgroundProvider();
        }

        public static SpriteProvider GetVerticalMiddleInputBackgroundProvider(this World world)
        {
            const string key = "LogixCustomizer_VerticalMiddleInputBackground_SpriteProvider";

            return world.GetOrCreateSpriteProvider(key,
                world.GetBackgroundTexture(),
                LogixVisualCustomizer.IndividualInputs ? LogixVisualCustomizer.FullBackgroundRect : LogixVisualCustomizer.VerticalMiddleBackgroundRect,
                LogixVisualCustomizer.IndividualInputs ? LogixVisualCustomizer.FullBackgroundBorders : LogixVisualCustomizer.VerticalMiddleBackgroundBorders,
                LogixVisualCustomizer.InputBackgroundScale);
        }

        public static SpriteProvider GetVerticalMiddleInputBorderProvider(this Worker worker)
        {
            return worker.World.GetVerticalMiddleInputBorderProvider();
        }

        public static SpriteProvider GetVerticalMiddleInputBorderProvider(this World world)
        {
            const string key = "LogixCustomizer_VerticalMiddleInputBorder_SpriteProvider";

            return world.GetOrCreateSpriteProvider(key,
                world.GetBorderTexture(),
                LogixVisualCustomizer.IndividualInputs ? LogixVisualCustomizer.FullBorderRect : LogixVisualCustomizer.VerticalMiddleBorderRect,
                LogixVisualCustomizer.IndividualInputs ? LogixVisualCustomizer.FullBorderBorders : LogixVisualCustomizer.VerticalMiddleBorderBorders,
                LogixVisualCustomizer.InputBorderScale);
        }

        private static void EnsureSettings(this SpriteProvider sprite, IAssetProvider<ITexture2D> texture, Rect localRect, float4 localBorders, float localScale)
        {
            (sprite.Texture.ActiveLink as SyncElement)?.Component.Destroy();

            sprite.Texture.Target = texture;

            var rectOverride = sprite.Rect.GetUserOverride(true);
            rectOverride.CreateOverrideOnWrite.Value = true;
            rectOverride.Default.Value = new Rect(0, 0, 1, 1);
            rectOverride.SetOverride(sprite.World.LocalUser, localRect);

            var bordersOverride = sprite.Borders.GetUserOverride(true);
            bordersOverride.CreateOverrideOnWrite.Value = true;
            bordersOverride.Default.Value = new float4(.5f, .5f, .5f, .5f);
            bordersOverride.SetOverride(sprite.World.LocalUser, localBorders);

            var scaleOverride = sprite.Scale.GetUserOverride(true);
            scaleOverride.CreateOverrideOnWrite.Value = true;
            scaleOverride.Default.Value = 1;
            scaleOverride.SetOverride(sprite.World.LocalUser, localScale);
        }

        private static void EnsureSettings(this StaticTexture2D texture, Uri source, bool useDefault)
        {
            texture.WrapModeU.Value = TextureWrapMode.Clamp;
            texture.WrapModeV.Value = TextureWrapMode.Clamp;
            texture.FilterMode.Value = useDefault ? DefaultFilterMode : TextureFilterMode.Anisotropic;

            var urlOverride = texture.URL.GetUserOverride(true);
            urlOverride.CreateOverrideOnWrite.Value = true;
            urlOverride.Default.Value = DefaultUri;
            urlOverride.SetOverride(texture.World.LocalUser, useDefault ? DefaultUri : source);
        }

        private static SpriteProvider GetOrCreateSpriteProvider(this World world, string key, IAssetProvider<ITexture2D> texture, Rect localRect, float4 localBorders, float localScale)
        {
            if (world.KeyOwner(key) is SpriteProvider sprite)
            {
                sprite.EnsureSettings(texture, localRect, localBorders, localScale);

                return sprite;
            }

            sprite = world.GetCustomizerAssets().AttachComponent<SpriteProvider>();
            sprite.EnsureSettings(texture, localRect, localBorders, localScale);

            sprite.DestroyWhenDestroyed(sprite.Rect.GetUserOverride());
            sprite.DestroyWhenDestroyed(sprite.Borders.GetUserOverride());
            sprite.DestroyWhenDestroyed(sprite.Scale.GetUserOverride());

            sprite.AssignKey(key);

            return sprite;
        }

        private static StaticTexture2D GetOrCreateTexture(this World world, string key, bool useDefault, Uri source)
        {
            if (world.KeyOwner(key) is StaticTexture2D texture)
            {
                texture.EnsureSettings(source, useDefault);

                return texture;
            }

            texture = world.GetCustomizerAssets().AttachComponent<StaticTexture2D>();
            texture.EnsureSettings(source, useDefault);

            texture.DestroyWhenDestroyed(texture.URL.GetUserOverride());

            texture.AssignKey(key);

            return texture;
        }
    }
}