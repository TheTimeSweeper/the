using EntityStates;
using System;

namespace ModdedEntityStates.Aliem {

    public class RayGun : GenericProjectileBaseState {

		public static float BaseDuration = 0.3f;
		public static float BaseDelayDuration = 0.00f;

		public static float DamageCoefficient = 1f;
		//needs to be set in the projectilecontroller component
		//public static float procCoefficient = 1f;

		public static float ProjectilePitch = 0f;
		
		public static float ThrowForce = 80f;
		public static float BoomForce = 100f;

		public override void OnEnter() {

			base.projectilePrefab = Modules.Projectiles.RayGunProjectilePrefab;
			//base.effectPrefab

			base.baseDuration = BaseDuration;
			base.baseDelayBeforeFiringProjectile = BaseDelayDuration;
			
			base.damageCoefficient = DamageCoefficient;
			base.force = ThrowForce;
			//base.projectilePitchBonus = 0;
			//min/maxSpread
			base.recoilAmplitude = 0.1f;
			base.bloom = 10;

			//targetmuzzle
			base.attackSoundString = "HenryBombThrow";

			ModifyState();

			base.OnEnter();
		}

        protected virtual void ModifyState() { }

        public override void FixedUpdate() {
			base.FixedUpdate();
		}

		public override InterruptPriority GetMinimumInterruptPriority() {
			return InterruptPriority.Skill;
		}

		public override void PlayAnimation(float duration) {

			if (base.GetModelAnimator()) {
				base.PlayAnimation("LeftArm, Override", "ShootGun", "ShootGun.playbackRate", duration);
			}
		}
	}
}