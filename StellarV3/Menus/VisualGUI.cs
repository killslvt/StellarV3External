using StellarV3External.Features.Visuals;
using StellarV3External.SDK.Utils;
using static StellarV3External.GUIButtonAPI.GUIButtonAPI;

namespace StellarV3External.Menus
{
    internal class VisualGUI
    {
        private static bool capESP = false;
        
        private static int yOffset = 0;


        public static void Menu()
        {
            yOffset = 80;

            new GUIToggleButton("Name ESP", () =>
            {
                NameESP.nameESP = true;
                PopupUtils.HudMessage("Name ESP", "Toggled On", 3f);
            },
            () =>
            {
                NameESP.nameESP = false;
                PopupUtils.HudMessage("Name ESP", "Toggled Off", 3f);
            }, () => NameESP.nameESP, yOffset);

            yOffset += 35;

            new GUIToggleButton("Trust Rank Toggle", () =>
            {
                NameESP.trustColor = true;
                PopupUtils.HudMessage("Trust Rank", "Toggled On", 3f);
            },
            () =>
            {
                NameESP.trustColor = false;
                PopupUtils.HudMessage("Trust Rank", "Toggled Off", 3f);
            }, () => NameESP.trustColor, yOffset);

            yOffset += 35;
        }
    }
}
