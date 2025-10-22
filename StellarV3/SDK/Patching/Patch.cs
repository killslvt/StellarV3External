using HarmonyLib;
using Il2Cpp;
using Il2CppBestHTTP.Logger;
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
                Logging.Log("[Patch Error] Failed to patch '" + targetMethod?.Name + "':\n" + ex, LType.Error);
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
                Logging.Log("OnPlayerJoin patch applied successfully.", LType.Success);
            }
            catch (Exception ex)
            {
                Logging.Log("[Patch Error] Failed to patch OnPlayerJoin:\n" + ex, LType.Error);
            }

            try //On Player Leave
            {
                DoPatch(
                    typeof(NetworkManager).GetMethod(nameof(NetworkManager.Method_Public_Void_Player_0)),
                    GetPatchMethod(nameof(OnPlayerLeavePatch))
                );
                Logging.Log("OnPlayerLeave patch applied successfully.", LType.Success);
            }
            catch (Exception ex)
            {
                Logging.Log("[Patch Error] Failed to patch OnPlayerLeave:\n" + ex, LType.Error);
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

                Logging.Log("Console Spam patch applied successfully.", LType.Success);
            }
            catch (Exception ex)
            {
                Logging.Log("[Patch Error] Failed to patch Console Spam:\n" + ex, LType.Error);
            }
        }

        internal static bool StopSpam() => false;

        #region OnPlayerJoin/Leave
        private static void OnPlayerJoinPatch(VRC.Player __0)
        {
            Logging.Log($"Player joined: {__0.field_Private_APIUser_0.displayName}", LType.Join);
        }

        private static void OnPlayerLeavePatch(VRC.Player __0)
        {
            Logging.Log($"Player left: {__0.field_Private_APIUser_0.displayName}", LType.Leave);
        }
        #endregion
    }
}
