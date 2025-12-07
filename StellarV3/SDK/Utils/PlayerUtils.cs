using Il2Cpp;
using Il2CppTMPro;
using Il2CppVRC.Core;
using UnityEngine;
using VRC;
using VRC.UI.Elements;

namespace StellarV3.SDK.Utils
{
    internal static class PlayerUtils
    {
        public static Player LocalPlayer() => Player.prop_Player_0;
        public static Vector3 GetBonePosition(Player player, HumanBodyBones bone) => player.field_Private_VRCPlayerApi_0.GetBonePosition(bone);
        public static Color GetTrustColor(this Player player) => VRCPlayer.Method_Public_Static_Color_APIUser_0(player.GetAPIUser());
        public static APIUser GetAPIUser(this Player player) => player.prop_APIUser_0;

        public static Il2CppSystem.Collections.Generic.List<Player>? GetAllPlayers()
        {
            if (PlayerManager.prop_PlayerManager_0 == null)
            {
                return null;
            }

            return PlayerManager.prop_PlayerManager_0?.field_Private_List_1_Player_0;
        }

        public static void CloneAvatar(string avatar_id)
        {
            PageAvatar.Method_Public_Static_Void_ApiAvatar_String_0(new ApiAvatar
            {
                id = avatar_id
            });
        }

        public static string ColorToHex(Color color)
        {
            return "#" + ColorUtility.ToHtmlStringRGB(color);
        }
    }
}
