using Terraria;
using Terraria.UI;

namespace BossCursor.UI
{
    public class UiCursor : UIState
    {
        public override void OnInitialize()
        {
            var cursorElement = new UiCursorElement();
            cursorElement.Width.Set(Main.screenWidth, 0);
            cursorElement.Height.Set(Main.screenHeight, 0);
            Append(cursorElement);
        }
    }
}
