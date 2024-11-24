using EntityStates;
using RoR2;

namespace KatamariMod.Survivors.Katamari.States
{
    public class ChargeUpRoll : BaseSkillState
    {
        private float speed;

        public override void OnEnter()
        {
            base.OnEnter();

            characterMotor.onHitGroundAuthority += CharacterMotor_onHitGround;

            if ((characterBody.bodyFlags & CharacterBody.BodyFlags.IgnoreFallDamage) == CharacterBody.BodyFlags.IgnoreFallDamage)
                return;

            base.characterBody.bodyFlags |= CharacterBody.BodyFlags.IgnoreFallDamage;
        }

        private void CharacterMotor_onHitGround(ref RoR2.CharacterMotor.HitGroundInfo hitGroundInfo)
        {
            base.characterBody.bodyFlags &= ~CharacterBody.BodyFlags.IgnoreFallDamage;
            characterMotor.onHitGroundAuthority -= CharacterMotor_onHitGround;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            speed += KatamariConfig.ChargeRoll_Multiplier.Value;

            if (!IsKeyDownAuthority())
            {
                characterMotor.velocity = GetAimRay().direction.normalized * speed;
                Log.Warning(speed);
                outer.SetNextStateToMain();
            }
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}
