using RoR2;
using System;
using UnityEngine;

namespace AliemMod.Components
{
    public class SmallHopController : MonoBehaviour
    {
        //to fuck with in runtimeinspector
        public float jumpMax = 10;
        public float jump = 10;
        public float jumpDiminishMin = 0.05f;
        public float jumpDiminishMax = 0.2f;
        public float jumpDiminishDecayRate = 0.03f;

        private CharacterMotor _motor;
        private GenericSkill _primarySkill;
        private GenericSkill _secondarySkill;
        private CharacterBody _characterBody;
        private InputBankTest _inputBank;

        private float _multiplier = 1;
        private float _diminishDecayTimer;

        void Start()
        {
            _motor = GetComponent<CharacterMotor>();
            _motor.onHitGroundAuthority += _motor_onHitGroundAuthority;
            _primarySkill = GetComponent<SkillLocator>().primary;
            _secondarySkill = GetComponent<SkillLocator>().secondary;
            _characterBody = GetComponent<CharacterBody>();
            _inputBank = GetComponent<InputBankTest>();

            _characterBody.onSkillActivatedAuthority += CharacterBody_onSkillActivatedAuthority;
        }

        void FixedUpdate()
        {
            _diminishDecayTimer -= jumpDiminishDecayRate;
        }

        private void _motor_onHitGroundAuthority(ref CharacterMotor.HitGroundInfo hitGroundInfo)
        {
            ResetAir();
        }

        private void ResetAir()
        {
            _multiplier = 1;
            _diminishDecayTimer = 0;
        }

        private void CharacterBody_onSkillActivatedAuthority(GenericSkill skill)
        {
            if (skill == _primarySkill || skill == _secondarySkill)
            {
                CalculateSmallHop();
            }
        }

        private void CalculateSmallHop()
        {
            SmallHopAuthority(_multiplier);
        }

        public void SmallHopAuthority(float inMultiplier, bool aimDown = true, bool limit = true)
        {
            if (_motor.isGrounded)
                return;

            float multiplier = inMultiplier;
            float aimMultiplier;
            if (aimDown)
            {
                aimMultiplier = Mathf.Clamp(Vector3.Dot(_inputBank.GetAimRay().direction.normalized, Vector3.down), 0, 0.7f);
            }
            else
            {
                aimMultiplier = 1;
            }
            multiplier *= aimMultiplier;

            float inVelY = _motor.velocity.y;
            ref float velocityY = ref _motor.velocity.y;
            if (!(limit && velocityY > jumpMax)) {
                velocityY += jump * multiplier;
            }

            DiminishHop(aimMultiplier);

            //Helpers.LogWarning($"aimMultiplier {aimMultiplier.ToString("0.00")}, inMult {inMultiplier.ToString("0.00")}, fMult{multiplier.ToString("0.00")}, inVelY {inVelY.ToString("0.00")}, velocityY {velocityY.ToString("0.00")}");
        }

        private void DiminishHop(float aimMultiplier)
        {
            //Helpers.LogWarning($"_multiplier {_multiplier.ToString("0.00")}, _diminishDecayTimer {_diminishDecayTimer.ToString("0.00")}");

            _multiplier -= Mathf.Lerp(jumpDiminishMin, jumpDiminishMax, _diminishDecayTimer);
            _multiplier = Mathf.Max(_multiplier, 0);
            _diminishDecayTimer = 1;

            //Helpers.LogWarning($"_multiplier {_multiplier.ToString("0.00")}, _diminishDecayTimer {_diminishDecayTimer.ToString("0.00")}");
        }
    }
}
