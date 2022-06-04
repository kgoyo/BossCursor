using System;
using System.Collections.Generic;
using BossCursor.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace BossCursor
{
	public class BossCursor : Mod
	{
	    
        public static bool CursorEnabled = true;
        public const string Modname = "BossCursor";
        public static HashSet<int> BlackList;
        public static Dictionary<int, Texture2D> WhiteList;
        private const string AddToBlackListMethod = "AddToBlackList";
        private const string AddNpcMethod = "AddNpc";

        public override void Load()
        {
            if (Main.dedServ) return;
            BlackList = new HashSet<int>();
            WhiteList = new Dictionary<int, Texture2D>();
        }

        public override void Unload()
        {
            if (Main.dedServ) return;
            BlackList = null;
            WhiteList = null;
        }

        public override void PostSetupContent()
        {
            var config = ModContent.GetInstance<Config>();
            if (config.BlackListPillars)
            {
                BlackList.Add(NPCID.LunarTowerSolar);
                BlackList.Add(NPCID.LunarTowerNebula);
                BlackList.Add(NPCID.LunarTowerVortex);
                BlackList.Add(NPCID.LunarTowerStardust);
            }
        }

        public override object Call(params object[] args)
        {
            if (args.Length < 1)
            {
                return null;
            }
            var methodToCall = args[0] as string;
            switch (methodToCall)
            {
                case AddToBlackListMethod:
                    if (args.Length == 2)
                    {
                        AddToBlackList(Convert.ToInt32(args[1]));
                    }
                    break;
                case AddNpcMethod:
                    if (args.Length == 3)
                    {
                        AddNpc(Convert.ToInt32(args[1]), args[2] as Texture2D);
                    }
                    break;
                default:
                    Logger.Warn($"Unknown method: '{methodToCall}'");
                    break;
            }
            return null;
        }

        /// <summary>
        /// Used for cross mod compatibility to add NPCs to the blacklist
        /// bossCursor.Call("AddToBlackList", ModContent.NPCType<MyModNpc>());
        /// </summary>
        /// <param name="npcType">The id of the NPC to add to the blacklist</param>
        public void AddToBlackList(int npcType)
        {
            BlackList.Add(npcType);
            Logger.Info($"NPC with id: [{npcType}] has been added to the blacklist");
        }

        /// <summary>
        /// Used for cross mod compatibility to make additional non boss NPCs get cursors
        /// bossCursor.Call("AddNpc", ModContent.NPCType<MyModNpc>(), ModContent.GetTexture("MyMod/MyTexture"));
        /// </summary>
        /// <param name="npcType">The id of the NPC to add to the whitelist</param>
        /// <param name="headTexture">The head texture to use</param>
        public void AddNpc(int npcType, Texture2D headTexture)
        {
            WhiteList.Add(npcType, headTexture);
            Logger.Info($"NPC with id: [{npcType}] has been added to the whitelist");
        }
    }
}