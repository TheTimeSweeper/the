using MatcherMod.Modules.BaseStates;
using RoR2.Skills;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using MatcherMod.Survivors.Matcher.Content;
using MatcherMod.Survivors.Matcher.SkillDefs;
using MatcherMod.Survivors.Matcher.Components;

namespace MatcherMod.Survivors.Matcher.SkillStates
{
    public class Sword : BaseMeleeAttack, SteppedSkillDef.IStepSetter, IMatchBoostedState
    {
        public int swingIndex;

        public int consumedMatches { get; set; }

        //used by the steppedskilldef to increment your combo whenever this state is entered
        public void SetStep(int i)
        {
            swingIndex = i;
        }

        public override void OnEnter()
        {
            //mouse over variables for detailed explanations
            hitBoxGroupName = "swingTall";

            damageType = DamageType.Generic;
            damageCoefficient = CharacterConfig.M1_Sword_Damage.Value * Mathf.Max(1, consumedMatches * CharacterConfig.M1_Sword_MatchMultiplier.Value);
            procCoefficient = 1f;
            pushForce = 300f;
            bonusForce = Vector3.zero;
            baseDuration = CharacterConfig.M1_Sword_Duration * (consumedMatches > 0 ? CharacterConfig.M1_Sword_DurationBoostedMultiplier : 1f);

            //0-1 multiplier of baseduration, used to time when the hitbox is out (usually based on the run time of the animation)
            attackStartPercentTime = 0.295f;
            attackEndPercentTime = 0.45f;

            earlyExitPercentTime = 0.8f;

            hitStopDuration = baseDuration * (consumedMatches > 0 ? 0.15f : 0.1f);
            attackRecoil = 0.5f;
            hitHopVelocity = 8 * baseDuration;

            swingSoundString = "";
            playbackRateParam = "Swing.playbackRate";
            muzzleString = "notMercSlashVertical";
            swingEffectPrefab = consumedMatches > 0 ? CharacterAssets.notMercSlashEffectThicc : CharacterAssets.notMercSlashEffect;
            hitEffectPrefab = CharacterAssets.swordHitImpactEffect;

            //interrupt the roll state so you can cut off its movement
            EntityStateMachine.FindByCustomName(gameObject, "Weapon2").SetNextStateToMain();

            //impactSound = MatcherContent.Assets.swordHitSoundEvent.index;

            base.OnEnter();

            if (consumedMatches > 0)
            {
                GetModelTransform().gameObject.GetComponent<MatcherViewController>().RevealSword(duration);
            }

            PlayAttackAnimation();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            StartAimMode(2);
        }

        protected override void PlayAttackAnimation()
        {
            PlayCrossfade("Gesture, Override", "SwordSwingHeavy"/*"heavySwing"*/, playbackRateParam, duration, 0.1f * duration);
            if (characterMotor.isGrounded && !animator.GetBool("isMoving"))
            {
               PlayCrossfade("FullBody, Override", "SwordSwingHeavy", playbackRateParam, duration, 0.1f * duration);
            }
        }

        protected override void PlaySwingEffect()
        {
            base.PlaySwingEffect();
        }

        protected override void OnHitEnemyAuthority()
        {
            base.OnHitEnemyAuthority();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        //add these functions for steppedskilldefs
        //bit advanced so don't worry about this, it's for networking.
        //long story short this syncs a value from authority (current player) to all other clients, so the swingIndex is the same for all machines
        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write(swingIndex);
            writer.Write(consumedMatches);
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            swingIndex = reader.ReadInt32();
            consumedMatches = reader.ReadInt32();
        }
    }
}