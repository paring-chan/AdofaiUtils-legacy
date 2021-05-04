using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using ADOFAI;
using HarmonyLib;
using UnityEngine;

namespace AdofaiUtils
{
    [HarmonyPatch(typeof(scnCLS), "Update")]
    internal static class KeyBind
    {
        private static MethodBase _toggleSpeedTrial = typeof(scnCLS).GetMethod("ToggleSpeedTrial", AccessTools.all);
        private static MethodBase _deleteLevel = typeof(scnCLS).GetMethod("DeleteLevel", AccessTools.all);
        private static MethodBase _toggleSearchMode = typeof(scnCLS).GetMethod("ToggleSearchMode", AccessTools.all);
        private static MethodBase _incrementSortType = typeof(scnCLS).GetMethod("IncrementSortType", AccessTools.all);
        private static MethodBase _shiftPlanet = typeof(scnCLS).GetMethod("ShiftPlanet", AccessTools.all);
        private static MethodBase _loadSong = typeof(scnCLS).GetMethod("LoadSong", AccessTools.all);


        internal static bool Prefix(scnCLS __instance, ref float ___autoscrollTimer, ref scrCamera ___camera,
            bool ___disablePlanets, bool ___searchMode, ref float ___holdTimer, bool ___changingLevel,
            bool ___instantSelect, string ___levelToSelect, ref string ___newSongKey, ref float ___levelTransitionTimer,
            ref Coroutine ___loadSongCoroutine, Dictionary<string, bool> ___loadedLevelIsDeleted)
        {
            void Invoke(MethodBase methodBase, params object[] parameters)
            {
                methodBase.Invoke(__instance, parameters);
            }

            float num1 = Screen.width * 1f / Screen.height;
            float num2 = __instance.canvasScaler.referenceResolution.x / __instance.canvasScaler.referenceResolution.y;
            __instance.canvasScaler.matchWidthOrHeight = (double) num1 >= (double) num2 ? 1f : 0.0f;
            ___camera.camobj.orthographicSize = 5f * Mathf.Max(1f, num2 / num1);
            __instance.signContainer.LocalMoveY(___camera.camobj.orthographicSize - 1.4f);
            SteamIntegration.Instance.CheckCallbacks();
            SteamWorkshop.CheckDownloadInfo();

            if (___disablePlanets)
                __instance.controller.responsive = false;
            if (!__instance.controller.paused && !___searchMode && __instance.controller.responsive)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    __instance.Refresh();
                    return false;
                }

                if (Input.GetKeyDown(KeyCode.W))
                {
                    SteamWorkshop.OpenWorkshop();
                    return false;
                }

                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    if (___loadedLevelIsDeleted[___levelToSelect]) return false;
                    __instance.EnterLevel();
                    return false;
                }

                if (Input.GetKeyDown(KeyCode.S))
                {
                    Invoke(_toggleSpeedTrial);
                    return false;
                }

                if (Input.GetKeyDown(KeyCode.Delete))
                {
                    Invoke(_deleteLevel, ___levelToSelect);
                    return false;
                }

                if (Input.GetKeyDown(KeyCode.F))
                    Invoke(_toggleSearchMode, true);
                if (Input.GetKeyDown(KeyCode.O))
                    Invoke(_incrementSortType);
                if (Input.GetKeyDown(KeyCode.Alpha7))
                {
                    scnCLS.loadSongMode = (scnCLS.loadSongMode + 1) % 3;
                    if (scnCLS.loadSongMode == 0)
                        MonoBehaviour.print("loading all");
                    if (scnCLS.loadSongMode == 1)
                        MonoBehaviour.print("not loading mp3s");
                    if (scnCLS.loadSongMode == 2)
                        MonoBehaviour.print("loading none");
                }

                if (!__instance.controller.moving)
                {
                    if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
                    {
                        ___holdTimer += Time.deltaTime;
                    }
                    else
                    {
                        ___holdTimer = 0.0f;
                        ___autoscrollTimer = 0.0f;
                    }

                    if ((double) ___holdTimer > __instance.secondsForHold)
                    {
                        ___autoscrollTimer += Time.deltaTime *
                                              ((double) ___holdTimer >
                                               (double) __instance.secondsForHoldExtra
                                                  ? 2f
                                                  : 1f);
                        if (___autoscrollTimer > (double) __instance.autoScrollInterval)
                        {
                            if (Input.GetKey(KeyCode.UpArrow))
                                Invoke(_shiftPlanet, false);
                            else
                                Invoke(_shiftPlanet, true);
                            ___autoscrollTimer = 0.0f;
                        }
                    }
                    else if (Input.GetKeyDown(KeyCode.UpArrow))
                        Invoke(_shiftPlanet, false);
                    else if (Input.GetKeyDown(KeyCode.DownArrow))
                        Invoke(_shiftPlanet, true);
                }
            }

            if (___changingLevel)
            {
                if (___levelTransitionTimer >= (___instantSelect
                    ? __instance.portalTransitionTimeInstant
                    : (double) __instance.portalTransitionTimeNormal))
                {
                    __instance.DisplayLevel(___levelToSelect);
                    LevelData loadedLevel = __instance.loadedLevels[___levelToSelect];
                    if (!string.IsNullOrEmpty(loadedLevel.songFilename))
                    {
                        string path = Path.Combine(__instance.loadedLevelDirs[___levelToSelect],
                            loadedLevel.songFilename);
                        ___newSongKey = Path.GetFileName(path) + "*external";
                        if (!path.ToLower().EndsWith(".mp3"))
                            ___loadSongCoroutine =
                                __instance.StartCoroutine((IEnumerator) _loadSong.Invoke(__instance,
                                    new object[] {path, ___newSongKey}));
                    }

                    ___instantSelect = false;
                }
                else
                    ___levelTransitionTimer += Time.deltaTime;
            }

            __instance.portalAndSign.MoveY(___camera.yGlobal);
            return false;
        }
    }
}