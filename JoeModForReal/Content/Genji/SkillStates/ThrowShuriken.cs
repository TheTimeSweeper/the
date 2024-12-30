using EntityStates;
using JoeModForReal.Content.Survivors;
using RoR2;
using RoR2.Projectile;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ModdedEntityStates.Genji {

	public class BaseShurikenState : BaseSkillState {

		public static float damageCoefficient => GenjiConfig.shurikenDamage.Value;// 1;

		protected void fireProjectile(Vector3 direction) {

			//Util.PlaySound("play_joe_fireShoot", base.gameObject);

			base.PlayAnimation("Arms, Override", "cast 2", "cast.playbackRate", 0.2f);

			StartAimMode();

			if (base.isAuthority) {
				Ray aimRay = base.GetAimRay();

				ProjectileManager.instance.FireProjectile(GenjiProjectiles.genjiShuriken,
					aimRay.origin,
					Util.QuaternionSafeLookRotation(direction),
					base.gameObject,
					damageCoefficient * this.damageStat,
					0f,
					base.RollCrit(),
					DamageColorIndex.Default,
					null,
					100,
					DamageTypeCombo.GenericPrimary);
			}
		}

		public override InterruptPriority GetMinimumInterruptPriority() {
			return InterruptPriority.Skill;
		}
	}

    public class ThrowShuriken : BaseShurikenState {
		
        public static float baseInterval => GenjiConfig.shurikensInterval.Value;
		public static float baseFinalInterval => GenjiConfig.shurikensFinalInterval.Value;
        public static int shurikens => GenjiConfig.shurikens.Value;

        private float interval;
		private float finalInterval;
        private int thrownShurikens;
        private float intervalTim;

        public override void OnEnter() {
            base.OnEnter();
            interval = baseInterval / attackSpeedStat;
			finalInterval = baseFinalInterval / attackSpeedStat;
		}

        public override void FixedUpdate() {
            base.FixedUpdate();

            intervalTim -= Time.fixedDeltaTime;

			Vector3 direction = GetAimRay().direction;
            while(intervalTim <= 0) {

				//throw normally until the last one
				if(thrownShurikens < shurikens - 1) {

					fireProjectile(direction);
					thrownShurikens++;
					intervalTim += interval;
				} 
				//at the last one, set the final interval
				else if(thrownShurikens == shurikens - 1) {

					fireProjectile(direction);
					thrownShurikens++;
					intervalTim += finalInterval;
				}
				//after the final interval, don't throw and end state
				else if (thrownShurikens == shurikens) {
					thrownShurikens++;
					base.outer.SetNextStateToMain();
				} 
				else if(thrownShurikens > shurikens) {
					return;
                }
            }
        }
    }


	public class ThrowShurikenAlt : BaseShurikenState {

		public static float baseDuration = 0.4f;
		private float duration;

		public override void OnEnter() {
			base.OnEnter();
			duration = baseDuration / attackSpeedStat;

			Vector3 direction = GetAimRay().direction;
			Vector3 aimCross = Vector3.Cross(direction, Vector3.up).normalized;

			fireProjectile(direction);
			fireProjectile(direction + aimCross * 0.1f);
			fireProjectile(direction - aimCross * 0.1f);
		}

		public override void FixedUpdate() {
			base.FixedUpdate();

			if (fixedAge > duration) {
				outer.SetNextStateToMain();
			}
		}
	}
}
