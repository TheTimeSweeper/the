using EntityStates;
using ModdedEntityStates.BaseStates;
using Modules;
using R2API;
using RoR2;
using UnityEngine;

namespace ModdedEntityStates.TeslaTrooper {
    
    public class BigZap : BaseTimedSkillState {
        public static GameObject bigZapEffectPrefab = Assets.LoadAsset<GameObject>("prefabs/effects/magelightningbombexplosion");
        public static GameObject bigZapEffectPrefabArea = Assets.LoadAsset<GameObject>("prefabs/effects/lightningstakenova");
        public static GameObject bigZapEffectFlashPrefab = Assets.LoadAsset<GameObject>("prefabs/effects/omnieffect/omniimpactvfxlightning");

        public static float DamageCoefficient = 5.0f;
        public static float ProcCoefficient = 1f;
        public static float BaseAttackRadius = 10;
        public static float BaseDuration = 1;
        public static float BaseCastTime = 0;//0.2f //todo windup sound, windup vfx, and zapping ground

        public float skillsPlusAreaMulti = 1f;
        public float skillsPlusDamageMulti = 1f;
        public float attackRadius;
        public Vector3 aimPoint;

        private string zapSound = "Play_tank_vtesatta_tesla_tank_attack";
        private string zapSoundCrit = "Play_tank_vtesattb_tesla_tank_attack";

        private bool commandedTowers;

        public override void OnEnter() {
            base.OnEnter();
            
            InitDurationValues(BaseDuration, BaseCastTime);
            
            TeslaTowerControllerController controller = GetComponent<TeslaTowerControllerController>();

            if (base.isAuthority && controller && controller.coilReady) {
                TeslaTrackerComponent tracker = GetComponent<TeslaTrackerComponent>();
                if (tracker && tracker.GetTrackingTarget()) {

                    controller.commandTowers(tracker.GetTrackingTarget());

                    commandedTowers = true;
                }
            }
            attackRadius = BaseAttackRadius * skillsPlusAreaMulti;
            
            PlayAnimation("Gesture, Additive", "Shock", "Shock.playbackRate", 0.3f);

            base.characterBody.AddSpreadBloom(1);
        }

        protected override void OnCastEnter() {
            base.OnCastEnter();

            if (commandedTowers)
                return;

            bool isCrit = RollCrit();

            if (base.isAuthority) {

                BlastAttack blast = new BlastAttack {
                    attacker = gameObject,
                    inflictor = gameObject,
                    teamIndex = teamComponent.teamIndex,
                    //attackerFiltering = AttackerFiltering.NeverHit

                    position = aimPoint,
                    radius = attackRadius,
                    falloffModel = BlastAttack.FalloffModel.None,

                    baseDamage = damageStat * DamageCoefficient * skillsPlusDamageMulti,
                    crit = isCrit,
                    damageType = DamageType.Stun1s,
                    //damageColorIndex = DamageColorIndex.Default,

                    procCoefficient = 1,
                    //procChainMask = 
                    //losType = BlastAttack.LoSType.NearestHit,

                    baseForce = -5, //enfucker void grenade here we go
                                    //bonusForce = ;

                    //impactEffect = EffectIndex.uh;
                };

                if (FacelessJoePlugin.conductiveMechanic && FacelessJoePlugin.conductiveEnemy) {
                    blast.AddModdedDamageType(DamageTypes.consumeConductive);
                }
                blast.Fire();
            }

            Util.PlaySound(isCrit ? zapSound : zapSoundCrit, gameObject);
            
            #region effects
            EffectData fect = new EffectData {
                origin = aimPoint,
                scale = attackRadius,
            };

            if (Input.GetKey(KeyCode.LeftAlt)) {
                tryEffects(fect);
                return;
            }
            
            EffectManager.SpawnEffect(bigZapEffectPrefabArea, fect, true);

            if (!Input.GetKey(KeyCode.G))
                EffectManager.SpawnEffect(bigZapEffectPrefab, fect, true);

            if (Input.GetKey(KeyCode.H)) {
                fect.scale /= 2f;
                EffectManager.SpawnEffect(bigZapEffectFlashPrefab, fect, true);
            }
            #endregion effects
        }

        #region testGameEffects
        public static float keep_bigsexyeffect = 0;

        public static GameObject[] effects =
            {
                Assets.LoadAsset<GameObject>("prefabs/effects/magelightningbombexplosion"), //oke
                Assets.LoadAsset<GameObject>("prefabs/effects/lightningstakenova"), //decent, simple, probably best but boring

                Assets.LoadAsset<GameObject>("prefabs/effects/impacteffects/impactlightning"), //2 //teeny         //gauntlet? too blue, not cyan, mabye that's fine
                Assets.LoadAsset<GameObject>("prefabs/effects/impacteffects/lightningflash"),// teeny              //looks like purity lol
                Assets.LoadAsset<GameObject>("prefabs/effects/impacteffects/lightningstrikeimpact"), //NOT TEENY
                Assets.LoadAsset<GameObject>("prefabs/effects/impacteffects/simplelightningstrikeimpact"), //small lightning, doesn't scale, probably still don't want lightning tho

                Assets.LoadAsset<GameObject>("prefabs/effects/muzzleflashes/muzzleflashmagelightning"), //6              // teeny      //gauntlet? way too small. what's this even for anyway?  
                Assets.LoadAsset<GameObject>("prefabs/effects/muzzleflashes/muzzleflashmagelightninglarge"),             // teeny      //cool but too arti again  
                Assets.LoadAsset<GameObject>("prefabs/effects/muzzleflashes/muzzleflashmagelightninglargewithtrail"),    // teeny      //same as last. when parented to gauntlet must be cool

                Assets.LoadAsset<GameObject>("prefabs/effects/omnieffect/omniimpactvfxlightning"), //9  //p good, scale too high, no sound            //also p good gauntlet
                Assets.LoadAsset<GameObject>("prefabs/effects/omnieffect/omniimpactvfxlightningmage"), //probably favorite, but too blatantly arti. also doesn't scale I think, outer radius blast too far (misleading (but i can just increase my range to match lol)
                Assets.LoadAsset<GameObject>("prefabs/effects/omnieffect/omniimpactvfxloaderlightning"), //same as 10 but yellow
            };

        private static void tryEffects(EffectData fect)
        {
            try
            {
                // bigZapEffectPrefab
                EffectManager.SpawnEffect(effects[(int)keep_bigsexyeffect], fect, true);
            }
            catch
            {
                keep_bigsexyeffect = 0;
                EffectManager.SpawnEffect(effects[(int)keep_bigsexyeffect], fect, true);
            }
        }
        #endregion testeffects

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.Skill;
        }
    }
}