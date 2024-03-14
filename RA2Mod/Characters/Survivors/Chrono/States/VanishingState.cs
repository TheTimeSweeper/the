using EntityStates;
using RoR2;
using UnityEngine;

namespace RA2Mod.Survivors.Chrono.SkillStates
{
    public class VanishingState : BaseState
    {
        public float freezeDuration = 1;
        private TemporaryOverlay temporaryOverlay;
        private Animator modelAnimator;
        private float duration;

        public override void OnEnter()
        {
            base.OnEnter();
            Transform modelTransform = base.GetModelTransform();
            if (modelTransform)
            {
                CharacterModel component = modelTransform.GetComponent<CharacterModel>();
                if (component)
                {
                    this.temporaryOverlay = base.gameObject.AddComponent<TemporaryOverlay>();
                    this.temporaryOverlay.duration = this.freezeDuration;
                    this.temporaryOverlay.originalMaterial = ChronoAssets.frozenOverlayMaterial;
                    this.temporaryOverlay.AddToCharacerModel(component);
                }
            }
            this.modelAnimator = base.GetModelAnimator();
            if (this.modelAnimator)
            {
                this.modelAnimator.enabled = false;
                this.duration = this.freezeDuration;
                //EffectManager.SpawnEffect(FrozenState.frozenEffectPrefab, new EffectData
                //{
                //    origin = base.characterBody.corePosition,
                //    scale = (base.characterBody ? base.characterBody.radius : 1f)
                //}, false);
            }
            if (base.rigidbody && !base.rigidbody.isKinematic)
            {
                base.rigidbody.velocity = Vector3.zero;
                if (base.rigidbodyMotor)
                {
                    base.rigidbodyMotor.moveVector = Vector3.zero;
                }
            }
            //base.healthComponent.isInFrozenState = true;
            if (base.characterDirection)
            {
                base.characterDirection.moveVector = base.characterDirection.forward;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.isAuthority && base.fixedAge >= this.duration)
            {
                this.outer.SetNextStateToMain();
            }
        }

        public override void OnExit()
        {
            if (this.modelAnimator)
            {
                this.modelAnimator.enabled = true;
            }
            if (this.temporaryOverlay)
            {
                EntityState.Destroy(this.temporaryOverlay);
            }
            //EffectManager.SpawnEffect(FrozenState.frozenEffectPrefab, new EffectData
            //{
            //    origin = base.characterBody.corePosition,
            //    scale = (base.characterBody ? base.characterBody.radius : 1f)
            //}, false);
            //base.healthComponent.isInFrozenState = false;
            base.OnExit();
        }

    }
}