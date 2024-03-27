using EntityStates;
using RoR2;
using UnityEngine.Networking;
using UnityEngine;
using RA2Mod.Modules.BaseStates;

namespace RA2Mod.Survivors.Tesla.States
{
    public class BigZap : BaseTimedSkillState
    {
        public override float TimedBaseDuration => BaseDuration;
        public override float TimedBaseCastStartPercentTime => BaseCastTime;

        //todo teslamove assets
        public static GameObject bigZapEffectPrefab = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/magelightningbombexplosion");
        public static GameObject bigZapEffectPrefabArea = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/lightningstakenova");
        public static GameObject bigZapEffectFlashPrefab = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/omnieffect/omniimpactvfxlightning");

        public static float DamageCoefficient = 6.9f;
        public static float ProcCoefficient = 1f;
        public static float BaseAttackRadius = 10;

        public static float BaseDuration = 0.6f;
        public static float BaseCastTime = 0;//0.2f //todo windup sound, windup vfx, and zapping ground

        public float skillsPlusAreaMulti = 1f;
        public float skillsPlusDamageMulti = 1f;
        public float attackRadius;
        public Vector3 aimPoint;

        private string zapSound = "Play_tank_vtesatta_tesla_tank_attack";
        private string zapSoundCrit = "Play_tank_vtesattb_tesla_tank_attack";

        private bool commandedTowers;
        private HurtBox commandTarget;


        public override void OnEnter()
        {
            base.OnEnter();

            //todo teslamove hascomponentskilldef
            TeslaTowerControllerController controller = GetComponent<TeslaTowerControllerController>();

            if (controller && controller.coilReady && isAuthority)
            {
                //client will find the commandtarget and serialize it
                commandTarget = GetComponent<TeslaTrackerComponentZap>()?.GetTrackingTarget();
            }

            //server will deserialize a commandTarget from the client
            if (commandTarget)
            {

                if (NetworkServer.active)
                {
                    controller.commandTowers(commandTarget);
                }

                commandedTowers = true;
            }

            if (commandedTowers)
            {

                //todo anim tower
                PlayAnimation("Gesture, Additive", "Shock_bak", "Shock.playbackRate", 0.3f);

            }
            else
            {

                attackRadius = BaseAttackRadius * skillsPlusAreaMulti;

                PlayAnimation("Gesture, Additive", "Shock", "Shock.playbackRate", 0.3f);

                characterBody.AddSpreadBloom(1);
            }
        }

        protected override void OnCastEnter()
        {
            base.OnCastEnter();

            if (commandedTowers)
                return;

            bool isCrit = RollCrit();

            if (isAuthority)
            {

                BlastAttack blast = new BlastAttack
                {
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
                blast.Fire();
            }

            Util.PlaySound(isCrit ? zapSound : zapSoundCrit, gameObject);

            #region effects
            EffectData fect = new EffectData
            {
                origin = aimPoint,
                scale = attackRadius,
            };

            if (General.GeneralConfig.Debug.Value && Input.GetKey(KeyCode.LeftAlt))
            {
                tryEffects(fect);
                return;
            }

            EffectManager.SpawnEffect(bigZapEffectPrefabArea, fect, true);

            if (!Input.GetKey(KeyCode.G))
                EffectManager.SpawnEffect(bigZapEffectPrefab, fect, true);

            if (Input.GetKey(KeyCode.H))
            {
                fect.scale /= 2f;
                EffectManager.SpawnEffect(bigZapEffectFlashPrefab, fect, true);
            }
            #endregion effects
        }

        #region testGameEffects
        public static float keep_bigsexyeffect = 0;

        public static GameObject[] effects =
            {
                LegacyResourcesAPI.Load<GameObject>("prefabs/effects/magelightningbombexplosion"), //oke
                LegacyResourcesAPI.Load<GameObject>("prefabs/effects/lightningstakenova"), //decent, simple, probably best but boring

                LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/impactlightning"), //2 //teeny         //gauntlet? too blue, not cyan, mabye that's fine
                LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/lightningflash"),// teeny              //looks like purity lol
                LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/lightningstrikeimpact"), //NOT TEENY
                LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/simplelightningstrikeimpact"), //small lightning, doesn't scale, probably still don't want lightning tho

                LegacyResourcesAPI.Load<GameObject>("prefabs/effects/muzzleflashes/muzzleflashmagelightning"), //6              // teeny      //gauntlet? way too small. what's this even for anyway?  
                LegacyResourcesAPI.Load<GameObject>("prefabs/effects/muzzleflashes/muzzleflashmagelightninglarge"),             // teeny      //cool but too arti again  
                LegacyResourcesAPI.Load<GameObject>("prefabs/effects/muzzleflashes/muzzleflashmagelightninglargewithtrail"),    // teeny      //same as last. when parented to gauntlet must be cool

                LegacyResourcesAPI.Load<GameObject>("prefabs/effects/omnieffect/omniimpactvfxlightning"), //9  //p good, scale too high, no sound            //also p good gauntlet
                LegacyResourcesAPI.Load<GameObject>("prefabs/effects/omnieffect/omniimpactvfxlightningmage"), //probably favorite, but too blatantly arti. also doesn't scale I think, outer radius blast too far (misleading (but i can just increase my range to match lol)
                LegacyResourcesAPI.Load<GameObject>("prefabs/effects/omnieffect/omniimpactvfxloaderlightning"), //same as 10 but yellow
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

        public override void OnExit()
        {
            base.OnExit();

            GetModelAnimator().SetBool("isHandOut", false);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

        // Token: 0x0600419A RID: 16794 RVA: 0x0002F86B File Offset: 0x0002DA6B
        public override void OnSerialize(NetworkWriter writer)
        {

            writer.Write(HurtBoxReference.FromHurtBox(commandTarget));
            //not needed here, as both server and authority will set this value in onenter
            //writer.Write(commandedTowers);
        }

        // Token: 0x0600419B RID: 16795 RVA: 0x0010A8CC File Offset: 0x00108ACC
        public override void OnDeserialize(NetworkReader reader)
        {

            commandTarget = reader.ReadHurtBoxReference().ResolveHurtBox();
            //this.commandedTowers = reader.ReadBoolean();
        }
    }
}