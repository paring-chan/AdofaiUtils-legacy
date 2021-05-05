using System.Globalization;
using System.Reflection;
using AdofaiUtils.Tweaks;
using UnityModManagerNet;
using HarmonyLib;
using UnityEngine;

namespace AdofaiUtils
{
    internal static class Main
    {
        private static Harmony _harmony;
        private static UnityModManager.ModEntry _mod;
        internal static Settings settings;
        private static TextOverlay TextOverlay = new TextOverlay();

        private static bool Load(UnityModManager.ModEntry modEntry)
        {
            settings = Settings.Load<Settings>(modEntry);
            _mod = modEntry;
            _mod.OnToggle = OnToggle;
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;
            modEntry.OnUpdate = OnUpdate;

            return true;
        }

        private static void OnUpdate(UnityModManager.ModEntry entry, float f)
        {
            TextOverlay.Show();
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
