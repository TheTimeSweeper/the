using AliemMod.Content;
using EntityStates;
using UnityEngine;

namespace ModdedEntityStates.Aliem
{
    public class SwordFireChargedDash : BaseTimedSkillState
    {
        public override float TimedBaseDuration => 0.2f;
        public override float TimedBaseCastStartPercentTime => 1;

        private Vector3 blinkVector = Vector3.zero;
        private float _speedCoefficient;
        private Vector3 _previousVelocity;

        public SwordFireChargedDash()
        {
            _speedCoefficient = AliemConfig.M1_SwordCharged_Speed_Max.Value;
        }

        public SwordFireChargedDash(float speedCoefficient)
        {
            _speedCoefficient = speedCoefficient;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            this.blinkVector = this.GetBlinkVector();
            if (base.characterMotor)
            {
                characterMotor.Motor.ForceUnground();
                _previousVelocity = characterMotor.velocity;
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.characterMotor)
            {
                base.characterMotor.velocity = Vector3.zero;
                base.characterMotor.rootMotion += this.blinkVector * (_speedCoefficient * Time.fixedDeltaTime);
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            characterMotor.velocity.x = _previousVelocity.x;
            characterMotor.velocity.z = _previousVelocity.z;
        }
        protected virtual Vector3 GetBlinkVector()
        {
            return base.inputBank.aimDirection;
        }
    }
}