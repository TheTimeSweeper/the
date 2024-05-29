using AliemMod.Content;
using UnityEngine;

namespace ModdedEntityStates.Aliem
{
    public class SwordFire : RayGunFire
    {
        public override float BaseDuration => 0.2f;
        public override float BaseDamageCoefficient => AliemConfig.M1_Sword_Damage.Value;
        public override GameObject projectile => Modules.Projectiles.SwordProjectilePrefab;
        public override string soundString => "Play_AliemEnergySword";

        public override void OnEnter()
        {
            base.OnEnter();

            if (!base.isGrounded)
            {
                SmallHop(characterMotor, AliemConfig.smallhop.Value / Mathf.Sqrt(this.attackSpeedStat));
            }
        }

        public override Ray ModifyProjectileAimRay(Ray aimRay)
        {
            Ray ray = base.ModifyProjectileAimRay(aimRay);
            ray.origin = transform.position;
            return ray;
        }
    }
}