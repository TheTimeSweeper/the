using EntityStates;

namespace RA2Mod.Survivors.GI.SkillStates
{
    public class FireMissile : GenericProjectileBaseState
    {
        public static float BaseDuration => GIConfig.M1_Missile_Duration.Value;

        public static float DamageCoefficient => GIConfig.M1_Missile_Damage.Value;

        public override void OnEnter()
        {
            projectilePrefab = GIAssets.missilePrefab;
            //base.effectPrefab = Modules.Assets.SomeMuzzleEffect;
            //targetmuzzle = "muzzleThrow"

            attackSoundString = "Play_GuardienGIMissile";

            baseDuration = BaseDuration;
            baseDelayBeforeFiringProjectile = 0;

            damageCoefficient = DamageCoefficient;
            //proc coefficient is set on the components of the projectile prefab
            force = 80f;

            //base.projectilePitchBonus = 0;
            //base.minSpread = 0;
            //base.maxSpread = 0;

            recoilAmplitude = 0.1f;
            bloom = 10;

            base.OnEnter();
        }
    }
}