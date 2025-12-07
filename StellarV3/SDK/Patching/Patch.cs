using HarmonyLib;
using Il2Cpp;
using Il2CppPhoton.Client;
using Il2CppVRC.Core;
using Il2CppVRC.SDKBase;
using Photon.Realtime;
using StellarV3.SDK.Utils;
using System.Reflection;
using UnityEngine;

namespace StellarV3.SDK.Patching
{
    internal static class Patch
    {
        public static readonly HarmonyLib.Harmony Instance = new HarmonyLib.Harmony("StellarV3");

        private static HarmonyMethod GetPatchMethod(string methodName)
        {
            var method = typeof(Patch).GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic);
            return method != null ? new HarmonyMethod(method) : null;
        }

        private static void DoPatch(MethodInfo targetMethod, HarmonyMethod prefix = null, HarmonyMethod postfix = null)
        {
            try
            {
                Instance.Patch(targetMethod, prefix, postfix);
            }
            catch (Exception ex)
            {
                ClarityLib.Logs.Log("[Patch Error] Failed to patch '" + targetMethod?.Name + "':\n" + ex, LType.Error.ToString(), Logging.GetColor(LType.Error), System.ConsoleColor.Cyan, "Stellar");
                Logging.Log("[Patch Error] Failed to patch '" + targetMethod?.Name + "':\n" + ex, LType.Error);
            }
        }

        public static void Init()
        {
            try //On Player Join
            {
                DoPatch(
                    typeof(NetworkManager).GetMethod(nameof(NetworkManager.Method_Public_Void_Player_PDM_1)),
                    GetPatchMethod(nameof(OnPlayerJoinPatch))
                );
                ClarityLib.Logs.Log("OnPlayerJoin patch applied successfully", LType.Success.ToString(), Logging.GetColor(LType.Success), System.ConsoleColor.Cyan, "Stellar");
                Logging.Log("OnPlayerJoin patch applied successfully", LType.Success);
            }
            catch (Exception ex)
            {
                ClarityLib.Logs.Log("[Patch Error] Failed to patch OnPlayerJoin:" + ex, LType.Error.ToString(), Logging.GetColor(LType.Error), System.ConsoleColor.Cyan, "Stellar");
                Logging.Log("[Patch Error] Failed to patch OnPlayerJoin:\n" + ex, LType.Error);
            }

            try //On Player Leave
            {
                DoPatch(
                    typeof(NetworkManager).GetMethod(nameof(NetworkManager.Method_Public_Void_Player_0)),
                    GetPatchMethod(nameof(OnPlayerLeavePatch))
                );
                ClarityLib.Logs.Log("OnPlayerLeave patch applied successfully", LType.Success.ToString(), Logging.GetColor(LType.Success), System.ConsoleColor.Cyan, "Stellar");
                Logging.Log("OnPlayerLeave patch applied successfully", LType.Success);
            }
            catch (Exception ex)
            {
                ClarityLib.Logs.Log("[Patch Error] Failed to patch OnPlayerLeave:\n" + ex, LType.Error.ToString(), Logging.GetColor(LType.Error), System.ConsoleColor.Cyan, "Stellar");
                Logging.Log("[Patch Error] Failed to patch OnPlayerLeave:\n" + ex, LType.Error);
            }

            try //VRCPlus Spoof (Credit: catnotadog https://discord.gg/fXVn2JJyuA)
            {
                Instance.Patch(
                    typeof(VRCPlusStatus).GetProperty(nameof(VRCPlusStatus.prop_ReactiveProperty_1_Boolean_0)).GetGetMethod(),
                    postfix: new HarmonyMethod(typeof(Patch), nameof(VRCSpoof)));
                ClarityLib.Logs.Log("VRCPlus patch applied successfully", LType.Success.ToString(), Logging.GetColor(LType.Success), System.ConsoleColor.Cyan, "Stellar");
                Logging.Log("VRCPlus patch applied successfully", LType.Success);
            }
            catch (Exception ex)
            {
                ClarityLib.Logs.Log("[Patch Error] Failed to patch VRCPlus:\n" + ex, LType.Error.ToString(), Logging.GetColor(LType.Error), System.ConsoleColor.Cyan, "Stellar");
                Logging.Log("[Patch Error] Failed to patch VRCPlus:\n" + ex, LType.Error);
            }

            try //On Event
            {
                DoPatch(
                    typeof(LoadBalancingClient).GetMethod(nameof(LoadBalancingClient.OnEvent)),
                    GetPatchMethod(nameof(OnEventPatch))
                );
                ClarityLib.Logs.Log("OnEvent patch applied successfully", LType.Success.ToString(), Logging.GetColor(LType.Success), System.ConsoleColor.Cyan, "Stellar");
                Logging.Log("OnEvent patch applied successfully", LType.Success);
            }
            catch (Exception ex)
            {
                ClarityLib.Logs.Log("[Patch Error] Failed to patch OnEvent:\n" + ex, LType.Error.ToString(), Logging.GetColor(LType.Error), System.ConsoleColor.Cyan, "Stellar");
                Logging.Log("[Patch Error] Failed to patch OnEvent:\n" + ex, LType.Error);
            }
        }

        #region OnEvent
        public static bool antiUdon = false;

        private static bool OnEventPatch(EventData __0)
        {
            var data = __0.CustomData;

            switch (__0.Code)
            {
                case 11: //Prob not needed 
                case 18:
                    return !antiUdon;
                default:
                    return true;
            }
        }
        #endregion

        #region OnPlayerJoin/Leave
        public static bool ownerSpoof = false;
        public static bool customSpoof = false;
        public static string customName = "StellarV3";
        public static VRC_Pickup[] _pickups;

        private static void OnPlayerJoinPatch(VRC.Player __0)
        {
            ClarityLib.Logs.Log($"Player joined: {__0.field_Private_APIUser_0.displayName}", LType.Join.ToString(), Logging.GetColor(LType.Join), System.ConsoleColor.Cyan, "Stellar");
            Logging.Log($"Player joined: {__0.field_Private_APIUser_0.displayName}", LType.Join);

            if (__0.field_Private_VRCPlayerApi_0.isLocal)
            {
                if (ownerSpoof)
                {
                    customSpoof = false;
                    string ownerSpoof = RoomManager.field_Internal_Static_ApiWorld_0.authorName ?? "DisplayName";

                    __0.field_Private_VRCPlayerApi_0.displayName = ownerSpoof;
                    __0.field_Private_APIUser_0.displayName = ownerSpoof;
                }
                else if (customSpoof)
                {
                    ownerSpoof = false;
                    __0.field_Private_VRCPlayerApi_0.displayName = customName;
                    __0.field_Private_APIUser_0.displayName = customName;
                }

                _pickups = Resources.FindObjectsOfTypeAll<VRC_Pickup>().ToArray();
            }
        }

        private static void OnPlayerLeavePatch(VRC.Player __0)
        {
            ClarityLib.Logs.Log($"Player left: {__0.field_Private_APIUser_0.displayName}", LType.Leave.ToString(), Logging.GetColor(LType.Leave), System.ConsoleColor.Cyan, "Stellar");
            Logging.Log($"Player left: {__0.field_Private_APIUser_0.displayName}", LType.Leave);
        }
        #endregion

        #region VRCPlus Spoof
        internal static void VRCSpoof(ref ReactiveProperty<bool> __result)
        {
            __result.field_Protected_T_0 = true;
        }
        #endregion
    }
}
