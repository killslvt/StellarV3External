using Il2CppVRC.SDKBase;
using UnityEngine;

namespace StellarV3.Features.Movement
{
    internal class ClickTP
    {
        public static void Update()
        {
            if (Input.GetKeyInt(KeyCode.LeftControl) && Input.GetKeyDownInt(KeyCode.Mouse0))
            {
                ClickTeleport();
            }
        }

        private static void ClickTeleport()
        {
            Ray posF = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit[] PosData = Physics.RaycastAll(posF);
            if (PosData.Length > 0) { RaycastHit pos = PosData[0]; Networking.LocalPlayer.gameObject.transform.position = pos.point; }
        }
    }
}
