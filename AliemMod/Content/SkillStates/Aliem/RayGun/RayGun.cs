using EntityStates;
using System;

namespace ModdedEntityStates.Aliem {

    public class RayGun : GenericProjectileBaseState {

		public static float BaseDuration = 0.3f;
		public static float BaseDelayDuration = 0.00f;

		public static float DamageCoefficient = 2.2f;
		//needs to be set in the projectilecontroller component
		//public static float procCoefficient = 1f;

		public static float ProjectilePitch = 0f;
		
		public static float ProjectileForce = 80f;

		public override void OnEnter() {

			base.projectilePrefab = Modules.Projectiles.RayGunProjectilePrefab;
			//base.effectPrefab

			base.baseDuration = BaseDuration;
			base.baseDelayBeforeFiringProjectile = BaseDelayDuration;
			
			base.damageCoefficient = DamageCoefficient;
			base.force = ProjectileForce;
			//base.projectilePitchBonus = 0;
			//min/maxSpread
			base.recoilAmplitude = 0.1f;
			base.bloom = 10;

			//targetmuzzle
			base.attackSoundString = "Play_RayGun";

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
				base.PlayAnimation("Gesture, Override", "ShootGun", "ShootGun.playbackRate", duration);
			}
		}
	}
}