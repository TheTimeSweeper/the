using MatcherMod.Modules.BaseStates;
using MatcherMod.Survivors.Matcher.MatcherContent;
using MatcherMod.Survivors.Matcher.SkillDefs;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace MatcherMod.Survivors.Matcher.SkillStates
{
    public class Secondary1Explosion : BaseTimedSkillState, IMatchBoostedState
    {
        public override float TimedBaseDuration => 0.6f;

        public override float TimedBaseCastStartPercentTime => 1;

        public int consumedMatches { get; set; }

        public override void OnEnter()
        {
            base.OnEnter();

            PlayAnimation("Fullbody, overried", "charge", "dash.playbackRate", duration);

            SmallHop(characterMotor, Config.M2_Staff2_SmallHop.Value);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            
            if (base.characterMotor)
            {
                if (isAuthority)
                {
                    ref float ySpeed = ref characterMotor.velocity.y;
                    ySpeed += Config.M2_Staff2_AntiGrav.Value * Time.deltaTime;
                }
            }
        }

        protected override void OnCastEnter()
        {
            base.OnCastEnter();

            PlayAnimation("Fullbody, overried", "ded3", "castDed.playbackRate", 0.15f);

            SmallHop(characterMotor, 10);

            int additionalStocks = activatorSkillSlot.stock;
            activatorSkillSlot.stock = 0;

            float blastRadius = Config.M2_Staff_Radius.Value + consumedMatches * Config.M2_Staff2_MatchRadius.Value;

            if (isAuthority)
            {
                EffectData data = new EffectData
                {
                    scale = blastRadius,
                    origin = transform.position,
                };

                EffectManager.SpawnEffect(MatcherContent.Assets.JoeFireballExplosion, data, true);

                new BlastAttack
                {
                    attacker = base.characterBody.gameObject,
                    baseDamage = Config.M2_Staff_Damage.Value * (1 + consumedMatches) * (1 + additionalStocks) * base.characterBody.damage,
                    baseForce = 2000,
                    attackerFiltering = AttackerFiltering.NeverHitSelf,
                    crit = base.characterBody.RollCrit(),
                    //damageColorIndex = DamageColorIndex.Item,
                    //damageType = DamageType.SlowOnHit,
                    inflictor = base.gameObject,
                    position = base.transform.position,
                    procChainMask = default(ProcChainMask),
                    procCoefficient = 1f,
                    radius = blastRadius,
                    teamIndex = base.characterBody.teamComponent.teamIndex,
                    falloffModel = BlastAttack.FalloffModel.None
                    //bonusForce = this.extraExplosionForce,
                    //damageType.damageSource = DamageSource.Utility,
                }.Fire();
            }
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write(consumedMatches);
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            consumedMatches = reader.ReadInt32();
        }
    }
}