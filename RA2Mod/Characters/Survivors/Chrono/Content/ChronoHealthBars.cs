using RoR2;
using RoR2.UI;
using Slipstream;
using UnityEngine;
using UnityEngine.UI;

namespace RA2Mod.Survivors.Chrono
{
    public class ChronoHealthBars
    {
        public static void Init()
        {
            ExtraHealthbarSegment.Init();
            Slipstream.ExtraHealthbarSegment.AddType<VanishingBarData>();
        }

        public class VanishingBarData : ExtraHealthbarSegment.BarData
        {
            private int sickness;

            HealthBarStyle.BarStyle vanishingBarStyle;

            public override HealthBarStyle.BarStyle GetStyle()
            {
                HealthBarStyle.BarStyle style = vanishingBarStyle;
                style.sizeDelta = bar.style.cullBarStyle.sizeDelta;
                style.baseColor = Color.cyan;
                style.imageType = bar.style.cullBarStyle.imageType;
                style.sprite = bar.style.cullBarStyle.sprite;
                style.enabled = true;

                return style;
            }

            public override void UpdateInfo(ref HealthBar.BarInfo info, HealthComponent healthSource)
            {
                base.UpdateInfo(ref info, healthSource);

                info.enabled = sickness > 0;
                info.normalizedXMin = 0;
                info.normalizedXMax = sickness / (ChronoConfig.M4ChronoStacksToVanish.Value * 2);
            }

            public override void ApplyBar(ref HealthBar.BarInfo info, Image image, HealthComponent source, ref int i)
            {
                base.ApplyBar(ref info, image, source, ref i);
            }

            public override void CheckInventory(ref HealthBar.BarInfo info, CharacterBody body)
            {
                base.CheckInventory(ref info, body);
                sickness = body.inventory.GetItemCount(ChronoItems.chronoSicknessItemDef.itemIndex);
            }
        }
    }
}
