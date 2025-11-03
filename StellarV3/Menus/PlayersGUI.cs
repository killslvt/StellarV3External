using Il2Cpp;
using MelonLoader;
using StellarV3.Features.Movement;
using StellarV3External.Features.Exploits;
using StellarV3External.SDK.Utils;
using UnityEngine;
using static StellarV3External.GUIButtonAPI.GUIButtonAPI;

namespace StellarV3External.Menus
{
    internal class PlayersGUI
    {
        private static int yOffset = 0;
        public static VRC.Player selectedPlayer = null;
        private static bool inSitMenu = false;
        private static int currentPage = 0;
        private const int playersPerPage = 15;

        public static void Menu()
        {
            yOffset = 80;

            if (selectedPlayer == null)
            {
                var allPlayers = PlayerUtils.GetAllPlayers();
                if (allPlayers == null || allPlayers.Count == 0) return;

                int totalPlayers = allPlayers.Count;
                int totalPages = Mathf.CeilToInt((float)totalPlayers / playersPerPage);
                currentPage = Mathf.Clamp(currentPage, 0, totalPages - 1);

                int yStep = 30;
                int startIndex = currentPage * playersPerPage;
                int endIndex = Mathf.Min(startIndex + playersPerPage, totalPlayers);

                for (int i = startIndex; i < endIndex; i++)
                {
                    VRC.Player player = allPlayers[i];
                    string displayName = player?.field_Private_APIUser_0?.displayName ?? "Unknown";
                    Color rankColor = PlayerUtils.GetTrustColor(player);
                    string rankHex = PlayerUtils.ColorToHex(rankColor);

                    new GUISingleButton($"<color={rankHex}>{displayName}</color>", () =>
                    {
                        selectedPlayer = player;
                    }, yOffset);

                    yOffset += yStep;
                }

                if (currentPage > 0)
                {
                    new GUISingleButton("← Prev Page", () => currentPage--, yOffset);
                    yOffset += yStep;
                }

                if (currentPage < totalPages - 1)
                {
                    new GUISingleButton("Next Page →", () => currentPage++, yOffset);
                    yOffset += yStep;
                }
            }
            else if (inSitMenu)
            {
                SitOnPlayerMenu();
            }
            else
            {
                PlayerMenu(selectedPlayer);
            }
        }
        public static bool PortalSpamUser = false;

        private static void PlayerMenu(VRC.Player player)
        {
            yOffset = 80;

            if (player == null)
            {
                new GUISingleButton("Player is null", () => { }, yOffset);
                return;
            }

            string displayName = player.field_Private_APIUser_0?.displayName ?? "Unknown";

            new GUISingleButton("Teleport To Player", () =>
            {
                var local = PlayerUtils.LocalPlayer();
                if (local != null)
                {
                    local.transform.position = player.transform.position;
                    PopupUtils.HudMessage("Select User", $"Teleported to {displayName}'s position", 3f);
                }
            }, yOffset);

            yOffset += 35;

            new GUISingleButton("Force Clone", () =>
            {
                string avatarid = player.prop_ApiAvatar_0.id;
                PlayerUtils.CloneAvatar(avatarid);
                PopupUtils.HudMessage("Select User", "Force Cloned User", 3f);
            }, yOffset);

            yOffset += 35;

            new GUIToggleButton("Portal Spam", () =>
            {
                PortalSpamUser = true;
                if (PortalExploits.PortalCoroutine != null)
                    MelonCoroutines.Stop(PortalExploits.PortalCoroutine);

                PortalExploits.portalDrop = true;
                PortalExploits.PortalCoroutine = (Coroutine)MelonCoroutines.Start(PortalExploits.PortalDrop(selectedPlayer));
                PopupUtils.HudMessage("Portal Spam", "Toggled On", 3);
            },
            () =>
            {
                PortalSpamUser = false;
                if (PortalExploits.PortalCoroutine != null)
                {
                    MelonCoroutines.Stop(PortalExploits.PortalCoroutine);
                    PortalExploits.PortalCoroutine = null;
                }
                PopupUtils.HudMessage("Portal Spam", "Toggled Off", 3);
            },
            () => PortalSpamUser, yOffset);

            yOffset += 35;

            new GUISingleButton("Copy Avatar ID", () =>
            {
                var apiUser = player.field_Private_APIUser_0;
                if (apiUser != null)
                {
                    GUIUtility.systemCopyBuffer = player.prop_ApiAvatar_0.id;
                    PopupUtils.HudMessage("Avatar ID", $"Copied {apiUser.displayName}'s avatar id to clipboard", 3f);
                }
                else
                {
                    PopupUtils.HudMessage("Avatar ID", "API User is null", 3f);
                }
            }, yOffset);

            yOffset += 35;

            new GUISingleButton("Sit On Player", () =>
            {
                inSitMenu = true;
            }, yOffset);

            yOffset += 35;

            new GUISingleButton("Back To Player List", () =>
            {
                selectedPlayer = null;
            }, yOffset);
        }

