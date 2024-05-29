using AliemMod.Content;
using EntityStates;
using Modules;
using System;
using UnityEngine;

namespace ModdedEntityStates.Aliem
{
    public class RayGunFire : GenericProjectileBaseState, IOffHandable {

		public virtual float BaseDuration => 0.3f;
		public virtual float BaseDelayDuration => 0.00f;

		public static float RayGunDamageCoefficient => AliemConfig.M1_RayGun_Damage.Value;
        public virtual float BaseDamageCoefficient => RayGunDamageCoefficient;
        public virtual string muzzleString => isOffHanded ? "BlasterMuzzle.R" : "BlasterMuzzle";

        //needs to be set in the projectilecontroller component
        //public static float procCoefficient = 1f;

        public static float ProjectilePitch = 0f;
		
		public static float ProjectileForce = 80f;

        public virtual GameObject projectile => Modules.Projectiles.RayGunProjectilePrefab;

        public virtual string soundString => AliemConfig.M1_RayGun_Sound_Alt.Value? "Play_INV_RayGun" : "Play_RayGun";

        public bool isOffHanded { get; set; }

        public override void OnEnter() {
			base.projectilePrefab = projectile;
			//base.effectPrefab

			base.baseDuration = BaseDuration;
			base.baseDelayBeforeFiringProjectile = BaseDelayDuration;

            base.targetMuzzle = muzzleString;
            base.effectPrefab = EntityStates.Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab;
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
                base.PlayAnimation(isOffHanded ? "RightArm, Over" : "LeftArm, Over", "ShootGun");
                base.PlayAnimation(isOffHanded ? "LeftArm, Under" : "RightArm, Under", "ShootGun");
            }
		}
	}
}