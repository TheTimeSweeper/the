﻿using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace PlagueMod.Survivors.Plague.SkillStates
{
    public class BaseBlastJump : GenericCharacterMain
    {
        // Token: 0x04000DD4 RID: 3540
        //public static GameObject blinkPrefab;

        // Token: 0x04000DD5 RID: 3541
        public static float minDuration = 0.3f;

        // Token: 0x04000DD6 RID: 3542
        public static string beginSoundString;

        public static float forwardVelocity => PlagueConfig.blastJumpForward.Value;
        public static float upwardVelocity => PlagueConfig.blastJumpUpward.Value;

        public static float airControl => PlagueConfig.blastJumpAirControl.Value;

        public static float gravity => PlagueConfig.blastJumpGravity.Value;

        // Token: 0x04000DE2 RID: 3554
        protected Vector3 blastPosition;

        protected float previousAirControl;

        // Token: 0x06000BAD RID: 2989 RVA: 0x000308AC File Offset: 0x0002EAAC
        public override void OnEnter()
        {
            base.OnEnter();

            Util.PlaySound(BaseBlastJump.beginSoundString, base.gameObject);
            base.PlayAnimation("FullBody, Underride", "Smear");
            base.characterMotor.Motor.ForceUnground();
            base.characterMotor.velocity = Vector3.zero;
            previousAirControl = characterMotor.airControl;
            characterMotor.airControl = airControl;

            Vector3 moveDirection = inputBank.moveVector;

            base.characterBody.bodyFlags |= CharacterBody.BodyFlags.IgnoreFallDamage;

            if (base.isAuthority)
            {
                this.blastPosition = base.characterBody.corePosition;

                base.characterBody.isSprinting = true;
                Vector3 a = moveDirection * forwardVelocity * this.moveSpeedStat;
                Vector3 b = Vector3.up * upwardVelocity;
                base.characterMotor.Motor.ForceUnground();
                base.characterMotor.velocity = a + b;
            }
        }

        // Token: 0x06000BAE RID: 2990 RVA: 0x00030A29 File Offset: 0x0002EC29
        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write(this.blastPosition);
        }

        // Token: 0x06000BAF RID: 2991 RVA: 0x00030A3E File Offset: 0x0002EC3E
        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            this.blastPosition = reader.ReadVector3();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (isAuthority)
            {
                ref float ySpeed = ref characterMotor.velocity.y;
                ySpeed += -gravity * Time.deltaTime;
            }

            if (base.fixedAge >= BaseBlastJump.minDuration && characterMotor.isGrounded)
            {
                this.outer.SetNextStateToMain();
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            base.characterBody.bodyFlags &= ~CharacterBody.BodyFlags.IgnoreFallDamage;
            characterMotor.airControl = previousAirControl;
        }

        public override void ProcessJump()
        {
            if (this.jumpInputReceived && base.characterBody && base.characterMotor.jumpCount < base.characterBody.maxJumpCount)
            {
                Vector3 previousLateralVelocity = characterMotor.velocity;
                previousLateralVelocity.y = 0;
                base.ProcessJump();
                characterMotor.velocity += previousLateralVelocity;
                characterMotor.velocity.y = Mathf.Sqrt((characterMotor.velocity.y * characterMotor.velocity.y * (Physics.gravity.y - gravity))/Physics.gravity.y); //been like 10 years. can I still do algebra?
            }
            else
            {
                base.ProcessJump();
            }
        }
    }
}