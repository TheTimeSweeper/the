using LfPMod.Modules.BaseStates;
using ModdedEntityStates.Joe;
using System;
using System.Collections.Generic;
using System.Text;

namespace LfPMod.Survivors.Mamluk.SkillStates
{
    public class Axe_M1 : Primary1JumpSwingLand
    {
        protected override void SetSwingValues()
        {
            base.SetSwingValues();

            baseDuration = 0.5f;

        }
    }
    public class Axe_M2Enter : Primary1JumpSwingLand
    {
        protected override void SetSwingValues()
        {
            base.SetSwingValues();

            baseDuration = 1;
        }

        protected override void PlayAttackAnimation()
        {
            base.PlayAnimation("Arms, Override", "jumpSwingLandHeavy", "jumpSwing.playbackRate", this.duration);

        }
    }
}
 