using Il2Cpp;
using UnityEngine;

namespace StellarV3.SDK.Utils
{
    internal class PortalUtils
    {
        public static void SpawnPortal(Vector3 positon, string worldSecureCode)
        {
            PortalManager.Method_Public_Static_Boolean_String_Boolean_Vector3_Quaternion_String_Action_1_LocalizableString_0(
                worldSecureCode,
                true,
                positon,
                new Quaternion(0f, 0f, 0f, 0f),
                null,
                null);
        }
    }
}
