using Il2CppVRC.SDKBase;
using Il2CppVRC.Udon;
using Il2CppVRC.Udon.Common.Interfaces;
using StellarV3External.SDK;
using StellarV3External.SDK.Utils;
using UnityEngine;
using static StellarV3External.GUIButtonAPI.GUIButtonAPI;

namespace StellarV3External.Menus
{
    internal class WorldHacksGUI
    {
        private static int yOffset = 0;
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

        private static GameObject targetGameObject;
        private static string targetUdonEvent;
        private static bool eventTarget = false;
        private static NetworkEventTarget target = NetworkEventTarget.Self;

        public static void Menu()
        {
            yOffset = 80;

            new GUIToggleButton("Network Event Target", () =>
            {
                eventTarget = true;
                target = NetworkEventTarget.All;
                PopupUtils.HudMessage("Network Event Target", "Target Set To All", 3f);
            }, () =>
            {
                eventTarget = false;
                target = NetworkEventTarget.Self;
                PopupUtils.HudMessage("Network Event Target", "Target Set To Self", 3f);
            }, () => eventTarget, yOffset);

            yOffset += 35;

            new GUISingleButton("Set GameObject", () =>
            {
                string objectName = GUIUtility.systemCopyBuffer;
                if (string.IsNullOrWhiteSpace(objectName))
                {
                    PopupUtils.HudMessage("Custom Udon", "Clipboard is empty!", 3f);
                    return;
                }

                targetGameObject = GameObject.Find(objectName);
                if (targetGameObject != null)
                    PopupUtils.HudMessage("Custom Udon", $"GameObject Found: {targetGameObject.name}", 3f);
                else
                    PopupUtils.HudMessage("Custom Udon", $"GameObject \"{objectName}\" Not Found", 3f);
            }, yOffset);

            yOffset += 35;

            new GUISingleButton("Set Event", () =>
            {
                string clipboardEvent = GUIUtility.systemCopyBuffer;
                if (string.IsNullOrWhiteSpace(clipboardEvent))
                {
                    PopupUtils.HudMessage("Custom Udon", "Clipboard is empty!", 3f);
                    return;
                }

                targetUdonEvent = clipboardEvent;
                PopupUtils.HudMessage("Custom Udon", $"Event Set: {targetUdonEvent}", 3f);
            }, yOffset);

            yOffset += 35;

            new GUISingleButton("Send Custom Udon Event", () =>
            {
                try
                {
                    var udon = targetGameObject.GetComponent<UdonBehaviour>();
                    udon.SendCustomNetworkEvent(target, targetUdonEvent);
                    PopupUtils.HudMessage("Custom Udon", $"Sent Custom Udon Event {targetUdonEvent}", 3f);
                }
                catch (Exception ex)
                {
                    ClarityLib.Logs.Log($"Failed to send custom udon event: {ex}", LType.Error.ToString(), Logging.GetColor(LType.Error), System.ConsoleColor.Cyan, "Stellar");
                    PopupUtils.HudMessage("Custom Udon", "Failed to Send Event", 3f);
                }
            }, yOffset);

            yOffset += 35;

            if (worldLoaded)
            {
                new GUISingleButton("Start Game", () =>
                {
                    GameObject gameObject = GameObject.Find("Game Logic");
                    gameObject.GetComponent<UdonBehaviour>().SendCustomNetworkEvent(0, "SyncCountdown");
                    PopupUtils.HudMessage("Murder 4", "Started Game", 3f);
                }, yOffset);

                yOffset += 35;

                new GUISingleButton("End Game", () =>
                {
                    GameObject gameObject = GameObject.Find("Game Logic");
                    gameObject.GetComponent<UdonBehaviour>().SendCustomNetworkEvent(0, "SyncAbort");
                    PopupUtils.HudMessage("Murder 4", "Ended Game", 3f);
                }, yOffset);

                yOffset += 35;

                new GUISingleButton("Blind All", () =>
                {
                    GameObject gameObject = GameObject.Find("Game Logic");
                    gameObject.GetComponent<UdonBehaviour>().SendCustomNetworkEvent(0, "OnLocalPlayerBlinded");
                    PopupUtils.HudMessage("Murder 4", "Blinded All Players", 3f);
                }, yOffset);

                yOffset += 35;

                new GUISingleButton("Victory B", () =>
                {
                    GameObject gameObject = GameObject.Find("Game Logic");
                    gameObject.GetComponent<UdonBehaviour>().SendCustomNetworkEvent(0, "SyncVictoryB");
                    PopupUtils.HudMessage("Murder 4", "Blinded All Players", 3f);
                }, yOffset);

                yOffset += 35;

                new GUISingleButton("Victory M", () =>
                {
                    GameObject gameObject = GameObject.Find("Game Logic");
                    gameObject.GetComponent<UdonBehaviour>().SendCustomNetworkEvent(0, "SyncVictoryM");
                    PopupUtils.HudMessage("Murder 4", "Blinded All Players", 3f);
                }, yOffset);

                yOffset += 35;

                new GUISingleButton("Lights Off", () =>
                {
                    List<UdonBehaviour> list = new List<UdonBehaviour>();
                    Transform transform = GameObject.Find("Game Logic/Switch Boxes").transform;
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        list.Add(transform.GetChild(i).GetComponent<UdonBehaviour>());
                    }
                    for (int j = 0; j < list.Count; j++)
                    {
                        list[j].SendCustomNetworkEvent(0, "SwitchDown");
                    }
                    PopupUtils.HudMessage("Murder 4", "Forced Lights Off", 3f);
                }, yOffset);

                yOffset += 35;

                new GUISingleButton("Lights On", () =>
                {
                    List<UdonBehaviour> list = new List<UdonBehaviour>();
                    Transform transform = GameObject.Find("Game Logic/Switch Boxes").transform;
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        list.Add(transform.GetChild(i).GetComponent<UdonBehaviour>());
                    }
                    for (int j = 0; j < list.Count; j++)
                    {
                        list[j].SendCustomNetworkEvent(0, "SwitchUp");
                    }
                    PopupUtils.HudMessage("Murder 4", "Forced Lights On", 3f);
                }, yOffset);

