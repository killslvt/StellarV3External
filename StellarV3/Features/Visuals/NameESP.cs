using StellarV3.SDK.Utils;
using UnityEngine;

namespace StellarV3.Features.Visuals
{
    internal class NameESP
    {
        public static bool nameESP = false;
        public static bool trustColor = false;

        private static GUIStyle textStyle = new GUIStyle
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = 14,
            normal = { textColor = Color.white }
        };

        public static void OnGUI()
        {
            if (!nameESP) return;

            Camera cam = Camera.main;
            if (cam == null) return;

            foreach (var player in PlayerUtils.GetAllPlayers())
            {
                if (player == null || player.field_Private_VRCPlayerApi_0 == null)
                    continue;

                Vector3 worldPos = player.transform.position;
                worldPos.y -= 0.3f;

                Vector3 screenPos = cam.WorldToScreenPoint(worldPos);

                if (screenPos.z > 0f)
                {
                    float x = screenPos.x;
                    float y = Screen.height - screenPos.y;

                    string playerName = player.field_Private_VRCPlayerApi_0.displayName;

                    if (trustColor)
                    {
                        textStyle.normal.textColor = PlayerUtils.GetTrustColor(player);
                    }
                    else
                    {
                        textStyle.normal.textColor = Color.white;
                    }

                    BlackOutline(new Rect(x - 75f, y, 150f, 20f), playerName, textStyle);
                }
            }
        }

        private static void BlackOutline(Rect rect, string text, GUIStyle style)
        {
            Color oldColor = style.normal.textColor;

            style.normal.textColor = Color.black;
            GUI.Label(new Rect(rect.x + 1, rect.y + 1, rect.width, rect.height), text, style);

            style.normal.textColor = oldColor;
            GUI.Label(rect, text, style);
        }
    }
}
