using EntityStates;
using UnityEngine;

namespace ModdedEntityStates.TeslaTrooper.Tower {
    public class TowerSell : GenericCharacterDeath {

		private float deathDuration;

		// Token: 0x06004324 RID: 17188 RVA: 0x0011196C File Offset: 0x0010FB6C
		public override void PlayDeathAnimation(float crossfadeDuration = 0.1f) {
			Animator modelAnimator = base.GetModelAnimator();
			if (modelAnimator) {
				int layerIndex = modelAnimator.GetLayerIndex("Body");
				modelAnimator.PlayInFixedTime("Death", layerIndex);
				modelAnimator.Update(0f);
				this.deathDuration = modelAnimator.GetCurrentAnimatorStateInfo(layerIndex).length;
			}
		}

		public override void FixedUpdate() {
			base.FixedUpdate();
			if (base.fixedAge > this.deathDuration) {
				EntityState.Destroy(base.gameObject);
			}
		}

		public override bool shouldAutoDestroy {
			get {
				return false;
			}
		}

		// Token: 0x06004327 RID: 17191 RVA: 0x0002C851 File Offset: 0x0002AA51
		public override void OnExit() {
			base.DestroyModel();
			base.OnExit();
		}
	}

}
