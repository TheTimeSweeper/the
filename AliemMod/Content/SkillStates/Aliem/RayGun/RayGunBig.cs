using EntityStates;

namespace ModdedEntityStates.Aliem {

    public class RayGunBig : RayGun {
        
		new public static float BaseDuration = 1f;
		new public static float DamageCoefficient = 5;

        public RayGunBig() {
        }

        public RayGunBig(float dam_) {
            DamageCoefficient = dam_;
        }

        protected override void ModifyState() {

			base.projectilePrefab = Modules.Projectiles.RayGunProjectilePrefabBig;

			base.baseDuration = BaseDuration;
			base.damageCoefficient = DamageCoefficient;
		}

        public override void PlayAnimation(float duration) {
            //todo "ShootGunBig"
            base.PlayAnimation("LeftArm, Override", "ShootGun", "ShootGun.playbackRate", duration);
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.PrioritySkill;
        }
    }
}
