using EntityStates;

namespace ModdedEntityStates.Aliem
{
    public class InputBBGun : MashAndHoldInputs
    {
        protected override EntityState newMashState => new FireBBGun { activatorSkillSlot = activatorSkillSlot };

        protected override EntityState newHoldState => new ChargeBBGun { activatorSkillSlot = activatorSkillSlot };
        protected override InterruptPriority holdInterruptPriority => InterruptPriority.Skill;
    }
}