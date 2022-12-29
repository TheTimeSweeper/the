using EntityStates;
using System;
using UnityEngine;

namespace ModdedEntityStates.Joe {
    public class UtilityBaseDash : BaseSkillState {

		// Token: 0x040011B1 RID: 4529
		[SerializeField]
		public float speedCoefficient = 3f;//todo testvaluemanager
		// Token: 0x040011B0 RID: 4528
		[SerializeField]
		public float duration = 0.3f;//todo testvaluemanager

		public float _travelEndPercentTime = 1f;

		// Token: 0x040011AF RID: 4527
		protected Vector3 blinkVector = Vector3.zero;
		// Token: 0x040011AE RID: 4526
		protected float stopwatch;

		protected bool dashing;

		public override void OnEnter() {

            base.OnEnter();

            this.blinkVector = this.GetBlinkVector();

            PlayAnimation();
        }

        protected virtual void PlayAnimation() {
            PlayAnimation("Fullbody, underried", "Dash", "dash.playbackRate", duration);
        }

        // Token: 0x06000E36 RID: 3638 RVA: 0x00011909 File Offset: 0x0000FB09
        protected virtual Vector3 GetBlinkVector() {
			return ((base.inputBank.moveVector == Vector3.zero) ? base.characterDirection.forward : base.inputBank.moveVector).normalized;
		}

		public override void FixedUpdate() {

			base.FixedUpdate();

			this.stopwatch += Time.fixedDeltaTime;

			dashing = false;
			if (stopwatch < duration * _travelEndPercentTime) {
				
				DashUpdate();
				dashing = true;
			}

			if (this.stopwatch >= this.duration && base.isAuthority) {
				SetNextState();
			}
		}

        protected virtual void SetNextState() {
			base.outer.SetNextStateToMain();
        }

        protected virtual void DashUpdate() {
			if (base.characterMotor && base.characterDirection) {
				base.characterMotor.velocity = Vector3.zero;
				base.characterMotor.rootMotion += this.blinkVector * (this.moveSpeedStat * this.speedCoefficient * Time.fixedDeltaTime);
			}
		}
    }
}