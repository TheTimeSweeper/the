using RoR2;
using RoR2.Orbs;
using UnityEngine;

namespace TeslaTrooper {
    public class VisualOrbEffect : OrbEffect {

		// Token: 0x060040EB RID: 16619 RVA: 0x0010CA38 File Offset: 0x0010AC38
		new private void Start() {
			EffectComponent effectComponent = base.GetComponent<EffectComponent>();
			this.startPosition = effectComponent.effectData.origin;
			this.previousPosition = this.startPosition;
			//GameObject gameObject = component.effectData.ResolveHurtBoxReference();

			this.duration = effectComponent.effectData.genericFloat;

			if (this.duration == 0f) {
                Debug.LogFormat("Zero duration for effect \"{0}\"", new object[]
                {
                    base.gameObject.name
                });
                UnityEngine.Object.Destroy(base.gameObject);
                return;
			}

			this.lastKnownTargetPosition = effectComponent.effectData.start;
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
    }
}
