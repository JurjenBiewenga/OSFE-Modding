namespace OSFEModding
{
    public interface ICustomEnhancement
    {
        Enhancement EnhancementId { get; }

        void AddCustomEnhancement(PostCtrl __instance, SpellObject spellObj, Enhancement enhancement);
        string GetLocalization();
    }
}