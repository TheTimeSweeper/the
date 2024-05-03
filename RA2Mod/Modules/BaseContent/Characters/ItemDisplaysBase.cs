using RoR2;
using System.Collections.Generic;

namespace RA2Mod.Modules.Characters
{
    public abstract class ItemDisplaysBase
    {
        public void SetItemDisplays(ItemDisplayRuleSet itemDisplayRuleSet)
        {
            ItemDisplays.SetItemDisplaysWhenReady(() =>
            {
                List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules = new List<ItemDisplayRuleSet.KeyAssetRuleGroup>();
                SetItemDisplayRules(itemDisplayRules);
                itemDisplayRuleSet.keyAssetRuleGroups = itemDisplayRules.ToArray();
            });
        }

        protected abstract void SetItemDisplayRules(List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules);
    }
}
