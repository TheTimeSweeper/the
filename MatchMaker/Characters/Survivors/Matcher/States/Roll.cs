using EntityStates;
using MatcherMod.Survivors.Matcher.SkillDefs;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace MatcherMod.Survivors.Matcher.SkillStates
{
    public class Roll : BaseSkillState, IMatchBoostedState
    {
        public static float baseDuration => MatcherContent.Config.M3_Shield_RollDuration.Value;
        public static float initialSpeedCoefficient => MatcherContent.Config.M3_Shield_RollInitialSpeed.Value;
        public static float finalSpeedCoefficient => MatcherContent.Config.M3_Shield_RollFinalSpeed.Value;

        public static string dodgeSoundString = "HenryRoll";
        public static float dodgeFOV = global::EntityStates.Commando.DodgeState.dodgeFOV;

        private float matchSpeedMultiplier = 1;
        private float sprintSpeedMultiplier = 1;

        private float duration;
        private float rollSpeed;
        private Vector3 dashVector;
        //private Vector3 forwardDirection;
        private Animator animator;
        //private Vector3 previousPosition;

        public int consumedMatches { get; set; }

        public override void OnEnter()
        {
            base.OnEnter();
            animator = GetModelAnimator();

            characterMotor.Motor.ForceUnground();
            matchSpeedMultiplier = 1 + consumedMatches * MatcherContent.Config.M3_Shield_RollMatchSpeedMultiplier.Value;
            sprintSpeedMultiplier = characterBody.isSprinting ? 1 : characterBody.sprintingSpeedMultiplier;

            //if (isAuthority && inputBank && characterDirection)
            //{
            //    forwardDirection = (inputBank.moveVector == Vector3.zero ? characterDirection.forward : inputBank.moveVector).normalized;
            //}

            //Vector3 rhs = characterDirection ? characterDirection.forward : forwardDirection;
            //Vector3 rhs2 = Vector3.Cross(Vector3.up, rhs);

            //float num = Vector3.Dot(forwardDirection, rhs);
            //float num2 = Vector3.Dot(forwardDirection, rhs2);

            duration = baseDuration;

            RecalculateRollSpeed();

            dashVector = GetDashVector();
            if (dashVector.sqrMagnitude < Mathf.Epsilon)
            {
                dashVector = inputBank.aimDirection;
            }
            //base.gameObject.layer = LayerIndex.fakeActor.intVal;
            base.characterDirection.forward = dashVector;

            //Transform centerTransform = base.FindModelChild("center");
            //GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Prefabs.dashEffect, centerTransform.position + dashVector * 4f, Util.QuaternionSafeLookRotation(-dashVector));

            if (characterMotor && characterDirection)
            {
                characterMotor.velocity = dashVector * rollSpeed;
            }

            //Vector3 b = characterMotor ? characterMotor.velocity : Vector3.zero;
            //previousPosition = transform.position - b;

            PlayAnimation("Fullbody, overried", "Dash", "Dash.playbackRate", duration);
            Util.PlaySound(dodgeSoundString, gameObject);

            if (NetworkServer.active && consumedMatches > 0)
            {
                //characterBody.AddTimedBuff(MatcherContent.Buffs.armorBuff, 3f * duration);
                characterBody.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, duration);
            }
        }

        private Vector3 GetDashVector()
        {
            Vector3 aimDirection = inputBank.aimDirection;
            aimDirection.y = 0;
            Vector3 rightDirection = -Vector3.Cross(Vector3.up, aimDirection);
            float angle = Vector3.Angle(inputBank.aimDirection, aimDirection);
            if (inputBank.aimDirection.y < 0) angle = -angle;
            return Vector3.Normalize(Quaternion.AngleAxis(angle, rightDirection) * inputBank.moveVector);
        }

        private void RecalculateRollSpeed()
        {
            rollSpeed = moveSpeedStat * matchSpeedMultiplier * sprintSpeedMultiplier * Mathf.Lerp(initialSpeedCoefficient, finalSpeedCoefficient, fixedAge / duration);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            RecalculateRollSpeed();

            //if (characterDirection) characterDirection.forward = forwardDirection;
            //if (cameraTargetParams) cameraTargetParams.fovOverride = Mathf.Lerp(dodgeFOV, 60f, fixedAge / duration);

            //Vector3 moveDirection = (transform.position - previousPosition).normalized;
            //if (characterMotor && characterDirection && moveDirection != Vector3.zero)
            //{
            //    Vector3 vector = moveDirection * rollSpeed;
            //    float d = Mathf.Max(Vector3.Dot(vector, forwardDirection), 0f);
            //    vector = forwardDirection * d;
            //    vector.y = 0f;

            //    characterMotor.velocity = vector;
            //}
            //previousPosition = transform.position;

            characterMotor.velocity = dashVector * rollSpeed;

            if (isAuthority && fixedAge >= duration)
            {
                outer.SetNextStateToMain();
                return;
            }
        }

        public override void OnExit()
        {
            if (cameraTargetParams) cameraTargetParams.fovOverride = -1f;
            base.OnExit();

            characterMotor.disableAirControlUntilCollision = false;
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write(dashVector/*forwardDirection*/);
            writer.Write(consumedMatches);
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            /*forwardDirection*/
            dashVector = reader.ReadVector3();
            consumedMatches = reader.ReadInt32();
        }
    }
}