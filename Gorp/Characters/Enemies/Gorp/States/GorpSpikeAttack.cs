using GorpMod.Enemies.Gorp.Content;
using System.Collections.Generic;
using System.Text;

namespace GorpMod.Characters.Enemies.Gorp.States
{

    public class GorpSpikeAttack : EntityStates.Gup.GupSpikesState
    {
        private int repeats = CharacterConfig.SpikeStateRepeats;
        public override void OnEnter()
        {
            //baseDuration = CharacterConfig.SpikeStateDuration;
            base.OnEnter();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if(repeats > 0 && fixedAge > CharacterConfig.SpikeStateRepeatInterval)
            {
                fixedAge = 0;
                repeats--;
                PlayAnimation();
            }
        }
    }
}
