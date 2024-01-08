using EntityStates;
using UnityEngine;

namespace PlagueMod.Survivors.Plague.SkillStates
{
    public class PlagueCharacterMain : GenericCharacterMain
    {
        public override void ProcessJump()
        {
            if(characterMotor.isGrounded)
            {
                base.ProcessJump();
                return;
            }

            Vector3 previousLateralVelocity = characterMotor.velocity;
            previousLateralVelocity.y = 0;
            base.ProcessJump();
            characterMotor.velocity += previousLateralVelocity;
        }
    }
}