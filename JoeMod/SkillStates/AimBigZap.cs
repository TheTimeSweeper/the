using EntityStates;
using RoR2;

namespace HenryMod.ModdedEntityStates.Joe
{
    public class AimBigZap : AimThrowableBase //AimMortar
    {
        public static string EnterSoundString = EntityStates.Treebot.Weapon.AimMortar.enterSoundString;
        public static string ExitSoundString = EntityStates.Treebot.Weapon.AimMortar.exitSoundString;

        public override void OnEnter()
        {
            EntityStates.Toolbot.AimStunDrone goodState = new EntityStates.Toolbot.AimStunDrone();

            this.maxDistance = 48;
            this.rayRadius = 2f;
            //this.arcVisualizerPrefab = goodState.arcVisualizerPrefab;
            this.projectilePrefab = goodState.projectilePrefab;// EnforcerPlugin.EnforcerModPlugin.tearGasProjectilePrefab;
            this.endpointVisualizerPrefab = goodState.endpointVisualizerPrefab;
            this.endpointVisualizerRadiusScale = BigZap.AttackRadius;
            this.setFuse = false;
            this.damageCoefficient = 0f;
            this.baseMinimumDuration = 0.2f;
            //this.projectileBaseSpeed = 80;

            base.OnEnter();
            Util.PlaySound(EnterSoundString, base.gameObject);
            base.PlayAnimation("Gesture, Additive", "PrepBomb", "PrepBomb.playbackRate", this.minimumDuration);
        }

        // Token: 0x06003B19 RID: 15129 RVA: 0x0002B5A9 File Offset: 0x000297A9
        public override void OnExit()
        {
            base.OnExit();
            this.outer.SetNextState(new BigZap() { aimPoint = currentTrajectoryInfo.hitPoint});
            if (!this.outer.destroying)
            {
                Util.PlaySound(ExitSoundString, base.gameObject);
            }
        }

        public override void FireProjectile()
        {
            
        }

        // Token: 0x06003B1A RID: 15130 RVA: 0x000150E1 File Offset: 0x000132E1
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}