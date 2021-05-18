using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using ADOFAI;
using AdofaiUtils.Attributes;
using HarmonyLib;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AdofaiUtils.Tweaks
{
    internal class KeyBindPatches
    {
        private static GameObject _infoGameObject = new GameObject();
        private static InfoBehavior _infoBehavior = null;

        private static bool shouldSkipPlay;

        [R68]
        [R71]
        [HarmonyPatch(typeof(CustomLevel), "LoadAndPlayLevel")]
        internal static class CustomLevelCheckForSpecialInputKeysOrPause
        {
            private static MethodBase _printesp = typeof(CustomLevel).GetMethod("printesp", AccessTools.all);

            private static MethodBase _setupConductorWithLevelData =
                typeof(CustomLevel).GetMethod("SetupConductorWithLevelData", AccessTools.all);

            internal static bool Prefix(string levelPath, CustomLevel __instance)
            {
                void Invoke(MethodBase methodBase, params object[] parameters)
                {
                    methodBase.Invoke(__instance, parameters);
                }

                Invoke(_printesp, (object) "");
                int num = __instance.LoadLevel(levelPath) ? 1 : 0;
                if (num == 0)
                    return num != 0;
                __instance.editor.filenameText.text = Path.GetFileName(levelPath);
                __instance.editor.filenameText.fontStyle = FontStyle.Bold;
                Invoke(_setupConductorWithLevelData);
                __instance.RemakePath();
                __instance.ReloadAssets();
                DiscordController.instance?.UpdatePresence();
                scnEditor.instance.settingsPanel.ShowPanel(LevelEventType.SongSettings);

                if (shouldSkipPlay)
                {
                    shouldSkipPlay = false;
                    return false;
                }

                return true;
            }
        }

        [R68]
        [R71]
        [HarmonyPatch(typeof(scrController), "CheckForSpecialInputKeysOrPause")]
        internal static class ScrControllerCheckForSpecialInputKeysOrPause
        {
            internal static bool Prefix(scrController __instance, ref bool __result)
            {
                if (__instance.levelEditorMode && !__instance.editor.controller.paused)
                {
                    if (Main.settings.KeyBindSettings.CustomPlayKeyBindSettings.Restart.Enabled &&
                        Main.settings.KeyBindSettings.CustomPlayKeyBindSettings.Restart.Down)
                    {
                        __result = true;
                        return false;
                    }
                }

                if (__instance.CLSMode)
                {
                    if (Main.settings.KeyBindSettings.ClsKeyBindSettings.MapInfo.Enabled &&
                        Main.settings.KeyBindSettings.ClsKeyBindSettings.MapInfo.Down)
                    {
                        __result = true;
                        return false;
                    }


                    if (Main.settings.KeyBindSettings.ClsKeyBindSettings.EnterMap.Enabled &&
                        Main.settings.KeyBindSettings.ClsKeyBindSettings.EnterMap.Down)
                    {
                        __result = true;
                        return false;
                    }

                    if (Main.settings.KeyBindSettings.ClsKeyBindSettings.Workshop.Enabled &&
                        Main.settings.KeyBindSettings.ClsKeyBindSettings.Workshop.Down)
                    {
                        __result = true;
                        return false;
                    }

                    if (Main.settings.KeyBindSettings.ClsKeyBindSettings.Reload.Enabled &&
                        Main.settings.KeyBindSettings.ClsKeyBindSettings.Reload.Down)
                    {
                        __result = true;
                        return false;
                    }

                    if (Main.settings.KeyBindSettings.ClsKeyBindSettings.Editor.Enabled &&
                        Main.settings.KeyBindSettings.ClsKeyBindSettings.Editor.Down)
                    {
                        __result = true;
                        return false;
                    }

                    if (Main.settings.KeyBindSettings.ClsKeyBindSettings.MapInfo.Enabled &&
                        Main.settings.KeyBindSettings.ClsKeyBindSettings.MapInfo.Down)
                    {
                        __result = true;
                        return false;
                    }
                }

                return true;
            }
        }

        internal class InfoBehavior : MonoBehaviour
        {
            private LevelData map;
            private string levelId;
            private static GUIStyle buttonStyle;

            public void SetMap(LevelData _map, string id)
            {
                map = _map;
                levelId = id;
            }

            private void OnGUI()
            {
                if (buttonStyle == null)
                {
                    buttonStyle = GUI.skin.button;
                    buttonStyle.stretchWidth = false;
                }

                GUILayout.BeginArea(new Rect(Screen.width / 2 - 200, Screen.height / 2 - 100, 400, 200), "맵 정보",
                    GUI.skin.window);

                var items = new[]
                {
                    new[]
                    {
                        "제목",
                        RDUtils.RemoveRichTags(map.artist + " - " + map.song)
                    },
                    new[]
                    {
                        "제작자",
                        RDUtils.RemoveRichTags(map.author)
                    },
                    new[]
                    {
                        "다운로드 링크",
                        "https://steamcommunity.com/sharedfiles/filedetails/?id=" + levelId
                    }
                };

                foreach (var item in items)
                {
                    GUILayout.Label(item[0]);
                    GUILayout.BeginHorizontal();
                    GUILayout.TextArea(item[1]);
                    if (GUILayout.Button("복사", buttonStyle))
                    {
                        GUIUtility.systemCopyBuffer = item[1];
                    }

                    GUILayout.EndHorizontal();
                }

                GUILayout.EndArea();
            }
        }

        [R71]
        [HarmonyPatch(typeof(scnCLS), "Update")]
        internal static class ClsKeyBind
        {
            private static MethodBase _toggleSpeedTrial = typeof(scnCLS).GetMethod("ToggleSpeedTrial", AccessTools.all);
            private static MethodBase _deleteLevel = typeof(scnCLS).GetMethod("DeleteLevel", AccessTools.all);
            private static MethodBase _toggleSearchMode = typeof(scnCLS).GetMethod("ToggleSearchMode", AccessTools.all);

            private static MethodBase _incrementSortType =
                typeof(scnCLS).GetMethod("IncrementSortType", AccessTools.all);


            private static MethodBase _shiftPlanet = typeof(scnCLS).GetMethod("ShiftPlanet", AccessTools.all);
            private static MethodBase _loadSong = typeof(scnCLS).GetMethod("LoadSong", AccessTools.all);


            internal static bool Prefix(scnCLS __instance, ref float ___autoscrollTimer, ref scrCamera ___camera,
                bool ___disablePlanets, bool ___searchMode, ref float ___holdTimer, bool ___changingLevel,
                ref bool ___instantSelect, string ___levelToSelect, ref string ___newSongKey,
                ref float ___levelTransitionTimer,
                ref Coroutine ___loadSongCoroutine, Dictionary<string, bool> ___loadedLevelIsDeleted,
                Dictionary<string, LevelData> ___loadedLevels)
            {
                if (_infoGameObject == null)
                {
                    _infoGameObject = new GameObject();
                }

                void Invoke(MethodBase methodBase, params object[] parameters)
                {
                    methodBase.Invoke(__instance, parameters);
                }

                float num1 = Screen.width * 1f / Screen.height;
                float num2 = __instance.canvasScaler.referenceResolution.x /
                             __instance.canvasScaler.referenceResolution.y;
                __instance.canvasScaler.matchWidthOrHeight = (double) num1 >= (double) num2 ? 1f : 0.0f;
                ___camera.camobj.orthographicSize = 5f * Mathf.Max(1f, num2 / num1);
                __instance.signContainer.LocalMoveY(___camera.camobj.orthographicSize - 1.4f);
                SteamIntegration.Instance.CheckCallbacks();
                SteamWorkshop.CheckDownloadInfo();

                if (___disablePlanets)
                {
                    __instance.controller.responsive = false;
                }

                if (!___searchMode && __instance.controller.responsive)
                {
                    if (Input.GetKeyDown(KeyCode.Escape) ||
                        Main.settings.KeyBindSettings.ClsKeyBindSettings.MapInfo.Down &&
                        Main.settings.KeyBindSettings.ClsKeyBindSettings.MapInfo.Enabled)
                    {
                        if (_infoBehavior != null)
                        {
                            Object.DestroyImmediate(_infoBehavior);
                            _infoBehavior = null;
                            scrController.instance.paused = false;
                            scrController.instance.audioPaused = false;
                            scrController.instance.enabled = true;
                            Time.timeScale = 1.0f;
                        }
                        else if (!__instance.controller.paused && !Input.GetKeyDown(KeyCode.Escape))
                        {
                            _infoBehavior = _infoGameObject.AddComponent<InfoBehavior>();
                            _infoBehavior.SetMap(___loadedLevels[___levelToSelect], ___levelToSelect);
                            scrController.instance.paused = true;
                            scrController.instance.audioPaused = true;
                            scrController.instance.enabled = false;
                            Time.timeScale = 0.0f;
                        }

                        return false;
                    }

                    if (!__instance.controller.paused)
                    {
                        if (Main.settings.KeyBindSettings.ClsKeyBindSettings.Reload.Down &&
                            Main.settings.KeyBindSettings.ClsKeyBindSettings.Reload.Enabled)
                        {
                            __instance.Refresh();
                            return false;
                        }

                        if (Main.settings.KeyBindSettings.ClsKeyBindSettings.Workshop.Down &&
                            Main.settings.KeyBindSettings.ClsKeyBindSettings.Workshop.Enabled)
                        {
                            SteamWorkshop.OpenWorkshop();
                            return false;
                        }

                        if (Main.settings.KeyBindSettings.ClsKeyBindSettings.EnterMap.Down &&
                            Main.settings.KeyBindSettings.ClsKeyBindSettings.EnterMap.Enabled)
                        {
                            if (___loadedLevelIsDeleted[___levelToSelect]) return false;
                            __instance.EnterLevel();
                            return false;
                        }

                        if (Main.settings.KeyBindSettings.ClsKeyBindSettings.EnterMap.Down &&
                            Main.settings.KeyBindSettings.ClsKeyBindSettings.Editor.Enabled)
                        {
                            if (___loadedLevelIsDeleted[___levelToSelect]) return false;
                            string levelPath = Path.Combine(__instance.loadedLevelDirs[___levelToSelect],
                                "main.adofai");
                            GCS.sceneToLoad = "scnEditor";
                            GCS.customLevelPaths = new string[1];
                            GCS.customLevelPaths[0] = levelPath;
                            GCS.standaloneLevelMode = false;
                            shouldSkipPlay = true;
                            __instance.controller.StartLoadingScene(WipeDirection.StartsFromRight);
                            __instance.editor.SwitchToEditMode();
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
                        {
                            Invoke(_toggleSearchMode, true);
                        }

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

        [R68]
        [HarmonyPatch(typeof(scnCLS), "Update")]
        internal static class ScnCLSUpdateR68
        {
            private static MethodBase _toggleSpeedTrial = typeof(scnCLS).GetMethod("ToggleSpeedTrial", AccessTools.all);
            private static MethodBase _deleteLevel = typeof(scnCLS).GetMethod("DeleteLevel", AccessTools.all);

            private static MethodBase _shiftPlanet = typeof(scnCLS).GetMethod("ShiftPlanet", AccessTools.all);
            private static MethodBase _loadSong = typeof(scnCLS).GetMethod("LoadSong", AccessTools.all);
            private static MethodBase _refresh = typeof(scnCLS).GetMethod("Refresh", AccessTools.all);

            private static bool Prefix(scnCLS __instance, scrCamera ___camera, bool ___disablePlanets,
                string ___levelToSelect, ref float ___holdTimer, ref float ___autoscrollTimer, bool ___changingLevel,
                ref float ___levelTransitionTimer, ref bool ___instantSelect, string ___newSongKey,
                ref Coroutine ___loadSongCoroutine, Dictionary<string, bool> ___loadedLevelIsDeleted,
                Dictionary<string, LevelData> ___loadedLevels)
            {
                if (_infoGameObject == null)
                {
                    _infoGameObject = new GameObject();
                }

                void Invoke(MethodBase methodBase, params object[] parameters)
                {
                    methodBase.Invoke(__instance, parameters);
                }

                if (Input.GetKeyDown(KeyCode.Alpha7))
                {
                    scnCLS.loadSongMode = (scnCLS.loadSongMode + 1) % 3;
                    if (scnCLS.loadSongMode == 0)
                        MonoBehaviour.print((object) "loading all");
                    if (scnCLS.loadSongMode == 1)
                        MonoBehaviour.print((object) "not loading mp3s");
                    if (scnCLS.loadSongMode == 2)
                        MonoBehaviour.print((object) "loading none");
                }

                float num1 = (float) Screen.width * 1f / (float) Screen.height;
                float num2 = __instance.canvasScaler.referenceResolution.x /
                             __instance.canvasScaler.referenceResolution.y;
                __instance.canvasScaler.matchWidthOrHeight = (double) num1 >= (double) num2 ? 1f : 0.0f;
                ___camera.camobj.orthographicSize = 5f * Mathf.Max(1f, num2 / num1);
                __instance.signContainer.LocalMoveY(___camera.camobj.orthographicSize - 1.4f);
                SteamIntegration.Instance.CheckCallbacks();
                SteamWorkshop.CheckDownloadInfo();
                if (___disablePlanets)
                    __instance.controller.responsive = false;
                if (Input.GetKeyDown(KeyCode.Escape) ||
                    Main.settings.KeyBindSettings.ClsKeyBindSettings.MapInfo.Down &&
                    Main.settings.KeyBindSettings.ClsKeyBindSettings.MapInfo.Enabled)
                {
                    if (_infoBehavior != null)
                    {
                        Object.DestroyImmediate(_infoBehavior);
                        _infoBehavior = null;
                        scrController.instance.paused = false;
                        scrController.instance.audioPaused = false;
                        scrController.instance.enabled = true;
                        Time.timeScale = 1.0f;
                    }
                    else if (!__instance.controller.paused && !Input.GetKeyDown(KeyCode.Escape))
                    {
                        _infoBehavior = _infoGameObject.AddComponent<InfoBehavior>();
                        _infoBehavior.SetMap(___loadedLevels[___levelToSelect], ___levelToSelect);
                        scrController.instance.paused = true;
                        scrController.instance.audioPaused = true;
                        scrController.instance.enabled = false;
                        Time.timeScale = 0.0f;
                    }

                    return false;
                }

                if (!__instance.controller.paused)
                {
                    if (Main.settings.KeyBindSettings.ClsKeyBindSettings.EnterMap.Down &&
                        Main.settings.KeyBindSettings.ClsKeyBindSettings.EnterMap.Enabled)
                    {
                        if (___loadedLevelIsDeleted[___levelToSelect]) return false;
                        __instance.EnterLevel();
                        return false;
                    }

                    if (Main.settings.KeyBindSettings.ClsKeyBindSettings.Editor.Down &&
                        Main.settings.KeyBindSettings.ClsKeyBindSettings.Editor.Enabled)
                    {
                        if (___loadedLevelIsDeleted[___levelToSelect]) return false;
                        string levelPath = Path.Combine(__instance.loadedLevelDirs[___levelToSelect],
                            "main.adofai");
                        GCS.sceneToLoad = "scnEditor";
                        GCS.customLevelPaths = new string[1];
                        GCS.customLevelPaths[0] = levelPath;
                        GCS.standaloneLevelMode = false;
                        shouldSkipPlay = true;
                        __instance.controller.StartLoadingScene(WipeDirection.StartsFromRight);
                        __instance.editor.SwitchToEditMode();
                        return false;
                    }


                    if (Main.settings.KeyBindSettings.ClsKeyBindSettings.Reload.Down &&
                        Main.settings.KeyBindSettings.ClsKeyBindSettings.Reload.Enabled)
                    {
                        Invoke(_refresh, false);
                        return false;
                    }

                    if (Input.GetKeyDown(KeyCode.S))
                    {
                        Invoke(_toggleSpeedTrial);
                        return false;
                    }

                    if (Main.settings.KeyBindSettings.ClsKeyBindSettings.Workshop.Down &&
                        Main.settings.KeyBindSettings.ClsKeyBindSettings.Workshop.Enabled)
                    {
                        SteamWorkshop.OpenWorkshop();
                        return false;
                    }

                    if (Input.GetKeyDown(KeyCode.Delete))
                    {
                        Invoke(_deleteLevel, ___levelToSelect);
                        return false;
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

                        if ((double) ___holdTimer > (double) __instance.secondsForHold)
                        {
                            ___autoscrollTimer += Time.deltaTime *
                                                  ((double) ___holdTimer > (double) __instance.secondsForHoldExtra
                                                      ? 2f
                                                      : 1f);
                            if ((double) ___autoscrollTimer > (double) __instance.autoScrollInterval)
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
                    if ((double) ___levelTransitionTimer >= (___instantSelect
                        ? (double) __instance.portalTransitionTimeInstant
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
                                    __instance.StartCoroutine(
                                        (IEnumerator) _loadSong.Invoke(__instance, new[] {path, ___newSongKey}));
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

        [R71]
        [R68]
        [HarmonyPatch(typeof(scnEditor), "Update")]
        internal static class ScnEditorUpdate
        {
            private static MethodBase _tryQuitToMenu = typeof(scnEditor).GetMethod("TryQuitToMenu", AccessTools.all);

            private static MethodBase _resetCustomLevel =
                typeof(scrController).GetMethod("ResetCustomLevel", AccessTools.all);

            private static bool Prefix(scnEditor __instance)
            {
                bool flag1 = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) ||
                             Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.RightCommand);
                bool flag2 = Main.settings.KeyBindSettings.EditorKeyBindSettings.Quit.Down;
                bool flag3 = flag1 && flag2 & Main.settings.KeyBindSettings.EditorKeyBindSettings.Quit.Enabled &&
                             !__instance.controller.paused;

                void Invoke(MethodBase methodBase, params object[] parameters)
                {
                    methodBase.Invoke(__instance, parameters);
                }

                if (flag3)
                {
                    if (Main.R68)
                    {
                        if (GCS.customLevelPaths != null)
                        {
                            GCS.sceneToLoad = "scnCLS";
                            scrUIController.instance.WipeToBlack(WipeDirection.StartsFromLeft);
                        }
                        else
                            scrController.instance.QuitToMainMenu();
                    }
                    else
                    {
                        Invoke(_tryQuitToMenu);
                    }

                    return false;
                }

                bool flag4 = Main.settings.KeyBindSettings.CustomPlayKeyBindSettings.Restart.Down && !flag1;

                if (flag4)
                {
                    scrController.instance.Restart();
                }

                return true;
            }
        }
    }
}