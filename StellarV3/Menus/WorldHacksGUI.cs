using Il2CppVRC.SDKBase;
using Il2CppVRC.Udon;
using Il2CppVRC.Udon.Common.Interfaces;
using MelonLoader;
using StellarV3.Features.Exploits;
using StellarV3.Menus.Worlds;
using StellarV3.SDK;
using StellarV3.SDK.Utils;
using UnityEngine;
using UnityEngine.UI;
using static StellarV3.GUIButtonAPI.GUIButtonAPI;

namespace StellarV3.Menus
{
    internal class WorldHacksGUI
    {
        private static int yOffset = 0;
        public static bool m4Loaded = false;
        public static bool amongusLoaded = false;

        public static void Initialize(string sceneName)
        {
            if (sceneName == "Murder Nevermore")
            {
				m4Loaded = true;
            }
            else
            {
                m4Loaded = false;
            }

            if (sceneName == "Skeld")
            {
                amongusLoaded = true;
            }
            else
            {
                amongusLoaded = false;
            }
        }

        public static GameObject targetGameObject;
        public static string targetUdonEvent;
        public static bool eventTarget = false;
        public static NetworkEventTarget target = NetworkEventTarget.Self;

        public static void Menu()
        {
            yOffset = 80;

            new GUIToggleButton("Network Event Target", () =>
            {
                eventTarget = true;
                target = NetworkEventTarget.Others;
                PopupUtils.HudMessage("Network Event Target", "Target Set To Other", 3f);
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

			new GUISingleButton("Log Udon Events To File", () =>
			{
				MelonCoroutines.Start(Udon.LogUdonEvents());
			}, yOffset);

			yOffset += 35;

            if (amongusLoaded)
            {
                new GUISingleButton("Start Game", () =>
                {
                    GameObject gameObject = GameObject.Find("Game Logic");
                    gameObject.GetComponent<UdonBehaviour>().SendCustomNetworkEvent(0, "SyncCountdown");
                    PopupUtils.HudMessage("Among Us", "Started Game", 3f);
                }, yOffset);

                yOffset += 35;

                new GUISingleButton("End Game", () =>
                {
                    GameObject gameObject = GameObject.Find("Game Logic");
                    gameObject.GetComponent<UdonBehaviour>().SendCustomNetworkEvent(0, "SyncAbort");
                    PopupUtils.HudMessage("Among Us", "Ended Game", 3f);
                }, yOffset);

                yOffset += 35;

                new GUISingleButton("Self Imposter", () =>
                {
                    for (int i = 0; i < 24; i++)
                    {
                        if (GameObject.Find("Game Logic/Game Canvas/Game In Progress/Player List/Player List Group/Player Entry (" + i.ToString() + ")/Player Name Text").GetComponent<Text>().text.Equals(Networking.LocalPlayer.displayName))
                        {
                            GameObject.Find("Player Node (" + i.ToString() + ")").GetComponent<UdonBehaviour>().SendCustomNetworkEvent(0, "SyncAssignM");
                        }
                    }
                    PopupUtils.HudMessage("Among Us", "You Are Now A Imposter", 3f);
                }, yOffset);

                yOffset += 35;

                new GUISingleButton("Self Crewmate", () =>
                {
                    for (int i = 0; i < 24; i++)
                    {
                        if (GameObject.Find("Game Logic/Game Canvas/Game In Progress/Player List/Player List Group/Player Entry (" + i.ToString() + ")/Player Name Text").GetComponent<Text>().text.Equals(Networking.LocalPlayer.displayName))
                        {
                            GameObject.Find("Player Node (" + i.ToString() + ")").GetComponent<UdonBehaviour>().SendCustomNetworkEvent(0, "SyncAssignB");
                        }
                    }
                    PopupUtils.HudMessage("Among Us", "You Are Now A Crewmate", 3f);
                }, yOffset);

                yOffset += 35;

                new GUISingleButton("Kill All", () =>
                {
                    GameObject gameObject = GameObject.Find("Game Logic");
                    gameObject.GetComponent<UdonBehaviour>().SendCustomNetworkEvent(0, "KillLocalPlayer");
                    PopupUtils.HudMessage("Among Us", "Killed All Players", 3f);
                }, yOffset);

                yOffset += 35;

                new GUISingleButton("Start Emergency", () =>
                {
                    GameObject gameObject = GameObject.Find("Game Logic");
                    gameObject.GetComponent<UdonBehaviour>().SendCustomNetworkEvent(0, "StartMeeting");
                    gameObject.GetComponent<UdonBehaviour>().SendCustomNetworkEvent(0, "SyncEmergencyMeeting");
                    PopupUtils.HudMessage("Among Us", "Start Emergency", 3f);
                }, yOffset);

                yOffset += 35;

                new GUISingleButton("Finish All Task", () =>
                {
                    GameObject gameObject = GameObject.Find("Game Logic");
                    gameObject.GetComponent<UdonBehaviour>().SendCustomNetworkEvent(0, "OnLocalPlayerCompletedTask");
                    PopupUtils.HudMessage("Among Us", "Killed All Players", 3f);
                }, yOffset);

                yOffset += 35;

                new GUISingleButton("Crew Win", () =>
                {
                    GameObject gameObject = GameObject.Find("Game Logic");
                    gameObject.GetComponent<UdonBehaviour>().SendCustomNetworkEvent(0, "SyncVictoryC");
                    PopupUtils.HudMessage("Among Us", "Crew Won", 3f);
                }, yOffset);

                yOffset += 35;

                new GUISingleButton("Imposter Win", () =>
                {
                    GameObject gameObject = GameObject.Find("Game Logic");
                    gameObject.GetComponent<UdonBehaviour>().SendCustomNetworkEvent(0, "SyncVictoryI");
                    PopupUtils.HudMessage("Among Us", "Imposter Won", 3f);
                }, yOffset);

                yOffset += 35;
            }

            if (m4Loaded)
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
                    PopupUtils.HudMessage("Murder 4", "B Won", 3f);
                }, yOffset);

                yOffset += 35;

                new GUISingleButton("Victory M", () =>
                {
                    GameObject gameObject = GameObject.Find("Game Logic");
                    gameObject.GetComponent<UdonBehaviour>().SendCustomNetworkEvent(0, "SyncVictoryM");
                    PopupUtils.HudMessage("Murder 4", "M Won", 3f);
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
                    Murder4Utils.OpenAllDoors();
                    PopupUtils.HudMessage("Murder 4", "Opened All Doors", 3f);
                }, yOffset);

                yOffset += 35;

                new GUISingleButton("Close Doors", () =>
                {
                    Murder4Utils.CloseAllDoors();
                    PopupUtils.HudMessage("Murder 4", "Closed All Doors", 3f);
                }, yOffset);

                yOffset += 35;

                new GUISingleButton("Kill All", () =>
                {
                    GameObject gameObject = GameObject.Find("Game Logic");
                    gameObject.GetComponent<UdonBehaviour>().SendCustomNetworkEvent(0, "KillLocalPlayer");
                    PopupUtils.HudMessage("Murder 4", "Killed All Players", 3f);
                }, yOffset);

                yOffset += 35;
            }
		}
    }
}
