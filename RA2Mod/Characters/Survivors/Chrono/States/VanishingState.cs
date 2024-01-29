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

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
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
            EffectManager.SpawnEffect(FrozenState.frozenEffectPrefab, new EffectData
            {
                origin = base.characterBody.corePosition,
                scale = (base.characterBody ? base.characterBody.radius : 1f)
            }, false);
            base.healthComponent.isInFrozenState = false;
            base.OnExit();
        }

    }
}