using System.Reflection;
using UnityModManagerNet;
using HarmonyLib;

namespace AdofaiUtils
{
    internal static class Main
    {
        private static Harmony _harmony;
        private static UnityModManager.ModEntry _mod;
        private static Settings settings;

        private static bool Load(UnityModManager.ModEntry modEntry)
        {
            settings = Settings.Load<Settings>(modEntry);
            _mod = modEntry;
            _mod.OnToggle = OnToggle;
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;

            return true;
        }

        private static void OnGUI(UnityModManager.ModEntry entry)
        {
            settings.Draw(entry);
        }
        
        private static void OnSaveGUI(UnityModManager.ModEntry entry)
        {
            settings.Save(entry);
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