        private static void SitOnPlayerMenu()
        {
            yOffset = 80;

            new GUISingleButton("Sit On Head", () =>
            {
                SitOnPlayer.isSitting = !SitOnPlayer.isSitting;
                if (SitOnPlayer.isSitting)
                {
                    MelonCoroutines.Start(SitOnPlayer.AttachTo(SitOnPlayer.BodyPart.Head, selectedPlayer));
                }
                else
                {
                    SitOnPlayer.isSitting = false;
                    VRCPlayer.field_Internal_Static_VRCPlayer_0.gameObject.GetComponent<CharacterController>().enabled = true;
                }
            }, yOffset);

            yOffset += 35;

            new GUISingleButton("Sit On Right Shoulder", () =>
            {
                SitOnPlayer.isSitting = !SitOnPlayer.isSitting;
                if (SitOnPlayer.isSitting)
                {
                    MelonCoroutines.Start(SitOnPlayer.AttachTo(SitOnPlayer.BodyPart.Right_Shoulder, selectedPlayer));
                }
                else
                {
                    SitOnPlayer.isSitting = false;
                    VRCPlayer.field_Internal_Static_VRCPlayer_0.gameObject.GetComponent<CharacterController>().enabled = true;
                }
            }, yOffset);

            yOffset += 35;

            new GUISingleButton("Sit On Left Shoulder", () =>
            {
                SitOnPlayer.isSitting = !SitOnPlayer.isSitting;
                if (SitOnPlayer.isSitting)
                {
                    MelonCoroutines.Start(SitOnPlayer.AttachTo(SitOnPlayer.BodyPart.Left_Shoulder, selectedPlayer));
                }
                else
                {
                    SitOnPlayer.isSitting = false;
                    VRCPlayer.field_Internal_Static_VRCPlayer_0.gameObject.GetComponent<CharacterController>().enabled = true;
                }
            }, yOffset);

            yOffset += 35;

            new GUISingleButton("Sit On Right Hand", () =>
            {
                SitOnPlayer.isSitting = !SitOnPlayer.isSitting;
                if (SitOnPlayer.isSitting)
                {
                    MelonCoroutines.Start(SitOnPlayer.AttachTo(SitOnPlayer.BodyPart.Right_Hand, selectedPlayer));
                }
                else
                {
                    SitOnPlayer.isSitting = false;
                    VRCPlayer.field_Internal_Static_VRCPlayer_0.gameObject.GetComponent<CharacterController>().enabled = true;
                }
            }, yOffset);

            yOffset += 35;

            new GUISingleButton("Sit On Left Hand", () =>
            {
                SitOnPlayer.isSitting = !SitOnPlayer.isSitting;
                if (SitOnPlayer.isSitting)
                {
                    MelonCoroutines.Start(SitOnPlayer.AttachTo(SitOnPlayer.BodyPart.Left_Hand, selectedPlayer));
                }
                else
                {
                    SitOnPlayer.isSitting = false;
                    VRCPlayer.field_Internal_Static_VRCPlayer_0.gameObject.GetComponent<CharacterController>().enabled = true;
                }
            }, yOffset);

            yOffset += 35;

            new GUISingleButton("Back", () =>
            {
                inSitMenu = false;
            }, yOffset);
        }
    }
}