                yOffset += 35;

                new GUISingleButton("Bring Revolver", () =>
                {
                    GameObject gameObject = GameObject.Find("Game Logic/Weapons/Revolver");
                    Networking.SetOwner(Networking.LocalPlayer, gameObject);
                    gameObject.transform.position = Networking.LocalPlayer.gameObject.transform.position + new Vector3(0f, 0.1f, 0f);
                    PopupUtils.HudMessage("Murder 4", "Brought Revolver", 3f);
                }, yOffset);

                yOffset += 35;

                new GUISingleButton("Bring Shotgun", () =>
                {
                    GameObject gameObject = GameObject.Find("Game Logic/Weapons/Unlockables/Shotgun (0)");
                    Networking.SetOwner(Networking.LocalPlayer, gameObject);
                    gameObject.transform.position = Networking.LocalPlayer.gameObject.transform.position + new Vector3(0f, 0.1f, 0f);
                    PopupUtils.HudMessage("Murder 4", "Brought Shotgun", 3f);
                }, yOffset);

                yOffset += 35;

                new GUISingleButton("Bring Luger", () =>
                {
                    GameObject gameObject = GameObject.Find("Game Logic/Weapons/Unlockables/Luger (0)");
                    Networking.SetOwner(Networking.LocalPlayer, gameObject);
                    gameObject.transform.position = Networking.LocalPlayer.gameObject.transform.position + new Vector3(0f, 0.1f, 0f);
                    PopupUtils.HudMessage("Murder 4", "Brought Luger", 3f);
                }, yOffset);

                yOffset += 35;

                new GUISingleButton("Open Doors", () =>
                {
                    DoorManager.OpenAllDoors();
                    PopupUtils.HudMessage("Murder 4", "Opened All Doors", 3f);
                }, yOffset);

                yOffset += 35;

                new GUISingleButton("Close Doors", () =>
                {
                    DoorManager.CloseAllDoors();
                    PopupUtils.HudMessage("Murder 4", "Closed All Doors", 3f);
                }, yOffset);

                yOffset += 35;

                new GUISingleButton("Lock Doors", () =>
                {
                    DoorManager.LockAllDoors();
                    PopupUtils.HudMessage("Murder 4", "Locked All Doors", 3f);
                }, yOffset);

                yOffset += 35;
            }
        }

        #region Features
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
            }

            public static void LockAllDoors()
            {
                InteractWithAll("lock");
            }

            public static void UnlockAllDoors()
            {
                InteractWithAll("shove", 4);
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
