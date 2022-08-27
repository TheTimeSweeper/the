using RoR2;
using RoR2.Orbs;
using System;
using UnityEngine;

namespace JoeMod {

	public class VisualOrbEffect : OrbEffect {

		// Token: 0x060040EB RID: 16619 RVA: 0x0010CA38 File Offset: 0x0010AC38
		new private void Start() {
			EffectComponent effectComponent = base.GetComponent<EffectComponent>();
			this.startPosition = effectComponent.effectData.origin;
			this.previousPosition = this.startPosition;
			//GameObject gameObject = component.effectData.ResolveHurtBoxReference();

			this.targetTransform = CreateDummyTransformAtPoint(effectComponent.effectData.start);
			this.duration = effectComponent.effectData.genericFloat;

			if (this.duration == 0f) {
				//Debug.LogFormat("Zero duration for effect \"{0}\"", new object[]
				//{
				//	base.gameObject.name
				//});
				//UnityEngine.Object.Destroy(base.gameObject);
				return;
			}

			this.lastKnownTargetPosition = (this.targetTransform ? this.targetTransform.position : this.startPosition);
			if (this.startEffect) {
				EffectData effectData = new EffectData {
					origin = base.transform.position,
					scale = this.startEffectScale
				};
				if (this.startEffectCopiesRotation) {
					effectData.rotation = base.transform.rotation;
				}
				EffectManager.SpawnEffect(this.startEffect, effectData, false);
			}
			this.startVelocity.x = Mathf.Lerp(this.startVelocity1.x, this.startVelocity2.x, UnityEngine.Random.value);
			this.startVelocity.y = Mathf.Lerp(this.startVelocity1.y, this.startVelocity2.y, UnityEngine.Random.value);
			this.startVelocity.z = Mathf.Lerp(this.startVelocity1.z, this.startVelocity2.z, UnityEngine.Random.value);
			this.endVelocity.x = Mathf.Lerp(this.endVelocity1.x, this.endVelocity2.x, UnityEngine.Random.value);
			this.endVelocity.y = Mathf.Lerp(this.endVelocity1.y, this.endVelocity2.y, UnityEngine.Random.value);
			this.endVelocity.z = Mathf.Lerp(this.endVelocity1.z, this.endVelocity2.z, UnityEngine.Random.value);
			this.UpdateOrb(0f);
		}

        private Transform CreateDummyTransformAtPoint(Vector3 start) {
            Transform dummyTransform = new GameObject("orbDummyTarget").transform;
			dummyTransform.position = start;
			return dummyTransform;
        }

		void OnDestroy() {
			UnityEngine.Object.Destroy(targetTransform);
        }
    }

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
