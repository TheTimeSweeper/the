using MatcherMod.Modules.BaseStates;
using RoR2.Skills;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using Matchmaker.Survivors.Matcher.SkillDefs;

namespace MatcherMod.Survivors.Matcher.SkillStates
{
    public class SlashCombo : BaseMeleeAttack, SteppedSkillDef.IStepSetter, IMatchBoostedState
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
            hitBoxGroupName = swingIndex == 0 ? "swing1" : "swing2";

            damageType = DamageType.Generic;
            damageCoefficient = MatcherContent.StaticValues.swordDamageCoefficient * (1 + consumedMatches);
            procCoefficient = 1f;
            pushForce = 300f;
            bonusForce = Vector3.zero;
            baseDuration = 1f;

            //0-1 multiplier of baseduration, used to time when the hitbox is out (usually based on the run time of the animation)
            attackStartPercentTime = 0.2f;
            attackEndPercentTime = 0.4f;

            earlyExitPercentTime = 0.6f;

            hitStopDuration = 0.012f;
            attackRecoil = 0.5f;
            hitHopVelocity = 4f;

            swingSoundString = "";
            playbackRateParam = "swing.playbackRate";
            //muzzleString = swingIndex == 0 ? "hitboxSwing1" : "hitboxSwing2";
            //swingEffectPrefab = MatcherContent.Assets.swordSwingEffect;
            //hitEffectPrefab = MatcherContent.Assets.swordHitImpactEffect;

            //impactSound = MatcherContent.Assets.swordHitSoundEvent.index;

            base.OnEnter();

            PlayAttackAnimation();
        }

        protected override void PlayAttackAnimation()
        {
            //play a adifferent animation based on what step of the combo you are currently in.
            if (swingIndex == 0)
            {
                PlayCrossfade("Arms, Override", "swing1 v2", playbackRateParam, duration, 0.1f * duration);
            }
            if (swingIndex == 1)
            {
                PlayCrossfade("Arms, Override", "swing2 v2", playbackRateParam, duration, 0.1f * duration);
            }
            //as a challenge, see if you can rewrite this code to be one line.
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
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            swingIndex = reader.ReadInt32();
        }
    }
}