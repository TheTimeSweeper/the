using RoR2;
using RoR2.Orbs;
using UnityEngine;

namespace TeslaTrooper {
    public class PseudoLightningOrb : LightningOrb {

		public ModdedLightningType moddedLightningType;

		// Token: 0x060040C4 RID: 16580 RVA: 0x0010BF94 File Offset: 0x0010A194
		public override void Begin() {

			if(speed <= 0) {
				base.duration = 0.0001f;
			} else {
				base.duration = base.distanceToTarget / this.speed;
            }

			GameObject effect = null;
			switch (this.moddedLightningType) {
				case ModdedLightningType.Ukulele:
					effect = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/OrbEffects/LightningOrbEffect");
					break;
				case ModdedLightningType.Tesla:
					effect = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/OrbEffects/TeslaOrbEffect");
					break;
				case ModdedLightningType.BFG:
					effect = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/OrbEffects/BeamSphereOrbEffect");
					break;
				case ModdedLightningType.Loader:
					effect = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/OrbEffects/LoaderLightningOrbEffect");
					break;
				case ModdedLightningType.MageLightning:
					effect = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/OrbEffects/MageLightningOrbEffect");
					break;
				case ModdedLightningType.Nod:
					effect = Modules.Assets.TeslaLightningOrbEffectRed;
					break;
				case ModdedLightningType.NodMage:
					effect = Modules.Assets.TeslaMageLightningOrbEffectRed;
					break;
				case ModdedLightningType.NodMageThick:
					effect = Modules.Assets.TeslaMageLightningOrbEffectRedThick;
					break;

			}
			EffectData effectData = new EffectData {
				origin = this.origin,
				genericFloat = base.duration
			};
			effectData.SetHurtBoxReference(this.target);
			EffectManager.SpawnEffect(effect, effectData, true);
		}
	}

	public enum ModdedLightningType {
		Ukulele,
		Tesla,
		BFG,
		Loader,
		MageLightning,
		Nod,
		NodMage,
        NodMageThick
    }
}
