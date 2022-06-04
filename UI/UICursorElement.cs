using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace BossCursor.UI
{
    public class UiCursorElement : UIElement
    {
        private const string CursorTexturePath = "BossCursor/UI/Cursor";
        private const float HeadDistance = 20f;
        private readonly List<NPC> _bosses = new();
        private Config _config;
        private bool _drawCursors;

        public override void Update(GameTime gameTime)
        {
            if (!BossCursor.CursorEnabled || Main.mapStyle == 2)
            {
                _drawCursors = false;
                return;
            }
            _config = ModContent.GetInstance<Config>();
            UpdateBossesList();
            _drawCursors = _bosses.Count > 0;
        }
        
        private void UpdateBossesList()
        {
            _bosses.Clear();
            for (var i = 0; i < Main.maxNPCs; i++)
            {
                var npc = Main.npc[i];
                if (!npc.active) continue;
                if (BossCursor.BlackList.Contains(npc.type) || npc.dontCountMe)
                {
                    continue;
                }
                if (IsBoss(npc))
                {
                    _bosses.Add(npc);
                }
            }
        }

        private static bool IsBoss(NPC npc)
        {
            return npc.GetBossHeadTextureIndex() != -1 || BossCursor.WhiteList.ContainsKey(npc.type);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            // Draw nothing if cursor is disabled
            if (!_drawCursors)
            {
                return;
            }

            // Get the player position
            var playerPos = Main.LocalPlayer.Center;

            //Get UI scale and prepare scaling factor
            var posScaleFactor = 1f / Main.UIScale;

            // Draw an arrow for each boss
            foreach (var boss in _bosses.Where(IsBoss))
            {
                if (_config.HideOnScreen)
                {
                    // Hide cursor if boss is on screen
                    var p = boss.Center - Main.screenPosition;
                    if (!(p.X < 0 || p.Y < 0 || p.X > Main.screenWidth * Main.UIScale || p.Y > Main.screenHeight * Main.UIScale))
                    {
                        continue;
                    }
                }
                // Get the vector pointing towards the boss
                var bossVector = boss.Center - playerPos;
                
                // reverse arrow if gravitation potion effect is active
                bossVector.Y *= Main.LocalPlayer.gravDir;
                
                // Defines variables used to for drawing
                var modifier = Clamp(1.15f - 1 / (2f * Main.screenWidth) * bossVector.Length(), 0.02f, 1f);
                var alpha = modifier * 0.9f;
                var scale = modifier * 1.2f ;
                bossVector.Normalize();
                var arrowPos = playerPos + bossVector * _config.CursorDistance - Main.screenPosition;
                arrowPos *= posScaleFactor;

                var rotation = (float) Math.Atan2(bossVector.Y, bossVector.X);
                
                // Draw the arrow
                var tex = GetCursorTexture();
                spriteBatch.Draw(
                    tex,
                    arrowPos, 
                    null,
                    Color.White * alpha,
                    rotation,
                    tex.Size() / 2f,
                    1.2f * _config.CursorSize,
                    SpriteEffects.None,
                    1);

                // Draw the boss head
                var headTex = GetHeadTexture(boss);
                var headPos = playerPos + bossVector * (_config.CursorDistance - (HeadDistance * Main.UIScale) * _config.CursorSize) - Main.screenPosition;
                headPos *= posScaleFactor;
                spriteBatch.Draw(
                    headTex,
                    headPos,
                    null,
                    Color.White * alpha,
                    0f,
                    headTex.Size() * 0.5f,
                    scale * _config.CursorSize,
                    boss.GetBossHeadSpriteEffects(),
                    0);
            }
        }

        private static Texture2D GetHeadTexture(NPC boss)
        {
            if (BossCursor.WhiteList.ContainsKey(boss.type))
            {
                return BossCursor.WhiteList[boss.type];
            }

            var texIndex = boss.GetBossHeadTextureIndex();
            if (texIndex < 0 || texIndex >= TextureAssets.NpcHeadBoss.Length)
            {
                return TextureAssets.Projectile[ProjectileID.ShadowBeamFriendly].Value;
            }

            return TextureAssets.NpcHeadBoss[boss.GetBossHeadTextureIndex()].Value;
        }

        private static Texture2D GetCursorTexture()
        {
            return ModContent.Request<Texture2D>(CursorTexturePath).Value;
        }

        private static float Clamp(float value, float min, float max)
        {
            if (value > max)
            {
                return max;
            }
            if (value < min)
            {
                return min;
            }
            return value;
        }
    }
}
