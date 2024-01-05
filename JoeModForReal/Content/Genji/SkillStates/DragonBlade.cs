using EntityStates;
using JoeModForReal.Content.Survivors;
using ModdedEntityStates.BaseStates;
using RoR2;

namespace ModdedEntityStates.Genji {

    public class DragonBlade : BaseTimedSkillState {

        public static float bladeDuration => GenjiConfig.dragonBladeIntroDuration.Value + GenjiConfig.dragonBladeDuration.Value + 0.5f;

        public override void OnEnter() {
            base.OnEnter();

            InitDurationValues(bladeDuration, GenjiConfig.dragonBladeIntroDuration.Value / bladeDuration, (bladeDuration - 0.5f) / bladeDuration, true);

            EntityStateMachine.FindByCustomName(gameObject, "Body").SetState(EntityStateCatalog.InstantiateState(typeof(DragonBladeIntro)));

            skillLocator.primary.SetSkillOverride(this, GenjiSurvivor.swingBladeSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            //skillLocator.secondary.SetSkillOverride(this, GenjiSurvivor.swingBladeSkillDef, GenericSkill.SkillOverridePriority.Contextual);
        }

        protected override void OnCastEnter() {

            //huh guess nothing
        }

        protected override void OnCastExit() {
            
            EntityStateMachine.FindByCustomName(gameObject, "Body").SetState(EntityStateCatalog.InstantiateState(typeof(DragonBladeOutro)));
            skillLocator.primary.UnsetSkillOverride(this, GenjiSurvivor.swingBladeSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            //skillLocator.secondary.UnsetSkillOverride(this, GenjiSurvivor.swingBladeSkillDef, GenericSkill.SkillOverridePriority.Contextual);
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.Death;
        }
    }

    public class DragonBladeIntro : GenericCharacterMain {

        public override void OnEnter() {
            base.OnEnter();

            base.PlayCrossfade("Fullbody, underried", "ded3", "castDed.playbackRate", 2, GenjiConfig.dragonBladeIntroDuration.Value);
            Util.PlaySound("", gameObject);
        }

        public override bool CanExecuteSkill(GenericSkill skillSlot) {
            return false;
        }

        public override void FixedUpdate() {
            base.FixedUpdate();
            if (fixedAge > GenjiConfig.dragonBladeIntroDuration.Value) {
                outer.SetNextStateToMain();
            }
        }
    }

    public class DragonBladeOutro : GenericCharacterMain {

        public override void OnEnter() {
            base.OnEnter();

            base.PlayCrossfade("Fullbody, underried", "ded3", "castDed.playbackRate", 0.5f, GenjiConfig.dragonBladeIntroDuration.Value);
            Util.PlaySound("", gameObject);
        }

        public override bool CanExecuteSkill(GenericSkill skillSlot) {
            return false;
        }

        public override void FixedUpdate() {
            base.FixedUpdate();
            if (fixedAge > 0.5f) {
                outer.SetNextStateToMain();
            }
        }
    }
}