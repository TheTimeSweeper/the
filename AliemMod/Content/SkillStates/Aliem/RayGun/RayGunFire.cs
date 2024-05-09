using EntityStates;
using System;
using UnityEngine;

namespace ModdedEntityStates.Aliem
{
    public class RayGunFire : GenericProjectileBaseState {

		public virtual float BaseDuration => 0.3f;
		public virtual float BaseDelayDuration => 0.00f;

		public static float BaseDamageCoefficient = 2.0f;
		//needs to be set in the projectilecontroller component
		//public static float procCoefficient = 1f;

		public static float ProjectilePitch = 0f;
		
		public static float ProjectileForce = 80f;

        public virtual GameObject projectile => Modules.Projectiles.RayGunProjectilePrefab;

        public virtual string soundString => "Play_RayGun";

        public override void OnEnter() {
			base.projectilePrefab = projectile;
			//base.effectPrefab

			base.baseDuration = BaseDuration;
			base.baseDelayBeforeFiringProjectile = BaseDelayDuration;
			
			base.damageCoefficient = BaseDamageCoefficient;
			base.force = ProjectileForce;
			//base.projectilePitchBonus = 0;
			//min/maxSpread
			base.recoilAmplitude = 0.1f;
			base.bloom = 10;

			//targetmuzzle
			base.attackSoundString = soundString;

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