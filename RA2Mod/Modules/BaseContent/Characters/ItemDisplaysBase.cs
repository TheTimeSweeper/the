using RoR2;
using System.Collections.Generic;

namespace RA2Mod.Modules.Characters
{
    public abstract class ItemDisplaysBase
    {
        public void SetItemDisplays(ItemDisplayRuleSet itemDisplayRuleSet)
        {
            Modules.ItemDisplays.queuedDisplays++;
            ItemDisplays.SetItemDisplaysWhenReady(() =>
            {
                Log.CurrentTime("ITEM DISPLAY START");
                List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules = new List<ItemDisplayRuleSet.KeyAssetRuleGroup>();
                SetItemDisplayRules(itemDisplayRules);
                itemDisplayRuleSet.keyAssetRuleGroups = itemDisplayRules.ToArray();
                ItemDisplays.DisposeWhenDone();
                Log.CurrentTime("ITEM DISPLAY END");
            });

            //Modules.ItemDisplays.queuedDisplays++;

            //ItemDisplays.GetDisplaysReady();

            //SetItemDisplayRules(itemDisplayRules);

            //itemDisplayRuleSet.keyAssetRuleGroups = itemDisplayRules.ToArray();

            //ItemDisplays.DisposeWhenDone();

            //Log.CurrentTime("ITEM DISPLAY END");
        }

        protected abstract void SetItemDisplayRules(List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules);
    }
}
