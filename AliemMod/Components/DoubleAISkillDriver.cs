using RoR2;
using RoR2.CharacterAI;
using UnityEngine;

namespace AliemMod.Components
{
    public class DoubleAISkillDriver : AISkillDriver
    {
        new public SkillSlot skillSlot
        {
            get {
                return _swap ? skillSlot1 : skillSlot2;
            }
            set
            {
                Helpers.LogWarning("trying to set doubleaiskilldriver uh I didnt plan for this");
            }
        }
        public SkillSlot skillSlot1;
        public SkillSlot skillSlot2;
        private bool _swap;
        private float tim = 0.1f;
        void FixedUpdate()
        {
            tim -= Time.fixedDeltaTime;
            if(tim <= 0)
            {
                _swap = !_swap;
                tim += 0.1f;
            }
        }
    }
}
