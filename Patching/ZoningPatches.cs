using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using HarmonyLib;
using UnityEngine;
using Random = UnityEngine.Random;

namespace OSFEModding
{
    [HarmonyPatch]
    static class ZoningPatches
    {
        private static List<ICustomZone> customZonesImpl = new List<ICustomZone>();

        static ZoningPatches()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes().Where(y => y.IsClass && !y.IsInterface && typeof(ICustomZone).IsAssignableFrom(y)));

            foreach (Type type in types)
            {
                if (Activator.CreateInstance(type) is ICustomZone customZone)
                    customZonesImpl.Add(customZone);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(WorldBar), nameof(WorldBar.GenerateWorldBar))]
        static void GenerateWorldBarPostFix(WorldBar __instance)
        {
            foreach (ICustomZone customZone in customZonesImpl)
            {
                try
                {
                    customZone?.SetupZone(__instance);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(SpawnCtrl), nameof(SpawnCtrl.SpawnZoneC))]
        static void SpawnZoneCPostfix(SpawnCtrl __instance, ZoneType zoneType)
        {
            foreach (ICustomZone customZone in customZonesImpl)
            {
                try
                {
                    customZone?.GenerateZone(__instance, zoneType);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }
    }
}