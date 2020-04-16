using System;
using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace OSFEModding
{
    [HarmonyPatch]
    public class PostCtrlPatches
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(PostCtrl), nameof(PostCtrl.EnhanceSpell))]
        static void GenerateEnhanceSpellPostFix(PostCtrl __instance, SpellObject spellObj, Enhancement enhancement)
        {
            foreach (ICustomEnhancement customZone in PostCtrlGenerateEnhancementPatches.customEnhancementsImpl)
            {
                try
                {
                    customZone?.AddCustomEnhancement(__instance, spellObj, enhancement);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }
    }
}