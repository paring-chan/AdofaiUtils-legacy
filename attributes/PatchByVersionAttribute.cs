using System;
using HarmonyLib;
using UnityEngine;

namespace AdofaiUtils.attributes
{
    public static class Patch
    {
        public static Type[] R68Types = { };
        public static Type[] R71Types = { };
        
        public static void PatchAll(Harmony harmony)
        {
            if (typeof(scrMisc).Assembly.GetType("ADOStartup") == null)
            {
                foreach (var i in R68Types)
                {
                    harmony.CreateClassProcessor(i).Patch();
                }
            }
            else
            {
                foreach (var i in R71Types)
                {
                    harmony.CreateClassProcessor(i).Patch();
                }
            }
        }
    }
    
    [AttributeUsage(AttributeTargets.Class)]
    public class R68 : Attribute
    {
        public R68()
        {
            Patch.R68Types.AddItem(GetType());
        }
    }
    
    [AttributeUsage(AttributeTargets.Class)]
    public class R71 : Attribute
    {
        public R71()
        {
            Patch.R71Types.AddItem(GetType());
        }
    }
}