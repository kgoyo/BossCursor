using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace BossCursor
{
    public class Config : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;
        
        [Label("Hide cursor for on screen bosses")]
        [Tooltip("Hide the cursor for bosses, which are inside the view camera.")]
        [DefaultValue(false)]
        public bool HideOnScreen;
        
        [Label("Cursor distance")]
        [Tooltip("The distance the cursor is from the player.")]
        [Range(0,500)]
        [Increment(10)]
        [Slider]
        [DefaultValue(150)]
        public int CursorDistance;
        
        [Label("Cursor size scale")]
        [Tooltip("The scaling factor of the cursor.")]
        [Range(.1f, 2f)]
        [Increment(.1f)]
        [DrawTicks]
        [DefaultValue(1f)]
        public float CursorSize;

        [Label("Blacklist celestial pillars")]
        [Tooltip("Draw cursors for celestial pillars")]
        [ReloadRequired]
        [DefaultValue(true)]
        public bool BlackListPillars = true;
    }
}
