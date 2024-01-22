using EntityStates;
using RoR2;

namespace RA2Mod.Survivors.Chrono.SkillStates {
    public class ChronoCharacterMain : GenericCharacterMain {

        private GenericSkill passiveSkill;

        public override void OnEnter()
        {
            base.OnEnter();
            passiveSkill = skillLocator.FindSkill("LOADOUT_PASSIVE");
        }

        public override void HandleMovements() {
            bool canExecute = passiveSkill.CanExecute();
            if (!canExecute)
            {
                sprintInputReceived = false;
            }
            
            base.HandleMovements();
            
            if (sprintInputReceived && canExecute && isAuthority) {
                passiveSkill.ExecuteIfReady();
            }
        }
        
        //public override void HandleMovements() {
        //    modelLocator.autoUpdateModelTransform = !sprintInputReceived;
        //    base.HandleMovements();
        //}
    }
}