using RoR2;
using RoR2.Orbs;
using UnityEngine;

namespace JoeMod {
    public class PseudoLightningOrb : LightningOrb {

		// Token: 0x060040C4 RID: 16580 RVA: 0x0010BF94 File Offset: 0x0010A194
		public override void Begin() {
			base.duration = 0.0001f;
			string path = null;
			switch (this.lightningType) {
				case LightningOrb.LightningType.Ukulele:
					path = "Prefabs/Effects/OrbEffects/LightningOrbEffect";
					break;
				case LightningOrb.LightningType.Tesla:
					path = "Prefabs/Effects/OrbEffects/TeslaOrbEffect";
					break;
				case LightningOrb.LightningType.BFG:
					path = "Prefabs/Effects/OrbEffects/BeamSphereOrbEffect";
					break;
				case LightningOrb.LightningType.Loader:
					path = "Prefabs/Effects/OrbEffects/LoaderLightningOrbEffect";
					break;
				case LightningOrb.LightningType.MageLightning:
					path = "Prefabs/Effects/OrbEffects/MageLightningOrbEffect";
					break;
			}
			EffectData effectData = new EffectData {
				origin = this.origin,
				genericFloat = base.duration
			};
			effectData.SetHurtBoxReference(this.target);
			EffectManager.SpawnEffect(LegacyResourcesAPI.Load<GameObject>(path), effectData, true);
		}
	}
}
