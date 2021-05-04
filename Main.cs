using System.Reflection;
using UnityModManagerNet;
using HarmonyLib;

namespace AdofaiUtils
{
    internal static class Main
    {
        private static Harmony _harmony;
        private static UnityModManager.ModEntry _mod;

        private static bool Load(UnityModManager.ModEntry modEntry)
        {
            _mod = modEntry;
            _mod.OnToggle = OnToggle;
            
            return true;
        }

        private static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            _mod = modEntry;

            if (value) StartTweaks();
            else StopTweaks();

            return true;
        }

        private static void StartTweaks()
        {
            _harmony = new Harmony(_mod.Info.Id);
            _harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        private static void StopTweaks()
        {
            _harmony.UnpatchAll(_harmony.Id);
        }
    }
}
