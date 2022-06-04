using BossCursor.Systems;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace BossCursor
{
    public class BossCursorPlayer : ModPlayer
    {
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (!KeybindSystem.ToggleCursorHotKey.JustPressed || Main.dedServ) return;
            BossCursor.CursorEnabled = !BossCursor.CursorEnabled;
            Main.NewText(BossCursor.CursorEnabled ? "Boss Cursor enabled" : "Boss Cursor disabled", Color.Cyan);
        }
    }
}
