using EntityStates;
using RA2Mod.Modules.BaseStates;
using RA2Mod.Survivors.Tesla;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace RA2Mod.Survivors.Tesla.States
{
    public class ShieldZapReleaseDamage : BaseTimedSkillState
    {
        public override float TimedBaseDuration => BaseDuration;
        public override float TimedBaseCastStartPercentTime => BaseCastTime;

        public static event Action onShieldZap;

        public static float range = 30;

        public static float BaseDuration = 0.5f;
        public static float BaseCastTime = 0.5f;

        public static float MaxDamageCoefficient = 18;

        public CameraTargetParams.AimRequest aimRequest;
        public float collectedDamage;
        public TemporaryOverlayInstance temporaryOverlay;

        public override void OnEnter()
        {
            base.OnEnter();

            if (NetworkServer.active)
            {
                characterBody.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, BaseDuration);
            }
            PlayCrossfade("Gesture, Additive", "CastShield", "CastShield.playbackRate", duration, 0.1f * duration);

        }

        protected override void OnCastEnter()
        {
            Blast();
        }

        private void Blast()
        {
            //doesn't get to here on anyone but tesla trooper, but hey if you do have fun
            float redeemedDamage = damageStat * 6.9f;
            TeslaZapBarrierController controller = GetComponent<TeslaZapBarrierController>();
            if (controller)
            {
                redeemedDamage = controller.GetReflectedDamage();
            }

            float damageStatMultiplier = damageStat / characterBody.baseDamage;

            float totalRedeemed = redeemedDamage * damageStatMultiplier * 0.5f;// + redeemedDamage * (damageStatMultiplier - 1) / 2;

            //give it a minimum damage to justify the explosion
            float blastDamage = Mathf.Max(totalRedeemed, damageStat * 1f);

            //float testPercentDamage = blastDamage / damageStat * 100;                                                                                                                                             //\n {redeemedDamage} + {redeemedDamage} * {(damageStatMultiplier - 1)}/2 = {blastDamage}
            //Helpers.LogWarning($"blastDamage: - {blastDamage}(%{testPercentDamage}) - \ndamage taken {redeemedDamage}, damageStatMultiplier {damageStatMultiplier}({damageStatMultiplier * 0.4f}), total redeemed {totalRedeemed}");

            //todo teslamove test damage growth, add max damage in description, sounds n indicators n effects n shit
            if (!TeslaConfig.M3_ChargingUp_Uncapped.Value)
            {
                blastDamage = Mathf.Min(blastDamage, damageStat * MaxDamageCoefficient);
            }

            onShieldZap?.Invoke();

            if (NetworkServer.active)
            {
                BlastAttack blast = new BlastAttack
                {
                    attacker = gameObject,
                    inflictor = gameObject,
                    teamIndex = teamComponent.teamIndex,
                    //attackerFiltering = AttackerFiltering.NeverHit

                    position = transform.position,
                    radius = range,
                    falloffModel = BlastAttack.FalloffModel.None,

                    baseDamage = blastDamage,
                    crit = RollCrit(),
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

            Util.PlaySound(EntityStates.VagrantMonster.FireMegaNova.novaSoundString, gameObject);

            #region effects

            Transform chest = GetModelChildLocator()?.FindChild("Chest");
            Vector3 pos = chest ? chest.position : transform.position;

            EffectData fect = new EffectData
            {
                origin = pos,
                scale = range,
            };

            EffectManager.SpawnEffect(BigZap.bigZapEffectPrefabArea, fect, true);

            //if (EntityStates.VagrantMonster.FireMegaNova.novaEffectPrefab) {
            //    EffectManager.SpawnEffect(EntityStates.VagrantMonster.FireMegaNova.novaEffectPrefab, fect, false);
            //}

            //if (!Input.GetKey(KeyCode.G))
            EffectManager.SpawnEffect(BigZap.bigZapEffectPrefab, fect, false);

            //if (Input.GetKey(KeyCode.H)) {
            //fect.scale /= 2f;
            //EffectManager.SpawnEffect(BigZap.bigZapEffectFlashPrefab, fect, true);
            //}
            #endregion effects
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }
        public override void OnExit()
        {
            base.OnExit();

            if (aimRequest != null)
                aimRequest.Dispose();

            if (temporaryOverlay != null)
            {
                temporaryOverlay.Destroy();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}