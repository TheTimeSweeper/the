using EntityStates;
using RA2Mod.Survivors.Chrono.Components;
using RoR2;
using UnityEngine;

namespace RA2Mod.Survivors.Chrono.States
{
    public class PhaseState : BaseSkillState {

        public float windDownTime = 0.5f;
        public PhaseIndicatorController controller;
        private TemporaryOverlayInstance temporaryOverlay;
        private CharacterModel characterModel;
        private Ray aimRay;

        public override void OnEnter() {
            base.OnEnter();
            Util.PlaySound("Play_ChronoMove", gameObject);
            characterBody.isSprinting = false;
            
            controller?.UpdateIndicatorActive(true);
            GetModelAnimator().enabled = false;

            aimRay = GetAimRay();
            StartAimMode(aimRay, windDownTime);

            if (characterDirection)
            {
                characterDirection.forward = aimRay.direction;
            }
            
            Transform modelTransform = base.GetModelTransform();
            if (modelTransform)
            {
                characterModel = modelTransform.GetComponent<CharacterModel>();
                if (characterModel)
                {
                    //characterModel.invisibilityCount++;
                    temporaryOverlay = TemporaryOverlayManager.AddOverlay(gameObject);
                    this.temporaryOverlay.duration = windDownTime;
                    this.temporaryOverlay.originalMaterial = ChronoAssets.frozenOverlayMaterial;
                    this.temporaryOverlay.AddToCharacterModel(characterModel);
                }
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            controller?.UpdateIndicatorActive(false);
            GetModelAnimator().enabled = true;
            if (this.temporaryOverlay != null)
            {
                this.temporaryOverlay.Destroy();
            }

            StartAimMode(1);

            //if (characterModel)
            //{
            //    characterModel.invisibilityCount--;
            //}
        }

        public override void FixedUpdate() {
            base.FixedUpdate();

            //if (characterDirection)
            //{
            //    characterDirection.forward = aimDirection;
            //}

            controller?.UpdateIndicatorFill((windDownTime - fixedAge) / windDownTime);

            if (fixedAge > windDownTime) {
                outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.Frozen;
        }
    }
}