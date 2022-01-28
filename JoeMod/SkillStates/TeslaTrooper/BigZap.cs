using EntityStates;
using RoR2;
using UnityEngine;

namespace JoeMod.ModdedEntityStates.TeslaTrooper
{
    public class BigZap : BaseSkillState
    {
        public static float DamageCoefficient = 6.9f;
        public static float ProcCoefficient = 1f;
        public static float AttackRadius = 10;

        public Vector3 aimPoint;

        public GameObject bigZapEffectPrefab = Resources.Load<GameObject>("prefabs/effects/magelightningbombexplosion");
        public GameObject bigZapEffectPrefabArea = Resources.Load<GameObject>("prefabs/effects/lightningstakenova");
        public GameObject bigZapEffectPrefabFlash = Resources.Load<GameObject>("prefabs/effects/omnieffect/omniimpactvfxlightning");

        public override void OnEnter()
        {
            base.OnEnter();


            //todo incombat
            PlayAnimation("Gesture, Override", "HandOut");
            PlayAnimation("Gesture, Additive", "Shock", "Shock.playbackRate", 0.3f);

            new BlastAttack
            {
                attacker = gameObject,
                inflictor = gameObject,
                teamIndex = TeamComponent.GetObjectTeam(gameObject),
                //attackerFiltering = AttackerFiltering.NeverHit

                position = aimPoint,
                radius = AttackRadius,
                falloffModel = BlastAttack.FalloffModel.None,

                baseDamage = damageStat * DamageCoefficient,
                crit = RollCrit(),
                damageType = DamageType.Stun1s,
                //damageColorIndex = DamageColorIndex.Default,

                procCoefficient = 1,
                //procChainMask = 
                //losType = BlastAttack.LoSType.NearestHit,

                baseForce = -5, //enfucker void grenade here we go
                //bonusForce = ;

                //impactEffect = EffectIndex.uh;

            }.Fire();
            EffectData fect = new EffectData
            {
                origin = aimPoint,
                scale = AttackRadius,
            };

            if (Input.GetKey(KeyCode.LeftAlt))
            {
                tryEffects(fect);
                return;
            }

            EffectManager.SpawnEffect(bigZapEffectPrefabArea, fect, false);

            if (!Input.GetKey(KeyCode.G))
                EffectManager.SpawnEffect(bigZapEffectPrefab, fect, false);

            if (Input.GetKey(KeyCode.H))
            {
                fect.scale /= 2f;
                EffectManager.SpawnEffect(bigZapEffectPrefabFlash, fect, false);
            }

        }

        #region testEffects
        public static float bigsexyeffect = 0;

        public static GameObject[] effects =
            {
                Resources.Load<GameObject>("prefabs/effects/magelightningbombexplosion"), //oke
                Resources.Load<GameObject>("prefabs/effects/lightningstakenova"), //decent, simple, probably best but boring

                Resources.Load<GameObject>("prefabs/effects/impacteffects/impactlightning"), //2 //teeny         //gauntlet? too blue, not cyan, mabye that's fine
                Resources.Load<GameObject>("prefabs/effects/impacteffects/lightningflash"),// teeny              //looks like purity lol
                Resources.Load<GameObject>("prefabs/effects/impacteffects/lightningstrikeimpact"), //NOT TEENY
                Resources.Load<GameObject>("prefabs/effects/impacteffects/simplelightningstrikeimpact"), //small lightning, doesn't scale, probably still don't want lightning tho

                Resources.Load<GameObject>("prefabs/effects/muzzleflashes/muzzleflashmagelightning"), //6              // teeny      //gauntlet? way too small. what's this even for anyway?  
                Resources.Load<GameObject>("prefabs/effects/muzzleflashes/muzzleflashmagelightninglarge"),             // teeny      //cool but too arti again  
                Resources.Load<GameObject>("prefabs/effects/muzzleflashes/muzzleflashmagelightninglargewithtrail"),    // teeny      //same as last. when parented to gauntlet must be cool

                Resources.Load<GameObject>("prefabs/effects/omnieffect/omniimpactvfxlightning"), //9  //p good, scale too high, no sound            //also p good gauntlet
                Resources.Load<GameObject>("prefabs/effects/omnieffect/omniimpactvfxlightningmage"), //probably favorite, but too blatantly arti. also doesn't scale I think, outer radius blast too far (misleading (but i can just increase my range to match lol)
                Resources.Load<GameObject>("prefabs/effects/omnieffect/omniimpactvfxloaderlightning"), //same as 10 but yellow
            };


        private static void tryEffects(EffectData fect)
        {
            try
            {
                // bigZapEffectPrefab
                EffectManager.SpawnEffect(effects[(int)bigsexyeffect], fect, true);
            }
            catch
            {
                bigsexyeffect = 0;
                EffectManager.SpawnEffect(effects[(int)bigsexyeffect], fect, true);
            }
        }
        #endregion testeffects
    }
}