using StellarV3.SDK.Utils;
using UnityEngine;

namespace StellarV3.Features.Movement
{
    internal class Flight
    {
        public static bool FlyEnabled = false;
        public static float FlySpeed = 0.2f;
        public static DateTime LastKeyCheck = DateTime.Now;

        private static bool setupedNormalFly = true;
        private static float cachedGravity = 0f;

        public static void Update()
        {
            if (Input.GetKeyInt(KeyCode.LeftControl) && Input.GetKeyDownInt(KeyCode.F))
            {
                FlyEnabled = !FlyEnabled;
                PopupUtils.HudMessage("Flight", FlyEnabled ? "Toggled On" : "Toggled Off", 3);
            }

            var localPlayer = PlayerUtils.LocalPlayer();
            if (localPlayer == null || localPlayer.field_Private_VRCPlayerApi_0 == null)
                return;

            var vrcApi = localPlayer.field_Private_VRCPlayerApi_0;

            if (!FlyEnabled)
            {
                if (!setupedNormalFly)
                {
                    vrcApi.SetGravityStrength(cachedGravity);

                    var collider = localPlayer.gameObject?.GetComponent<Collider>();
                    if (collider != null)
                        collider.enabled = true;

                    setupedNormalFly = true;
                }

                return;
            }

            if (setupedNormalFly)
            {
                cachedGravity = vrcApi.GetGravityStrength();
                vrcApi.SetGravityStrength(0f);

                var collider = localPlayer.gameObject?.GetComponent<Collider>();
                if (collider != null)
                    collider.enabled = false;

                setupedNormalFly = false;
            }

            Transform playerTransform = localPlayer.gameObject.transform;
            Transform cameraTransform = Camera.main?.transform;

            if (cameraTransform == null)
                return;

            float speed = FlySpeed;

            if (Input.GetKeyDown(KeyCode.LeftShift)) FlySpeed *= 2f;
            if (Input.GetKeyUp(KeyCode.LeftShift)) FlySpeed /= 2f;

            Vector3 movement = Vector3.zero;
            if (Input.GetKey(KeyCode.Q)) movement += -cameraTransform.up * speed;
            if (Input.GetKey(KeyCode.E)) movement += cameraTransform.up * speed;
            if (Input.GetKey(KeyCode.A)) movement += -cameraTransform.right * speed;
            if (Input.GetKey(KeyCode.D)) movement += cameraTransform.right * speed;
            if (Input.GetKey(KeyCode.S)) movement += -cameraTransform.forward * speed;
            if (Input.GetKey(KeyCode.W)) movement += cameraTransform.forward * speed;

            playerTransform.position += movement;
        }
    }
}
