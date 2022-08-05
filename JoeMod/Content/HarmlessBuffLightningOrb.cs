using RoR2;
using RoR2.Orbs;
using UnityEngine;

namespace Content {

    public class HarmlessBuffOrb : Orb{

        public BuffDef buffToApply;

        public override void Begin() {
            base.Begin();

            base.duration = 0.05f;

			EffectData effectData = new EffectData {
				origin = this.origin,
				genericFloat = base.duration
			};
			effectData.SetHurtBoxReference(base.target);					//todo: custom effect
			EffectManager.SpawnEffect(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/OrbEffects/LightningOrbEffect"), effectData, true);
		}

        public override void OnArrival() {
                if (target) {
                    target.healthComponent.body.AddBuff(buffToApply);
                }
            }
        }
    }
}
