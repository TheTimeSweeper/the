using EntityStates;
using KatamariMod.SkillStates.BaseStates;
using RoR2.Audio;
using RoR2;
using static Rewired.ComponentControls.Effects.RotateAroundAxis;
using UnityEngine;

namespace KatamariMod.Survivors.Katamari.States
{
    public class Shove : BaseTimedSkillState
    {
        private OverlapAttack overlapAttack;

        public override float TimedBaseDuration => 0.1f;

        public override float TimedBaseCastStartPercentTime => 0f;
        public override bool DontAttackSpeed => true;

        private Vector3 velocity;
        private bool hit;

        public override void OnEnter()
        {
            base.OnEnter();

            HitBoxGroup hitBoxGroup = base.FindHitBoxGroup("Katamari");

            velocity = Vector3.Cross(Vector3.Cross(GetAimRay().direction, Vector3.up), Vector3.down);
            velocity *= KatamariConfig.shoveSpeed.Value;

            overlapAttack = new OverlapAttack();
            overlapAttack.attacker = base.gameObject;
            overlapAttack.damage = characterBody.damage * KatamariConfig.passive_speedAttackMultiplier.Value;
            overlapAttack.damageColorIndex = DamageColorIndex.Default;
            overlapAttack.damageType = DamageType.Generic;
            overlapAttack.forceVector = velocity;
            overlapAttack.hitBoxGroup = hitBoxGroup;
            overlapAttack.hitEffectPrefab = null;
            NetworkSoundEventDef networkSoundEventDef = null;
            overlapAttack.impactSound = ((networkSoundEventDef != null) ? networkSoundEventDef.index : NetworkSoundEventIndex.Invalid);
            overlapAttack.inflictor = base.gameObject;
            overlapAttack.isCrit = base.RollCrit();
            overlapAttack.procChainMask = default(ProcChainMask);
            overlapAttack.pushAwayForce = 1000;
            overlapAttack.procCoefficient = 1;
            overlapAttack.teamIndex = base.GetTeam();
        }
        
        public override void FixedUpdate()
        {
            base.FixedUpdate();

            hit = overlapAttack.Fire();

            if (hit) {

                characterMotor.rootMotion = Vector3.zero;
                Vector3 recoilVelocity = Vector3.Cross(Vector3.Cross(GetAimRay().direction, Vector3.up), Vector3.down);
                recoilVelocity = -recoilVelocity + Vector3.up * 0.5f;
                characterMotor.velocity = recoilVelocity * KatamariConfig.impactBounceMultiplier.Value;
                return;
            }

            characterMotor.rootMotion = velocity;
        }

        protected override void SetNextState()
        {
            outer.SetNextState(new WindDownState { windDownTime = 0.3f });
        }
        
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}
