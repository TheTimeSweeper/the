using AliemMod.Content;
using AliemMod.Modules;
using UnityEngine;

namespace ModdedEntityStates.Aliem
{
    public class SwordFire : RayGunFire
    {
        public override float BaseDuration => AliemConfig.M1_Sword_Duration.Value;
        public override float BaseDamageCoefficient => AliemConfig.M1_Sword_Damage.Value;
        public override GameObject projectile => Projectiles.SwordProjectilePrefab;
        public override string soundString => "Play_AliemEnergySword";
        
        public override Ray ModifyProjectileAimRay(Ray aimRay)
        {
            Ray ray = base.ModifyProjectileAimRay(aimRay);
            ray.origin = transform.position;
            return ray;
        }
    }
}