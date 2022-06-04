using System.Collections.Generic;
using BossCursor.UI;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace BossCursor.Systems;

public class UISystem : ModSystem
{
    private const string VanillaInterfaceLayer = "Vanilla: Entity Health Bars";
    private UserInterface bossCursorUserInterface;
    private UiCursor uiCursor;

    public override void Load()
    {
        if (Main.dedServ) return;
        uiCursor = new UiCursor();
        uiCursor.Activate();
        bossCursorUserInterface = new UserInterface();
        bossCursorUserInterface.SetState(uiCursor);
    }

    public override void Unload()
    {
        if (Main.dedServ) return;
        bossCursorUserInterface = null;
        uiCursor = null;
    }

    public override void UpdateUI(GameTime gameTime)
    {
        bossCursorUserInterface.Update(gameTime);
    }

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        int index = layers.FindIndex(layer => layer.Name.Equals(VanillaInterfaceLayer)) + 1;
        if (index != -1)
        {
            layers.Insert(index, new LegacyGameInterfaceLayer(BossCursor.Modname + ": UI",
                delegate
                {
                    bossCursorUserInterface.Draw(Main.spriteBatch, new GameTime());
                    return true;
                },
                InterfaceScaleType.UI));
        }
    }
}