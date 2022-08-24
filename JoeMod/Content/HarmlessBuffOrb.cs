using RoR2;
using RoR2.Orbs;
using UnityEngine;

namespace JoeMod {

    public class HarmlessBuffOrb : Orb{

        public BuffDef buffToApply;
        public float speed = -1;

        public override void Begin() {
            base.Begin();

            if (speed <= 0) {
                base.duration = 0.0001f;
            } else {
                base.duration = base.distanceToTarget / this.speed;
            }
            EffectData effectData = new EffectData {
				origin = this.origin,
				genericFloat = base.duration
			};
			effectData.SetHurtBoxReference(base.target);					//todo: custom effect
			EffectManager.SpawnEffect(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/OrbEffects/LightningOrbEffect"), effectData, true);
		}

        public override void OnArrival() {
            if (target && buffToApply) {
                target.healthComponent.body.AddBuff(buffToApply);
            }
        }
    }
}
