using RoR2;
using RoR2.Orbs;
using UnityEngine;

namespace JoeMod {
    public class PseudoLightningOrb : LightningOrb {

		public ModdedLightningType lightningType;


		// Token: 0x060040C4 RID: 16580 RVA: 0x0010BF94 File Offset: 0x0010A194
		public override void Begin() {
			base.duration = 0.0001f;
			string path = null;
			switch (this.lightningType) {
				case ModdedLightningType.Ukulele:
					path = "Prefabs/Effects/OrbEffects/LightningOrbEffect";
					break;
				case ModdedLightningType.Tesla:
					path = "Prefabs/Effects/OrbEffects/TeslaOrbEffect";
					break;
				case ModdedLightningType.BFG:
					path = "Prefabs/Effects/OrbEffects/BeamSphereOrbEffect";
					break;
				case ModdedLightningType.Loader:
					path = "Prefabs/Effects/OrbEffects/LoaderLightningOrbEffect";
					break;
				case ModdedLightningType.MageLightning:
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

	public enum ModdedLightningType {
		Ukulele,
		Tesla,
		BFG,
		Loader,
		MageLightning
	}
}
