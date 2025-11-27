using Il2CppVRC.Udon;
using UnityEngine;

namespace StellarV3.Menus.Worlds
{
    internal class Murder4Utils
    {
        private static readonly string[] DoorNames =
            {
                "Door", "Door (3)", "Door (4)", "Door (5)", "Door (6)", "Door (7)",
                "Door (15)", "Door (16)", "Door (8)", "Door (13)", "Door (17)",
                "Door (18)", "Door (19)", "Door (20)", "Door (21)", "Door (22)",
                "Door (23)", "Door (14)"
            };


        private static void InteractWithAll(string action, int repeat = 1)
        {
            foreach (string doorName in DoorNames)
            {
                Transform target = GameObject.Find(doorName)?.transform.Find($"Door Anim/Hinge/Interact {action}");
                if (target == null) continue;

                UdonBehaviour udon = target.GetComponent<UdonBehaviour>();
                if (udon == null) continue;

                for (int i = 0; i < repeat; i++)
                    udon.Interact();
            }
        }

        public static void CloseAllDoors()
        {
            InteractWithAll("close");
            LockAllDoors();
        }

        public static void LockAllDoors()
        {
            InteractWithAll("lock");
        }

        public static void UnlockAllDoors()
        {
            InteractWithAll("shove", 4);
            OpenAllDoors();
        }

        public static void OpenAllDoors()
        {
            InteractWithAll("open");
        }
    }
}
