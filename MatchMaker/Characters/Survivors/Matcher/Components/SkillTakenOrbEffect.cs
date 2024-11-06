using MatcherMod.Survivors.Matcher.MatcherContent;
using RoR2;
using RoR2.Skills;
using System.Collections;
using UnityEngine;

namespace MatcherMod.Survivors.Matcher.Components
{
    //stolen from ItemTakenOrbEffect
    [RequireComponent(typeof(EffectComponent))]
    public class SkillTakenOrbEffect : MonoBehaviour
    {
        // ror2 modding has really stunted my growth as a developer hasn't it
        // Token: 0x060058AA RID: 22698 RVA: 0x0003DAA5 File Offset: 0x0003BCA5
        private void OnEnable()
        {
            base.StartCoroutine(this.DelayedUpdateSprite());
        }

        // Token: 0x060058AB RID: 22699 RVA: 0x0003DAB4 File Offset: 0x0003BCB4
        private IEnumerator DelayedUpdateSprite()
        {
            yield return null;

            EffectData effectData = GetComponent<EffectComponent>().effectData;
            
            SkillDef skilldef = SkillCatalog.GetSkillDef(Util.UintToIntMinusOne(effectData.genericUInt)); //effectData.rootObject.GetComponent<SkillLocator>().GetSkillAtIndex((int)effectData.genericUInt).skillDef;

            //ItemDef itemDef = catalog.GetItemDef((ItemIndex)Util.UintToIntMinusOne(base.GetComponent<EffectComponent>().effectData.genericUInt));
            //ColorCatalog.ColorIndex colorIndex = ColorCatalog.ColorIndex.Error;
            Sprite sprite = null;
            if (skilldef != null)
            {
                //colorIndex = itemDef.colorIndex;
                sprite = skilldef.icon;
                //scaleSpriteComponent.scaleFactor = (Config.nipper.Value * sprite.pixelsPerUnit) / sprite.texture.width;
                this.iconSpriteRenderer.sprite = sprite;
                Matchmaker.Util.NormalizeSpriteScale(iconSpriteRenderer, Config.nipper.Value);
            }

            Color color = Color.white;// ColorCatalog.GetColor(colorIndex);
            this.trailToColor.startColor = this.trailToColor.startColor * color;
            this.trailToColor.endColor = this.trailToColor.endColor * color;
            for (int i = 0; i < this.particlesToColor.Length; i++)
            {
                ParticleSystem particleSystem = this.particlesToColor[i];
                //particleSystem.main.startColor = color;
                particleSystem.Play();
            }
            for (int j = 0; j < this.spritesToColor.Length; j++)
            {
                this.spritesToColor[j].color = color;
            }
        }

        // Token: 0x04003F18 RID: 16152
        public TrailRenderer trailToColor;

        // Token: 0x04003F19 RID: 16153
        public ParticleSystem[] particlesToColor;

        // Token: 0x04003F1A RID: 16154
        public SpriteRenderer[] spritesToColor;

        // Token: 0x04003F1B RID: 16155
        public SpriteRenderer iconSpriteRenderer;

        [SerializeField]
        public ScaleSpriteByCamDistance scaleSpriteComponent;
    }
}