using EntityStates;
using MatcherMod.Modules.BaseStates;
using MatcherMod.Survivors.Matcher.Components;
using MatcherMod.Survivors.Matcher.Content;
using MatcherMod.Survivors.Matcher.SkillDefs;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace MatcherMod.Survivors.Matcher.SkillStates
{
    public class Secondary2Explosion : BaseTimedSkillState, IMatchBoostedState
    {
        public override float TimedBaseDuration => 1f;

        public override float TimedBaseCastStartPercentTime => 0.6f;

        public int consumedMatches { get; set; }

        public override void OnEnter()
        {
            base.OnEnter();

            PlayAnimation("FullBody, Override", "StaffExplosionEnter", "Staff.playbackRate", castStartTime);

            if (consumedMatches > 0)
            {
                GetModelTransform().gameObject.GetComponent<MatcherViewController>().RevealStaff(duration);
            }

            SmallHop(characterMotor, 10);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            
            if (base.characterMotor)
            {
                if (isAuthority)
                {
                    ref float ySpeed = ref characterMotor.velocity.y;
                    ySpeed += 5 * Time.deltaTime;
                }
            }
        }

        protected override void OnCastEnter()
        {
            base.OnCastEnter();

            PlayAnimation("Fullbody, Override", "StaffExplosion");

            SmallHop(characterMotor, 10);

            int additionalStocks = activatorSkillSlot.stock;
            activatorSkillSlot.stock = 0;

            float blastRadius = CharacterConfig.M2_Staff_Radius.Value + consumedMatches * CharacterConfig.M2_Staff2_MatchRadius.Value;

            if (isAuthority)
            {
                EffectData data = new EffectData
                {
                    scale = blastRadius,
                    origin = transform.position,
                };

                EffectManager.SpawnEffect(Content.CharacterAssets.JoeFireballExplosion, data, true);

                new BlastAttack
                {
                    attacker = base.characterBody.gameObject,

                    baseDamage = base.characterBody.damage * ((1 + additionalStocks) * CharacterConfig.M2_Staff2_Damage.Value + (consumedMatches * CharacterConfig.M2_Staff2_Damage_Match.Value)),
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

        public override void OnExit()
        {
            base.OnExit();
            if (fixedAge < castStartTime)
            {
                PlayAnimation("Fullbody, Override", "BufferEmpty");
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

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}