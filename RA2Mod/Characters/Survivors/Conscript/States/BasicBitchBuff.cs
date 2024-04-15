using RA2Mod.Modules.BaseStates;

namespace RA2Mod.Survivors.Conscript.States
{
    public class BasicBitchBuff : BaseTimedSkillState
    {
        public override float TimedBaseDuration => 0.5f;
        public override float TimedBaseCastStartPercentTime => 1;

        public static float buffDuration => ConscriptConfig.M3_Buff_Duration.Value;

        public override void OnEnter()
        {
            base.OnEnter();
            characterBody.AddTimedBuff(ConscriptBuffs.armorBuff, buffDuration);
            //Util.PlaySound("idk", gameObject);
        }
    }
}
