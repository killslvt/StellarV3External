using Il2Cpp;
using Il2CppVRC.Localization;

namespace StellarV3External.SDK.Utils
{
    internal class PopupUtils
    {
        public static void HudMessage(string content, string? description = null, float duration = 5f)
        {
            LocalizableString title = LocalizableStringExtensions.Localize(content, null, null, null);
            LocalizableString desc = description != null ? LocalizableStringExtensions.Localize(description, null, null, null) : null;

            VRCUiManager.field_Private_Static_VRCUiManager_0.field_Private_HudController_0.notification
                .Method_Public_Void_Sprite_LocalizableString_LocalizableString_Single_Object1PublicIDisposableObAc1BoObObUnique_1_Boolean_0(
                    /*QMLoader.LoadSprite("Whatever sprite you want")*/ null, title, desc, duration, null
                );
        }
    }
}
