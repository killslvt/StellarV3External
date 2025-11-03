using HarmonyLib;
using Il2Cpp;
using Il2CppBestHTTP.Logger;
using Il2CppExitGames.Client.Photon;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppZLogger;
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
            }
        }

        public static void Init()
        {
            try //On Player Join
            {
                DoPatch(
                    typeof(NetworkManager).GetMethod(nameof(NetworkManager.Method_Public_Void_Player_PDM_2)),
                    GetPatchMethod(nameof(OnPlayerJoinPatch))
                );
                ClarityLib.Logs.Log("OnPlayerJoin patch applied successfully", LType.Success.ToString(), Logging.GetColor(LType.Success), System.ConsoleColor.Cyan, "Stellar");
            }
            catch (Exception ex)
            {
                ClarityLib.Logs.Log("[Patch Error] Failed to patch OnPlayerJoin:" + ex, LType.Error.ToString(), Logging.GetColor(LType.Error), System.ConsoleColor.Cyan, "Stellar");
            }

            try //On Player Leave
            {
                DoPatch(
                    typeof(NetworkManager).GetMethod(nameof(NetworkManager.Method_Public_Void_Player_0)),
                    GetPatchMethod(nameof(OnPlayerLeavePatch))
                );
                ClarityLib.Logs.Log("OnPlayerLeave patch applied successfully", LType.Success.ToString(), Logging.GetColor(LType.Success), System.ConsoleColor.Cyan, "Stellar");
            }
            catch (Exception ex)
            {
                ClarityLib.Logs.Log("[Patch Error] Failed to patch OnPlayerLeave:\n" + ex, LType.Error.ToString(), Logging.GetColor(LType.Error), System.ConsoleColor.Cyan, "Stellar");
            }

            try //ZLogger Spam Removal (Credit: catnotadog https://discord.gg/fXVn2JJyuA)
            {
                DoPatch(
                typeof(Il2CppVRC.Core.VRCLogger).GetMethod(
                    nameof(Il2CppVRC.Core.VRCLogger.Log),
                    new Type[] { typeof(ILogger), typeof(ZLoggerDebugInterpolatedStringHandler) }
                ),
                GetPatchMethod(nameof(StopSpam))
            );

                DoPatch(
                    typeof(Il2CppVRC.Core.ZLoggerHandlerLogger).GetMethod(
                        nameof(Il2CppVRC.Core.ZLoggerHandlerLogger.LogFormat),
                        new[] { typeof(UnityEngine.LogType), typeof(UnityEngine.Object), typeof(string), typeof(Il2CppReferenceArray<Il2CppSystem.Object>) }
                    ),
                    GetPatchMethod(nameof(StopSpam))
                );

                ClarityLib.Logs.Log("Console Spam patch applied successfully", LType.Success.ToString(), Logging.GetColor(LType.Success), System.ConsoleColor.Cyan, "Stellar");
            }
            catch (Exception ex)
            {
                ClarityLib.Logs.Log("[Patch Error] Failed to patch Console Spam:\n" + ex, LType.Error.ToString(), Logging.GetColor(LType.Error), System.ConsoleColor.Cyan, "Stellar");
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

        #region ZLogger
        internal static bool StopSpam() => false;
        #endregion
    }
}
