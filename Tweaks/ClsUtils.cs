using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ADOFAI;
using HarmonyLib;
using UnityEngine;

namespace AdofaiUtils.Tweaks
{
    public class ClsUtils
    {
        private static MethodBase _stopCurrentLevelSong = typeof(scnCLS).GetMethod("StopCurrentLevelSong", AccessTools.all);

        [HarmonyPatch(typeof(scnCLS), "SearchLevels")]
        private static class ScnClsSearchLevels
        {
            public static bool Prefix(scnCLS __instance, string sub, ref string ___searchParameter, List<string> ___sortedLevelKeys)
            {
                void Invoke(MethodBase methodBase, params object[] parameters)
                {
                    methodBase.Invoke(__instance, parameters);
                }
                
                ___searchParameter = sub;
                List<CustomLevelTile> source = new List<CustomLevelTile>();
                foreach (string sortedLevelKey in ___sortedLevelKeys)
                {
                    CustomLevelTile loadedLevelTile = __instance.loadedLevelTiles[sortedLevelKey];
                    LevelData loadedLevel = __instance.loadedLevels[sortedLevelKey];
                    string[] strArray =
                    {
                        RDUtils.RemoveRichTags(loadedLevel.artist),
                        RDUtils.RemoveRichTags(loadedLevel.author),
                        RDUtils.RemoveRichTags(loadedLevel.song)
                    };
                    bool flag = false;
                    if (!sub.IsNullOrEmpty())
                    {
                        foreach (string str in strArray)
                        {
                            if (str.ToLower().Contains(sub.ToLower()))
                            {
                                flag = true;
                                break;
                            }
                        }
                    }
                    else
                        flag = true;

                    if (flag)
                        source.Add(loadedLevelTile);
                    else
                        loadedLevelTile.gameObject.SetActive(false);
                }

                int num = Mathf.RoundToInt(__instance.controller.chosenplanet.transform.position.y);
                for (int index = 0; index < source.Count; ++index)
                {
                    CustomLevelTile customLevelTile = source[index];
                    customLevelTile.gameObject.SetActive(true);
                    customLevelTile.transform.MoveY(num - index);
                }

                bool flag1 = source.Count >= (double) __instance.levelCountForLoop;
                if (source.Count != 0)
                {
                    CustomLevelTile customLevelTile1 = source.First();
                    CustomLevelTile customLevelTile2 = source.Last();
                    if (flag1)
                    {
                        __instance.gemTop.MoveY(customLevelTile1.transform.position.y + 1f);
                        __instance.gemTopY = Mathf.RoundToInt(__instance.gemTop.position.y);
                        __instance.gemBottom.MoveY(customLevelTile2.transform.position.y - 1f);
                        __instance.gemBottomY = Mathf.RoundToInt(__instance.gemBottom.position.y);
                    }
                    else
                    {
                        __instance.chainTop.transform.MoveY(customLevelTile1.transform.position.y);
                        __instance.chainBottom.transform.MoveY(customLevelTile2.transform.position.y);
                    }
                }
                else
                {
                    __instance.chainTop.transform.MoveY((float) num);
                    __instance.chainBottom.transform.MoveY((float) num);
                }

                __instance.gemTop.gameObject.SetActive(flag1);
                __instance.gemBottom.gameObject.SetActive(flag1);
                __instance.chainTop.gameObject.SetActive(!flag1);
                __instance.chainBottom.gameObject.SetActive(!flag1);
                if (source.Count != 0)
                {
                    __instance.SelectLevel(source[0], true);
                }
                else
                {
                    __instance.DisplayLevel();
                    Invoke(_stopCurrentLevelSong);
                }

                string str1 = RDString.Get("cls.shortcut.find");
                if (!sub.IsNullOrEmpty())
                    str1 = str1 + " <color=#ffd000><i>" +
                           RDString.Get("cls.currentlySearching").Replace("[filter]", sub) + "</i></color>";
                __instance.currentSearchText.text = str1;
                return false;
            }
        }
    }
}