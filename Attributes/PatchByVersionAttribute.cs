using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;

namespace AdofaiUtils.Attributes
{
    public static class Patch
    {
        internal static List<T> ToArray<T>(IEnumerator<T> enumerator)
        {
            var list = new List<T>();
            while (enumerator.MoveNext())
            {
                list.Add(enumerator.Current);
            }

            return list;
        }

        public static void PatchAll(Harmony harmony)
        {
            var asm = Assembly.GetExecutingAssembly();
            var R68 =
                from t in asm.GetTypes()
                let attributes = t.GetCustomAttributes(typeof(R68), true)
                where attributes != null && attributes.Length > 0
                select t;
            
            var R71 =
                from t in asm.GetTypes()
                let attributes = t.GetCustomAttributes(typeof(R71), true)
                where attributes != null && attributes.Length > 0
                select t;

            if (Main.R68)
            {
                Main.Mod.Logger.Log("ADOFAI r68 or lower version detected.");
                foreach (var i in R68)
                {
                    Main.Mod.Logger.Log("Patching: " + i.FullName);
                    harmony.CreateClassProcessor(i).Patch();
                }
            }
            else
            {
                Main.Mod.Logger.Log("ADOFAI r71 or newer version detected.");
                foreach (var i in R71)
                {
                    Main.Mod.Logger.Log("Patching: " + i.FullName);
                    harmony.CreateClassProcessor(i).Patch();
                }
            }
        }
    }
    
    [AttributeUsage(AttributeTargets.Class)]
    public class R68 : Attribute
    {
        // public static Type[] Types = { };
        //
        // public R68()
        // {
        //     Main.Mod.Logger.Log("test");
        //     Types.AddItem(GetType());
        // }
    }
    
    [AttributeUsage(AttributeTargets.Class)]
    public class R71 : Attribute
    {
        // public static Type[] Types = { };
        //
        // public R71()
        // {
        //     Types.AddItem(GetType());
        // }
    }
}