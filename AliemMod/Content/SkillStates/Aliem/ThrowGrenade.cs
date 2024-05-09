using EntityStates;
using System;
using UnityEngine;

namespace ModdedEntityStates.Aliem {

    public class ThrowGrenade : GenericProjectileBaseState {

		public static float BaseDuration = 0.3f;
		public static float BaseDelayDuration = 0.00f;
		
		public static float DamageCoefficient = 12f;
		//needs to be set in the projectilecontroller component
		//public static float procCoefficient = 1f;

		public static float ProjectilePitch = -70;
		
		public static float ProjectileForce = 80f;

		public override void OnEnter() {

			base.projectilePrefab = Modules.Projectiles.GrenadeProjectile;
			//projectilePrefab.GetComponent<RoR2.Projectile.ProjectileSimple>().desiredForwardSpeed = TestValueManager.value2;

			//base.effectPrefab

			base.baseDuration = BaseDuration;
			base.baseDelayBeforeFiringProjectile = BaseDelayDuration;
			
			base.damageCoefficient = DamageCoefficient;
			base.force = ProjectileForce;
			base.projectilePitchBonus = ProjectilePitch;
			//min/maxSpread
			base.recoilAmplitude = 0.1f;
			base.bloom = 0;

			//targetmuzzle
			base.attackSoundString = "Play_ThrowBomb";

			ModifyState();
			
			base.OnEnter();
		}

        protected virtual void ModifyState() { }

        public override Ray ModifyProjectileAimRay(Ray aimRay) {

			Vector3 aimCross = Vector3.Cross(aimRay.direction, Vector3.up).normalized;
			Vector3 aimForwardPerpendicular = Vector3.Cross(Vector3.up, aimCross).normalized;
			aimRay.direction = aimForwardPerpendicular;
			return aimRay;
		}

        public override void FixedUpdate() {
			base.FixedUpdate();
		}
		
		public override InterruptPriority GetMinimumInterruptPriority() {
			return InterruptPriority.Skill;
		}

		public override void PlayAnimation(float duration) {

			if (base.GetModelAnimator()) {
				base.PlayAnimation("Gesture, Additive", "ThrowGrenade", "ThrowBomb.playbackRate", duration);
			}
		}
	}
}