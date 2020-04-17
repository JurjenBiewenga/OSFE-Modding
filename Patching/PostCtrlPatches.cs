using System;
using System.Linq;
using System.Runtime.CompilerServices;
using AssetBundles;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace OSFEModding
{
    [HarmonyPatch]
    public class PostCtrlPatches
    {
        private static Canvas canvas;
        private static float lastRolledTime = 0;
        
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

        [HarmonyPostfix]
        [HarmonyPatch(typeof(PostCtrl), nameof(PostCtrl.EndLoot))]
        static void EndLootPostFix(PostCtrl __instance)
        {
            if (canvas != null) 
                GameObject.Destroy(canvas.gameObject);
        }
        
        [HarmonyPostfix]
        [HarmonyPatch(typeof(PostCtrl), nameof(PostCtrl.Close))]
        static void ClosePostFix(PostCtrl __instance)
        {
            if (canvas != null) 
                GameObject.Destroy(canvas.gameObject);
        }
        
        [HarmonyPostfix]
        [HarmonyPatch(typeof(PostCtrl), nameof(PostCtrl.EndUpgradeOptions))]
        static void EndUpgradeOptionsPostFix(PostCtrl __instance)
        {
            if (canvas != null) 
                GameObject.Destroy(canvas.gameObject);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(PostCtrl), nameof(PostCtrl.GenerateLootOptions))]
        static void GenerateLootOptionsPostFix(PostCtrl __instance, RewardType rewardType)
        {
            Button button = Util.CreateRefreshUI(ref canvas);
            button.onClick.AddListener(() =>
            {
                if (Time.time < lastRolledTime + 1)
                    return;

                lastRolledTime = Time.time;
                
                __instance.EndLoot(rewardType, true);
                __instance.GenerateLootOptions(rewardType);
            });
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(PostCtrl), nameof(PostCtrl.StartUpgrade))]
        static void StartUpgradePostFix(PostCtrl __instance, SpellObject spellObj, int siblingIndex)
        {
            Button button = Util.CreateRefreshUI(ref canvas);
            button.onClick.AddListener(() =>
            {
                if (Time.time < lastRolledTime + 1)
                    return;

                lastRolledTime = Time.time;
                __instance.StartUpgrade(spellObj, siblingIndex);
            });
        }
        
        [HarmonyPostfix]
        [HarmonyPatch(typeof(PostCtrl), nameof(PostCtrl.StartLevelUpOptions))]
        static void StartLevelUpOptionsPostFix(PostCtrl __instance)
        {
            Button button = Util.CreateRefreshUI(ref canvas);
            button.onClick.AddListener(() =>
            {
                if (Time.time < lastRolledTime + 1)
                    return;

                lastRolledTime = Time.time;

                var method = AccessTools.Method(typeof(PostCtrl), "ClearAndHideCards");
                method.Invoke(__instance, null);
                var field = AccessTools.Field(typeof(PostCtrl), "levelsGained");
                field.SetValue(__instance, (int)field.GetValue(__instance)+1);
                __instance.StartLevelUpOptions();
            });
        }

    }

    public static class Util
    {
        public static Button CreateRefreshUI(ref Canvas canvas)
        {
            if (canvas != null)
                GameObject.Destroy(canvas.gameObject);

            Debug.Log("Creating canvas");
            var canvasGo = new GameObject("Canvas");
            canvas = canvasGo.AddComponent<Canvas>();
            canvas.sortingOrder = int.MaxValue;
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasGo.AddComponent<GraphicRaycaster>();

            var buttonGo = new GameObject("button").AddComponent<RectTransform>();
            buttonGo.transform.SetParent(canvasGo.transform);
            buttonGo.transform.localPosition = new Vector2(0, Screen.height / 2 - 50);
            buttonGo.transform.localScale = Vector3.one;
            buttonGo.sizeDelta = new Vector2(200, 100);
            buttonGo.gameObject.AddComponent<Image>().sprite = AssetBundleManager.LoadAsset<Sprite>("sprites_ui", "TransparentGradientBrandBG", out string err);
            var textGo = new GameObject("text");
            var text = textGo.AddComponent<Text>();
            text.text = "reroll";
            text.transform.SetParent(buttonGo.transform);
            text.transform.localPosition = Vector3.zero;
            text.transform.localScale = Vector3.one;
            text.font = (Font) Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
            text.color = Color.black;
            text.fontSize = 32;
            text.alignment = TextAnchor.MiddleCenter;

            var button = buttonGo.gameObject.AddComponent<Button>();
            return button;
        }
    }
}