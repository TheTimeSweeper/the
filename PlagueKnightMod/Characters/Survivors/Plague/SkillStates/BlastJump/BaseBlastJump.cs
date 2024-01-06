using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace PlagueMod.Survivors.Plague.SkillStates
{
    public class BaseBlastJump : BaseSkillState
    {
        // Token: 0x04000DD4 RID: 3540
        //public static GameObject blinkPrefab;

        // Token: 0x04000DD5 RID: 3541
        public static float duration = 0.3f;

        // Token: 0x04000DD6 RID: 3542
        public static string beginSoundString;

        public static float forwardVelocity => PlagueConfig.blastJumpForward.Value;
        public static float upwardVelocity => PlagueConfig.blastJumpUpward.Value;

        // Token: 0x04000DE2 RID: 3554
        protected Vector3 blastPosition;

        // Token: 0x06000BAD RID: 2989 RVA: 0x000308AC File Offset: 0x0002EAAC
        public override void OnEnter()
        {
            base.OnEnter();

            Util.PlaySound(BaseBlastJump.beginSoundString, base.gameObject);
            base.PlayAnimation("FullBody, Underride", "Smear");
            base.characterMotor.Motor.ForceUnground();
            base.characterMotor.velocity = Vector3.zero;

            Vector3 moveDirection = inputBank.moveVector;

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

        // Token: 0x06000BB3 RID: 2995 RVA: 0x00030AFF File Offset: 0x0002ECFF
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge >= BaseBlastJump.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
            }
        }
    }
}