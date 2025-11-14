using HarmonyLib;
using Il2Cpp;
using Il2CppExitGames.Client.Photon;
using System.Reflection;

namespace StellarV3External.SDK.Patching
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
                    typeof(VRCPlusStatus).GetProperty(nameof(VRCPlusStatus.prop_Object1PublicIDisposableObAc1BoObObUnique_1_Boolean_0)).GetGetMethod(),
                    postfix: new HarmonyMethod(typeof(Patch), nameof(VRCSpoof)));
                ClarityLib.Logs.Log("VRCPlus patch applied successfully", LType.Success.ToString(), Logging.GetColor(LType.Success), System.ConsoleColor.Cyan, "Stellar");
                Logging.Log("VRCPlus patch applied successfully", LType.Success);
            }
            catch (Exception ex)
            {
                ClarityLib.Logs.Log("[Patch Error] Failed to patch VRCPlus:\n" + ex, LType.Error.ToString(), Logging.GetColor(LType.Error), System.ConsoleColor.Cyan, "Stellar");
                Logging.Log("[Patch Error] Failed to patch VRCPlus:\n" + ex, LType.Error);
            }
        }

        #region OnEvent
        //Not used currently but kept for future reference
        private static bool OnEventPatch(EventData __0)
        {
            var data = __0.CustomData;

            switch (__0.Code)
            {
                case 1:
                    break;
            }

            return true;
        }
        #endregion

        #region OnPlayerJoin/Leave
        private static void OnPlayerJoinPatch(VRC.Player __0)
        {
            ClarityLib.Logs.Log($"Player joined: {__0.field_Private_APIUser_0.displayName}", LType.Join.ToString(), Logging.GetColor(LType.Join), System.ConsoleColor.Cyan, "Stellar");
        }

        private static void OnPlayerLeavePatch(VRC.Player __0)
        {
            ClarityLib.Logs.Log($"Player left: {__0.field_Private_APIUser_0.displayName}", LType.Leave.ToString(), Logging.GetColor(LType.Leave), System.ConsoleColor.Cyan, "Stellar");
        }
        #endregion

        #region VRCPlus Spoof
        internal static void VRCSpoof(ref Object1PublicIDisposableObAc1BoObObUnique<bool> __result)
        {
            __result.field_Protected_T_0 = true;
        }
        #endregion
    }
}
