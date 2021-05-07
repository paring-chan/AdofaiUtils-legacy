using HarmonyLib;

namespace AdofaiUtils.Tweaks
{
    public class Misc
    {
        [HarmonyPatch(typeof(scrController), "Update")]
        private static class ScrUIControllerWaitForStartCo
        {
            private static void Prefix(scrController __instance)
            {
                var scrController = __instance;
                if (scrController.txtCaption != null && Main.settings.MiscSettings.HideSpeedTrial1Text && GCS.speedTrialMode && GCS.currentSpeedRun.ToString("0.0") == "1.0")
                {
                    scrController.txtCaption.text = __instance.caption;
                }
            }
        }
    }
}