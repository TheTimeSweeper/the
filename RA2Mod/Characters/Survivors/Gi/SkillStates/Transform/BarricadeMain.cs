using EntityStates;
using UnityEngine;

namespace RA2Mod.Survivors.GI.SkillStates
{
    public class BarricadeMain : GenericCharacterMain
    {
        public override void HandleMovements()
        {
            moveVector = Vector3.zero;
            jumpInputReceived = false;
            sprintInputReceived = false;
            base.HandleMovements();
        }
    }
}