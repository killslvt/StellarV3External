using Il2Cpp;
using Il2CppVRC.SDKBase;
using StellarV3.Features.Movement;
using StellarV3.SDK.Utils;
using UnityEngine;
using VRC;
using VRC.DataModel;
using static StellarV3.GUIButtonAPI.GUIButtonAPI;

namespace StellarV3.Menus
{
    internal class MovementGUI
    {
        #region UpdateShit
        public static void Update()
        {
            Flight.Update();
            GhostMode.Update();

            if (!PlayerUtils.LocalPlayer()) return;

            bool isGrounded = PlayerUtils.LocalPlayer().prop_VRCPlayerApi_0.IsPlayerGrounded();

            if (Input.GetKey(KeyCode.Space))
            {
                if (jetPack)
                {
                    Jump(PlayerUtils.LocalPlayer().prop_VRCPlayerApi_0.GetJumpImpulse());
                }
                else if (bunnyHop && isGrounded)
                {
                    Jump(PlayerUtils.LocalPlayer().prop_VRCPlayerApi_0.GetJumpImpulse());
                }
            }

            wasGroundedLastFrame = isGrounded;
        }

        private static void Jump(float jumpPower)
        {
            Vector3 velocity = PlayerUtils.LocalPlayer().prop_VRCPlayerApi_0.GetVelocity();
            velocity.y = jumpPower;
            PlayerUtils.LocalPlayer().prop_VRCPlayerApi_0.SetVelocity(velocity);
        }
        #endregion

        public static bool jetPack;
        public static bool bunnyHop;
        public static bool headFlipper;
        public static bool speedHack;

        public static NeckRange neck;

        public static float JumpMultiplier { get; set; } = 5f;

        private static bool wasGroundedLastFrame = true;

        private static int yOffset = 0;

        public static void Menu()
        {
            yOffset = 80;

            new GUIToggleButton("Bunny Hop", () =>
            {
                bunnyHop = true;
                PopupUtils.HudMessage("Bunny Hop", "Toggled On", 3);
            },
            () =>
            {
                bunnyHop = false;
                PopupUtils.HudMessage("Bunny Hop", "Toggled Off", 3);
            }, () => bunnyHop, yOffset);

            yOffset += 35;

            new GUIToggleButton("Jetpack", () =>
            {
                jetPack = true;
                PopupUtils.HudMessage("Inf Jump", "Toggled On", 3);
            },
            () =>
            {
                jetPack = false;
                PopupUtils.HudMessage("Inf Jump", "Toggled Off", 3);
            }, () => jetPack, yOffset);

            yOffset += 35;

            new GUIToggleButton("Flight", () =>
            {
                Flight.FlyEnabled = true;
                PopupUtils.HudMessage("Flight", "Toggled On", 3);
            },
            () =>
            {
                Flight.FlyEnabled = false;
                PopupUtils.HudMessage("Flight", "Toggled Off", 3);
            }, () => Flight.FlyEnabled, yOffset);

            yOffset += 35;

            new GUISlider("Flight Speed", Flight.FlySpeed, 0.1f, 10f, yOffset, (value) =>
            {
                Flight.FlySpeed = value;
            });

            yOffset += 35;

            new GUIToggleButton("Speed Hack", () =>
            {
                speedHack = true;
                SpeedHack.Enable();
                PopupUtils.HudMessage("Speed Hack", "Toggled On", 3);
            },
            () =>
            {
                speedHack = false;
                SpeedHack.Disable();
                PopupUtils.HudMessage("Speed Hack", "Toggled Off", 3);
            }, () => speedHack, yOffset);

            yOffset += 35;

            new GUISlider("Speed Hack", SpeedHack.SpeedMultiplier, 0.1f, 100f, yOffset, (value) =>
            {
                SpeedHack.SpeedMultiplier = value;
            });

            yOffset += 35;

            new GUIToggleButton("Head Flipper", () =>
            {
                headFlipper = true;
                Player player = Player.prop_Player_0;
                neck = player.GetComponent<GamelikeInputController>().field_Protected_NeckMouseRotator_0.field_Public_NeckRange_0;
                player.GetComponent<GamelikeInputController>().field_Protected_NeckMouseRotator_0.field_Public_NeckRange_0 = new NeckRange(float.MinValue, float.MaxValue, 0f);
                PopupUtils.HudMessage("Head Flipper", "Toggled On", 3);
            },
            () =>
            {
                headFlipper = false;
                Player player = Player.prop_Player_0;
                player.GetComponent<GamelikeInputController>().field_Protected_NeckMouseRotator_0.field_Public_NeckRange_0 = neck;
                PopupUtils.HudMessage("Head Flipper", "Toggled Off", 3);
            }, () => headFlipper, yOffset);

            yOffset += 35;

            new GUIToggleButton("Ghost Mode", () =>
            {
                GhostMode.EnableGhostMode();
                PopupUtils.HudMessage("Ghost Mode", "Toggled On", 3);
            },
            () =>
            {
                GhostMode.DisableGhostMode();
                PopupUtils.HudMessage("Ghost Mode", "Toggled Off", 3);
            }, () => GhostMode.ghostmode, yOffset);

            yOffset += 35;

            new GUISingleButton("Force Jump", () =>
            {
                Networking.LocalPlayer.SetJumpImpulse(JumpMultiplier);
                PopupUtils.HudMessage("Movement", "Enabled Force Jump", 3f);
            }, yOffset);

            yOffset += 35;

            new GUISlider("Jump Impulse", JumpMultiplier, 1f, 100f, yOffset, (value) =>
            {
                JumpMultiplier = value;
            });

            yOffset += 35;
        }
    }
}
