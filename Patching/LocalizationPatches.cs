using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using I2.Loc;
using UnityEngine;

namespace OSFEModding
{
    [HarmonyPatch]
    public class LocalizationPatches
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(LocalizationManager), nameof(LocalizationManager.GetTranslation))]
        public static void PostFix(string Term,ref string __result)
        {
            if (Term.StartsWith("Enhancements/"))
            {
                foreach (ICustomEnhancement customEnhancement in PostCtrlGenerateEnhancementPatches.customEnhancementsImpl)
                {
                    if (Term.EndsWith(customEnhancement.EnhancementId.ToString()))
                    {
                        __result = customEnhancement.GetLocalization();
                    }
                }
            }
        }
    }
}