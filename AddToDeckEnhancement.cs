using System;
using System.Linq;
using UnityEngine;

namespace OSFEModding
{
    public class AddToDeckEnhancement : ICustomEnhancement
    {
        public Enhancement EnhancementId
        {
            get => (Enhancement) 100;
        }

        public void AddCustomEnhancement(PostCtrl postCtrl, SpellObject spellObj, Enhancement enhancement)
        {
            var newEnhancement = (Enhancement)100;
            Debug.Log($"Comparing {enhancement} to {newEnhancement}");
            if (enhancement == newEnhancement && spellObj.enhancements.All(x=>x != newEnhancement))
            {
                spellObj.nameString += U.I.Colorify(" ZZ", UIColor.Enhancement);
                spellObj.effectTags.Add(Effect.AddToDeckFront);
                var amountApp = new AmountApp();
                amountApp.initial = 1;
                amountApp.type = AmountType.Normal;
                spellObj.efApps.Add(new EffectApp(FTrigger.OnCast, 0.0f, 0.0f, Effect.AddToDeckFront, Target.Default, 0.0f, 0.0f, "ThisSpell", 1f, 0.0f, amtApp: amountApp));
                spellObj.enhancements.Add(newEnhancement);
            }
        }

        public string GetLocalization()
        {
            return "Return to deck";
        }
    }
}