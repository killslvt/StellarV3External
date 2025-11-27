using Il2CppVRC.SDKBase;
using StellarV3.Features.Visuals;
using StellarV3.SDK.Utils;
using UnityEngine;
using VRC.Networking;

namespace StellarV3.Features.Movement
{
    internal class GhostMode
    {
        public static bool ghostmode = false;

        public static void Update()
        {
            if (Input.GetKeyInt(KeyCode.LeftControl) && Input.GetKeyDownInt(KeyCode.U))
            {
                if (ghostmode)
                {
                    DisableGhostMode();
                }
                else
                {
                    EnableGhostMode();
                }
            }
        }

        public static void EnableGhostMode()
        {
            ghostmode = true;
            Networking.LocalPlayer.gameObject.GetComponent<FlatBufferNetworkSerializer>().enabled = false;
            Clone.ClonePlayer(PlayerUtils.LocalPlayer().prop_VRCPlayer_0);
            PopupUtils.HudMessage("Ghost Mode", "Toggled On", 3);
        }

        public static void DisableGhostMode()
        {
            ghostmode = false;
            Networking.LocalPlayer.gameObject.GetComponent<FlatBufferNetworkSerializer>().enabled = true;
            Clone.CloneDestroy();
            PopupUtils.HudMessage("Ghost Mode", "Toggled Off", 3);
        }
    }
}
