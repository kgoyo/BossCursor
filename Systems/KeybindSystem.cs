using Terraria.ModLoader;

namespace BossCursor.Systems;

public class KeybindSystem : ModSystem
{
    public static ModKeybind ToggleCursorHotKey { get; private set; }

    public override void Load()
    {
        ToggleCursorHotKey = KeybindLoader.RegisterKeybind(Mod, "Toggle Boss Cursor", "B");
    }

    public override void Unload()
    {
        ToggleCursorHotKey = null;
    }
}