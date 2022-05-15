using EntityStates;
using ModdedEntityStates.BaseStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace ModdedEntityStates.TeslaTrooper {
    public class ShieldZapReleaseDamage : BaseTimedSkillState {

        public static float damageMultiplier = 1;
        public static float range = 30;

        public static float BaseDuration = 1;
        public static float BaseCastTime = 0.5f;

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

            //probably doesn't even get to here on anyone but tesla trooper, but hey if you do
            float redeemedDamage = damageStat * 3f;
            ZapBarrierController controller = GetComponent<ZapBarrierController>();
            if (controller) {
                redeemedDamage = controller.RedeemDamage();
            }

            //Helpers.LogWarning(NetworkServer.active + " | " + "redeemed damage: " + damage);
            //Helpers.LogWarning(NetworkServer.active + " | " + "damage from events: " + collectedDamage);

            //give it a minimum damage to justify the explosion
            float blastDamage = Mathf.Max(redeemedDamage, damageStat * 1f);

            //range and/or no blast if no damage is absorbed
            if (NetworkServer.active) {
                
                BlastAttack blast = new BlastAttack {
                    attacker = gameObject,
                    inflictor = gameObject,
                    teamIndex = teamComponent.teamIndex,
                    //attackerFiltering = AttackerFiltering.NeverHit

                    position = transform.position,
                    radius = range,
                    falloffModel = BlastAttack.FalloffModel.None,

                    baseDamage = blastDamage * damageMultiplier,
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
            fect.scale /= 2f;
            EffectManager.SpawnEffect(BigZap.bigZapEffectFlashPrefab, fect, true);
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