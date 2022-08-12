using JoeMod;
using EntityStates;
using RoR2.Orbs;

namespace ModdedEntityStates.Test {
    public class SkillSteal : BaseSkillState {

        private TeslaTrackerComponent _tracker;

        public override void OnEnter() {
            base.OnEnter();

            _tracker = GetComponent<TeslaTrackerComponent>();
            SkillTracker target = _tracker.GetTrackingTarget().healthComponent.GetComponent<SkillTracker>();
            if (target.lastActivatedSkill == null) {

                base.outer.SetNextStateToMain();
                return;
            }

            SkillStealController steal = GetComponent<SkillStealController>();
            if(steal) {
                steal.StolenSkillDef = target.lastActivatedSkill.skillDef;
            }

            SkillOrb skillOrb = new SkillOrb {
                origin = transform.position,
                stolenSkillLocatorObject = target.gameObject,
                skillIndex = target.lastActivatedSkillIndex,
                stealerSkillSlot = activatorSkillSlot,
                target = _tracker.GetTrackingTarget()
            };

            OrbManager.instance.AddOrb(skillOrb);

            base.outer.SetNextStateToMain();
        }
    } 
}