using RA2Mod.Modules.BaseStates;
using UnityEngine;

namespace RA2Mod.General.States
{
    public class Rest : BaseEmote
    {
        private bool isDesolator; 
        private Animator modelAnimator;

        public override void OnEnter()
        {
            animString = "Sit";
            duration = 0;
            base.OnEnter();

            isDesolator = characterBody.bodyIndex == Survivors.Desolator.DesolatorSurvivor.instance.bodyIndex;

            //todo not this lol
            if (isDesolator)
            {
                PlayAnimation("RadCannonSpin", "CannonSpin");
                modelAnimator = GetModelAnimator();
            }
        }

        public override void Update()
        {
            base.Update();
            if (isDesolator)
            {
                modelAnimator.SetFloat("CannonSpin", modelAnimator.GetFloat("CannonSpinCurve"));
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            if (isDesolator)
            {
                PlayAnimation("RadCannonSpin", "DesolatorIdlePose");
            }
        }
    }
}
