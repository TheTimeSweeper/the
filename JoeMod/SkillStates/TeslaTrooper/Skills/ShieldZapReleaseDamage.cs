using EntityStates;
using ModdedEntityStates.BaseStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace ModdedEntityStates.TeslaTrooper {
    public class ShieldZapReleaseDamage : BaseTimedSkillState {

        public static float range = 30;

        public static float BaseDuration = 1;
        public static float BaseCastTime = 0.5f;

        public static float MaxDamageCoefficient = 15;

        public CameraTargetParams.AimRequest aimRequest;
        public float collectedDamage;

        public override void OnEnter() {
            base.OnEnter();
            InitDurationValues(BaseDuration, BaseCastTime);

            characterBody.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, BaseDuration);

            base.PlayCrossfade("Gesture, Override", "CastShield", "CastShield.playbackRate", duration, 0.1f * duration);
        }

        protected override void OnCastEnter() {
            Blast();
        }

        private void Blast() {

                                   //doesn't get to here on anyone but tesla trooper, but hey if you do the skill will still work
            float redeemedDamage = damageStat * 3f;
            ZapBarrierController controller = GetComponent<ZapBarrierController>();
            if (controller) {
                redeemedDamage = controller.RedeemDamage();
            }

            float damageMultiplier = damageStat / characterBody.baseDamage;

            float totalRedeemed = redeemedDamage * damageMultiplier * 0.4f;// + redeemedDamage * (damageMultiplier - 1) / 2;

            //give it a minimum damage to justify the explosion
            float blastDamage = Mathf.Max(totalRedeemed, damageStat * 1f);

            float testPercentDamage = blastDamage / damageStat * 100;                                                                                                                                             //\n {redeemedDamage} + {redeemedDamage} * {(damageMultiplier - 1)}/2 = {blastDamage}
            Helpers.LogWarning($"blastDamage: - {blastDamage}(%{testPercentDamage}) - \ndamage taken {redeemedDamage}, damageMultiplier {damageMultiplier}({damageMultiplier * 0.4f}), total redeemed {totalRedeemed}");

            if (!Modules.Config.UncappedUtility.Value) {
                blastDamage = Mathf.Min(blastDamage, damageStat * MaxDamageCoefficient);
            }
            
            if (NetworkServer.active) {
                
                BlastAttack blast = new BlastAttack {
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

            Vector3 pos = GetModelChildLocator() ? GetModelChildLocator().FindChild("Chest").position : transform.position;

            EffectData fect = new EffectData {
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

        public override void OnExit() {
            base.OnExit();

            aimRequest.Dispose();

            TemporaryOverlay overlay = GetComponent<TemporaryOverlay>();
            if (overlay) {
                Destroy(overlay);
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.PrioritySkill;
        }
    }
}