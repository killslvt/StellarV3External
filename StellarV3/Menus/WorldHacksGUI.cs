using Il2CppVRC.SDKBase;
using Il2CppVRC.Udon;
using Il2CppVRC.Udon.Common.Interfaces;
using MelonLoader;
using StellarV3External.SDK.Utils;
using System.Collections;
using UnityEngine;
using static StellarV3External.GUIButtonAPI.GUIButtonAPI;

namespace StellarV3External.Menus
{
    internal class WorldHacksGUI
    {
        public static bool worldLoaded = false;

        public static void Initialize(string sceneName)
        {
            if (sceneName == "Murder Nevermore")
            {
                worldLoaded = true;
            }
            else
            {
                worldLoaded = false;
            }
        }

        private static int yOffset = 0;

        public static void Menu()
        {
            yOffset = 80;

            if (worldLoaded)
            {
                new GUISingleButton("End Game", () =>
                {
                    GameObject gameObject = GameObject.Find("Game Logic");
                    gameObject.GetComponent<UdonBehaviour>().SendCustomNetworkEvent(0, "SyncAbort");
                    PopupUtils.HudMessage("Murder 4", "Ended Game", 3f);
                }, yOffset);

                yOffset += 35;

                new GUISingleButton("Win Game", () =>
                {
                    GameObject gameObject = GameObject.Find("Game Logic");
                    gameObject.GetComponent<UdonBehaviour>().SendCustomNetworkEvent(0, "SyncVictoryB");
                    PopupUtils.HudMessage("Murder 4", "Won Game", 3f);
                }, yOffset);

                yOffset += 35;

                new GUISingleButton("Tp Revolver", () =>
                {
                    GameObject gameObject = GameObject.Find("Game Logic/Weapons/Revolver");
                    Networking.SetOwner(Networking.LocalPlayer, gameObject);
                    gameObject.transform.position = Networking.LocalPlayer.gameObject.transform.position + new Vector3(0f, 0.1f, 0f);
                    PopupUtils.HudMessage("Murder 4", "Teleported Revolver", 3f);
                }, yOffset);

                yOffset += 35;

                new GUISingleButton("Open Doors", () =>
                {
                    DoorManager.OpenAllDoors();
                    PopupUtils.HudMessage("Murder 4", "Opened Doors", 3f);
                }, yOffset);

                yOffset += 35;

                new GUISingleButton("Close Doors", () =>
                {
                    DoorManager.CloseAllDoors();
                    PopupUtils.HudMessage("Murder 4", "Closed Doors", 3f);
                }, yOffset);

                yOffset += 35;

                new GUISingleButton("Kill All", () =>
                {
                    GameObject gameObject = GameObject.Find("Game Logic");
                    gameObject.GetComponent<UdonBehaviour>().SendCustomNetworkEvent(0, "KillLocalPlayer");
                    PopupUtils.HudMessage("Murder 4", "Killed All", 3f);
                }, yOffset);

                yOffset += 35;

                new GUIToggleButton("Patron Crash", () =>
                {
                    TargetLagAll = true;
                    MelonCoroutines.Start(PatreonCrash(0.5f));
                    PopupUtils.HudMessage("Patron Crash", "Toggled On", 3);
                },
                () =>
                {
                    TargetLagAll = false;
                    PopupUtils.HudMessage("Patron Crash", "Toggled Off", 3);
                }, () => TargetLagAll, yOffset);

                yOffset += 35;
            }
        }

        #region Features
        public static bool TargetLagAll = false;

        public static IEnumerator PatreonCrash(float delay)
        {
            GameObject targetObj = GameObject.Find("Patreon Credits");
            if (targetObj == null)
            {
                yield break;
            }

            UdonBehaviour udon = targetObj.GetComponent<UdonBehaviour>();
            if (udon == null)
            {
                yield break;
            }

            while (TargetLagAll)
            {
                udon.SendCustomNetworkEvent(NetworkEventTarget.All, "ListPatrons");
                yield return new WaitForSeconds(delay);
            }
        }
        public static class DoorManager
        {
            private static readonly string[] DoorNames =
            {
                "Door", "Door (3)", "Door (4)", "Door (5)", "Door (6)", "Door (7)",
                "Door (15)", "Door (16)", "Door (8)", "Door (13)", "Door (17)",
                "Door (18)", "Door (19)", "Door (20)", "Door (21)", "Door (22)",
                "Door (23)", "Door (14)"
            };


            private static void InteractWithAll(string action, int repeat = 1)
            {
                foreach (string doorName in DoorNames)
                {
                    Transform target = GameObject.Find(doorName)?.transform.Find($"Door Anim/Hinge/Interact {action}");
                    if (target == null) continue;

                    UdonBehaviour udon = target.GetComponent<UdonBehaviour>();
                    if (udon == null) continue;

                    for (int i = 0; i < repeat; i++)
                        udon.Interact();
                }
            }

            public static void CloseAllDoors()
            {
                InteractWithAll("close");
                LockAllDoors();
            }

            public static void LockAllDoors()
            {
                InteractWithAll("lock");
            }

            public static void UnlockAllDoors()
            {
                InteractWithAll("shove", repeat: 4);
                OpenAllDoors();
            }

            public static void OpenAllDoors()
            {
                InteractWithAll("open");
            }
        }
        #endregion
    }
}
