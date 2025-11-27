using Il2Cpp;
using UnityEngine;

namespace StellarV3.Features.Visuals
{
    internal class Clone
    {
        public static List<GameObject> AllClones = new List<GameObject>();
        public static void ClonePlayer(VRCPlayer player)
        {
            GameObject gameObject = UnityEngine.Object.Instantiate(player.field_Private_VRCAvatarManager_0.field_Private_GameObject_0);
            Animator component = gameObject.GetComponent<Animator>();
            if (component != null && component.isHuman)
            {
                Transform boneTransform = component.GetBoneTransform(HumanBodyBones.Head);
                if (boneTransform != null)
                {
                    boneTransform.localScale = Vector3.one;
                }
            }
            gameObject.name = $"{player.prop_Player_0.field_Private_APIUser_0.displayName}'s Clone";
            component.enabled = false;
            gameObject.transform.position = player.transform.position;
            gameObject.transform.rotation = player.transform.rotation;
            AllClones.Add(gameObject);
        }

        public static void CloneDestroy()
        {
            foreach (GameObject gameObject in AllClones)
            {
                UnityEngine.Object.Destroy(gameObject);
            }
        }
    }
}
