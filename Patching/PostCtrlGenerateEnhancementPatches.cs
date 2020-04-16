using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;

namespace OSFEModding
{
    [HarmonyPatch(typeof(PostCtrl), "GenerateEnhancement")]
    public class PostCtrlGenerateEnhancementPatches
    {
        public static List<ICustomEnhancement> customEnhancementsImpl = new List<ICustomEnhancement>();

        static PostCtrlGenerateEnhancementPatches()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes().Where(y => y.IsClass && !y.IsInterface && typeof(ICustomEnhancement).IsAssignableFrom(y)));

            foreach (Type type in types)
            {
                if (Activator.CreateInstance(type) is ICustomEnhancement customZone)
                    customEnhancementsImpl.Add(customZone);
            }
        }
        
        private static MethodInfo m_MyExtraMethod = typeof(PostCtrlGenerateEnhancementPatches).GetMethod(nameof(AddExtraEnhancements));

        public static void AddExtraEnhancements(PostCtrl thisObj, SpellObject spellObject, List<Enhancement> enhancements)
        {
            enhancements.AddRange(customEnhancementsImpl.Select(x=>x.EnhancementId));
        }
        
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var found = false;
            foreach (var instruction in instructions)
            {
                if (instruction.opcode == OpCodes.Ldarg_0 && !found)
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Ldarg_1);
                    yield return new CodeInstruction(OpCodes.Ldloc_0);
                    yield return new CodeInstruction(OpCodes.Call, m_MyExtraMethod);
                    found = true;
                }
                yield return instruction;
            }
            if (found == false)
                Debug.LogError("Cannot find <Ldarg_0> in PostCtrl.GenerateEnhancement");
        }
    }
}