using StellarV3External.SDK.Utils;

namespace StellarV3External.Features.Movement
{
    internal static class SpeedHack
    {
        public static bool IsEnabled { get; private set; } = false;
        public static float SpeedMultiplier { get; set; } = 5f;

        private static float originalWalkSpeed;
        private static float originalRunSpeed;
        private static float originalStrafeSpeed;

        private static bool speedsCached = false;

        public static void Enable()
        {
            var localPlayer = PlayerUtils.LocalPlayer()?._vrcplayer;
            var playerApi = localPlayer?.prop_VRCPlayerApi_0;

            if (playerApi == null)
                return;

            if (!speedsCached)
            {
                originalWalkSpeed = playerApi.GetWalkSpeed();
                originalRunSpeed = playerApi.GetRunSpeed();
                originalStrafeSpeed = playerApi.GetStrafeSpeed();
                speedsCached = true;
            }

            playerApi.SetWalkSpeed(originalWalkSpeed * SpeedMultiplier);
            playerApi.SetRunSpeed(originalRunSpeed * SpeedMultiplier);
            playerApi.SetStrafeSpeed(originalStrafeSpeed * SpeedMultiplier);

            IsEnabled = true;
        }

        public static void Disable()
        {
            var localPlayer = PlayerUtils.LocalPlayer()?._vrcplayer;
            var playerApi = localPlayer?.prop_VRCPlayerApi_0;

            if (playerApi == null || !speedsCached)
                return;

            playerApi.SetWalkSpeed(originalWalkSpeed);
            playerApi.SetRunSpeed(originalRunSpeed);
            playerApi.SetStrafeSpeed(originalStrafeSpeed);

            speedsCached = false;
            IsEnabled = false;
        }
    }
}
