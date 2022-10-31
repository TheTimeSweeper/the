using EntityStates;

namespace ModdedEntityStates.Aliem {

    public class RayGunBig : RayGun {
        
		new public static float BaseDuration = 0.5f;
        
        public static float BaseDamage = 6;
		new public float DamageCoefficient = BaseDamage;
        
        private string _shootSound = "Play_RayGunBigClassic";

        public RayGunBig() {
        }

        public RayGunBig(float dam_, string shootSound_) {
            DamageCoefficient = dam_;
            _shootSound = shootSound_;
        }

        protected override void ModifyState() {

			base.projectilePrefab = Modules.Projectiles.RayGunProjectilePrefabBig;

			base.baseDuration = BaseDuration;
			base.damageCoefficient = DamageCoefficient;
            base.attackSoundString = _shootSound;
		}

        public override void PlayAnimation(float duration) {
            base.PlayAnimation("Gesture, Override", "ShootGunBig", "ShootGun.playbackRate", duration);
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.PrioritySkill;
        }
    }
}
