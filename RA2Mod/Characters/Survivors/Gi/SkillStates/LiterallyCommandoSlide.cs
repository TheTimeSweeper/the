using EntityStates;
using EntityStates.Commando;
using RoR2;
using UnityEngine;

namespace RA2Mod.Survivors.GI.SkillStates
{
    public class LiterallyCommandoSlide : BaseSkillState
    {
        // Token: 0x040016BA RID: 5818
        public static float slideGroundDuration => GIConfig.M3_Slide_Duration.Value;

        public static float slideAirDuration => GIConfig.M3_Slide_AirDuration.Value;
        // Token: 0x040016BC RID: 5820
        public static AnimationCurve forwardSpeedCoefficientCurve => SlideState.forwardSpeedCoefficientCurve;
        // Token: 0x040016BE RID: 5822
        public static string soundString => SlideState.soundString;
        // Token: 0x040016C0 RID: 5824
        public static GameObject slideEffectPrefab => SlideState.slideEffectPrefab;
        // Token: 0x040016C1 RID: 5825
        private Vector3 forwardDirection;
        // Token: 0x040016C2 RID: 5826
        private GameObject slideEffectInstance;
        // Token: 0x040016C3 RID: 5827
        private bool startedStateGrounded;

        private float slideDuration;

        // Token: 0x060011EA RID: 4586 RVA: 0x0004F418 File Offset: 0x0004D618
        public override void OnEnter()
        {
            base.OnEnter();
            Util.PlaySound(soundString, base.gameObject);
            if (base.inputBank && base.characterDirection)
            {
                base.characterDirection.forward = ((base.inputBank.moveVector == Vector3.zero) ? base.characterDirection.forward : base.inputBank.moveVector).normalized;
            }
            if (base.characterMotor)
            {
                this.startedStateGrounded = base.characterMotor.isGrounded;
            }
            base.characterBody.SetSpreadBloom(0f, false);
            if (!this.startedStateGrounded)
            {
                this.PlayAnimation("Body", "Jump");
                Vector3 velocity = base.characterMotor.velocity;
                velocity.y = base.characterBody.jumpPower * GIConfig.M3_Slide_AirJumpMultiplier.Value;
                base.characterMotor.velocity = velocity;
                slideDuration = slideAirDuration;
                return;
            }
            slideDuration = slideGroundDuration;
            base.PlayAnimation("Fullbody, overried", "Dash", "dash.playbackRate", slideDuration);
            if (slideEffectPrefab)
            {
                Transform parent = base.FindModelChild("Root");
                this.slideEffectInstance = UnityEngine.Object.Instantiate<GameObject>(slideEffectPrefab, parent);
            }
        }

        // Token: 0x060011EB RID: 4587 RVA: 0x0004F59C File Offset: 0x0004D79C
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.isAuthority)
            {
                if (base.inputBank && base.characterDirection)
                {
                    base.characterDirection.moveVector = base.inputBank.moveVector;
                    this.forwardDirection = base.characterDirection.forward;
                }
                if (base.characterMotor)
                {                    
                    float forwardSpeed = forwardSpeedCoefficientCurve.Evaluate(base.fixedAge / slideDuration);
                    
                    base.characterMotor.rootMotion += forwardSpeed * this.moveSpeedStat * this.forwardDirection * Time.deltaTime;
                }
                if (base.fixedAge >= slideDuration)
                {
                    this.outer.SetNextStateToMain();
                }
            }
        }

        // Token: 0x060011EC RID: 4588 RVA: 0x0004F69A File Offset: 0x0004D89A
        public override void OnExit()
        {
            this.PlayImpactAnimation();
            if (this.slideEffectInstance)
            {
                EntityState.Destroy(this.slideEffectInstance);
            }
            base.OnExit();
        }

        // Token: 0x060011ED RID: 4589 RVA: 0x0004F6C0 File Offset: 0x0004D8C0
        private void PlayImpactAnimation()
        {
            Animator modelAnimator = base.GetModelAnimator();
            int layerIndex = modelAnimator.GetLayerIndex("Impact");
            if (layerIndex >= 0)
            {
                modelAnimator.SetLayerWeight(layerIndex, 1f);
            }
        }

        // Token: 0x060011EE RID: 4590 RVA: 0x0000EE17 File Offset: 0x0000D017
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}