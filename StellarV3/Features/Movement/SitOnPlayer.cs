using Il2Cpp;
using System.Collections;
using UnityEngine;
using ClarityLib;
using StellarV3External.SDK;

namespace StellarV3.Features.Movement
{
    internal class SitOnPlayer
    {
        public static bool isSitting;

        public enum BodyPart
        {
            Not_Attached,
            Head,
            Left_Hand,
            Left_Shoulder,
            Right_Hand,
            Right_Shoulder
        }

        private static readonly Dictionary<BodyPart, HumanBodyBones> BodyPartToBone = new Dictionary<BodyPart, HumanBodyBones>()
        {
            { BodyPart.Head, HumanBodyBones.Head },
            { BodyPart.Left_Hand, HumanBodyBones.LeftHand },
            { BodyPart.Left_Shoulder, HumanBodyBones.LeftShoulder },
            { BodyPart.Right_Hand, HumanBodyBones.RightHand },
            { BodyPart.Right_Shoulder, HumanBodyBones.RightShoulder }
        };

        public static IEnumerator AttachTo(BodyPart attachLocation, VRC.Player target)
        {
            if (attachLocation == BodyPart.Not_Attached || target == null)
                yield break;

            if (!BodyPartToBone.TryGetValue(attachLocation, out var bone))
            {
                Logs.Log("Invalid BodyPart " + attachLocation, LType.Error.ToString(), Logging.GetColor(LType.Error), System.ConsoleColor.Cyan, "Stellar");
                Logging.Log("Invalid BodyPart " + attachLocation, LType.Error);
                yield break;
            }

            var localPlayer = VRCPlayer.field_Internal_Static_VRCPlayer_0;
            var vrcPlayerApi = target.field_Private_VRCPlayerApi_0;

            while (isSitting)
            {
                if (target == null || vrcPlayerApi == null)
                {
                    isSitting = false;
                    VRCPlayer.field_Internal_Static_VRCPlayer_0.gameObject.GetComponent<CharacterController>().enabled = true;
                    yield break;
                }

                if (Input.GetKeyDown(KeyCode.X))
                {
                    isSitting = false;
                    VRCPlayer.field_Internal_Static_VRCPlayer_0.gameObject.GetComponent<CharacterController>().enabled = true;
                    yield break;
                }

                localPlayer.transform.position = vrcPlayerApi.GetBonePosition(bone);
                VRCPlayer.field_Internal_Static_VRCPlayer_0.gameObject.GetComponent<CharacterController>().enabled = false;

                yield return null;
            }

            VRCPlayer.field_Internal_Static_VRCPlayer_0.gameObject.GetComponent<CharacterController>().enabled = true;
        }
    }
}
