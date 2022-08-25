using RoR2;
using RoR2.Orbs;
using UnityEngine;

namespace JoeMod {

    public class HarmlessBuffOrb : Orb{

        public BuffDef buffToApply;
		public ModdedLightningType moddedLightningType = ModdedLightningType.Ukulele;
        public float speed = -1;

        public override void Begin() {
            base.Begin();

            if (speed <= 0) {
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
			effectData.SetHurtBoxReference(base.target);

			EffectManager.SpawnEffect(effect, effectData, true);
		}

        public override void OnArrival() {
            if (target && buffToApply) {
                target.healthComponent.body.AddBuff(buffToApply);
            }
        }
    }
}
