using EntityStates;
using UnityEngine;

namespace ModdedEntityStates.Aliem
{
    public class SwordInputs : MashAndHoldInputs
    {
        protected override EntityState initialMashState => new SwordFire();

        protected override EntityState mashState => new SwordFire();

        protected override EntityState holdState => new SwordCharging();
    }

    public class SwordFire : RayGunFire
    {
        public override GameObject projectile => Modules.Projectiles.SwordProjectilePrefab;
        public override string soundString => "Play_AliemEnergySword";
        public override float BaseDuration => 0.2f;

        public override Ray ModifyProjectileAimRay(Ray aimRay)
        {
            Ray ray = base.ModifyProjectileAimRay(aimRay);
            ray.origin = transform.position;
            return ray;
        }
    }
}