using RoR2;
using RoR2.Skills;
using UnityEngine;
//stolen from ItemTakenOrbEffect
[RequireComponent(typeof(EffectComponent))]
public class SkillTakenOrbEffect : MonoBehaviour {

	// ror2 modding has really stunted my growth as a developer hasn't it
	private void Start() {

        EffectData effectData = GetComponent<EffectComponent>().effectData;

        SkillDef skilldef = effectData.rootObject.GetComponent<SkillLocator>().GetSkillAtIndex((int)effectData.genericUInt).skillDef;

        //ItemDef itemDef = catalog.GetItemDef((ItemIndex)Util.UintToIntMinusOne(base.GetComponent<EffectComponent>().effectData.genericUInt));
		ColorCatalog.ColorIndex colorIndex = ColorCatalog.ColorIndex.Error;
		Sprite sprite = null;
		if (skilldef != null) {
			//colorIndex = itemDef.colorIndex;
			sprite = skilldef.icon;
		}
		Color color = Color.white;// ColorCatalog.GetColor(colorIndex);
		this.trailToColor.startColor = this.trailToColor.startColor * color;
		this.trailToColor.endColor = this.trailToColor.endColor * color;
		for (int i = 0; i < this.particlesToColor.Length; i++) {
			ParticleSystem particleSystem = this.particlesToColor[i];
			//particleSystem.main.startColor = color;
			particleSystem.Play();
		}
		for (int j = 0; j < this.spritesToColor.Length; j++) {
			this.spritesToColor[j].color = color;
		}
		this.iconSpriteRenderer.sprite = sprite;
	}

	// Token: 0x04003F18 RID: 16152
	public TrailRenderer trailToColor;

	// Token: 0x04003F19 RID: 16153
	public ParticleSystem[] particlesToColor;

	// Token: 0x04003F1A RID: 16154
	public SpriteRenderer[] spritesToColor;

	// Token: 0x04003F1B RID: 16155
	public SpriteRenderer iconSpriteRenderer;
}
