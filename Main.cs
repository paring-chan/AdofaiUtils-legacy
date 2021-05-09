using System.Reflection;
using UnityModManagerNet;
using HarmonyLib;
using Patch = AdofaiUtils.attributes.Patch;

namespace AdofaiUtils
{
    internal static class Main
    {
        private static Harmony _harmony;
        internal static UnityModManager.ModEntry Mod;
        internal static Settings.Settings settings;

        private static bool Load(UnityModManager.ModEntry modEntry)
        {
            settings = Settings.Settings.Load<Settings.Settings>(modEntry);
            Mod = modEntry;
            Mod.OnToggle = OnToggle;
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
            Mod = modEntry;

            if (value) StartTweaks();
            else StopTweaks();

            return true;
        }

        private static void StartTweaks()
        {
            _harmony = new Harmony(Mod.Info.Id);
            
            Patch.PatchAll(_harmony);
            
            // _harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        private static void StopTweaks()
        {
            _harmony.UnpatchAll(_harmony.Id);
        }
    }
}
