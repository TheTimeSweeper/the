using EntityStates;
using RoR2;
using RoR2.Skills;

namespace RA2Mod.Survivors.Chrono.States {
    public class ChronoCharacterMain : GenericCharacterMain {

        private GenericSkill passiveSkill;

        public override void OnEnter()
        {
            base.OnEnter();
            passiveSkill = skillLocator.FindSkill("LOADOUT_SKILL_PASSIVE");
        }

        public override void HandleMovements() {
            bool canExecute = passiveSkill.CanExecute();
            if (!canExecute)
            {
                sprintInputReceived = false;
            }
            
            base.HandleMovements();
            
            if (sprintInputReceived && canExecute && isAuthority) {

                SkillDef skillDef = passiveSkill.skillDef;

                passiveSkill.hasExecutedSuccessfully = true;
                //ugly avoid CharacterBody.OnSkillActivated cause it was not intended to be used with extra generic skills woops
                passiveSkill.stateMachine.SetInterruptState(skillDef.InstantiateNextState(passiveSkill), skillDef.interruptPriority);
                passiveSkill.stock -= skillDef.stockToConsume;
            }
        }
        
        //public override void HandleMovements() {
        //    modelLocator.autoUpdateModelTransform = !sprintInputReceived;
        //    base.HandleMovements();
        //}
    }
}