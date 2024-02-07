using EntityStates;
using RoR2;

namespace RA2Mod.Survivors.GI.SkillStates
{
    public class BarricadeTransform : BaseTransform
    {
        private SerializableEntityStateType origMain;
        private EntityStateMachine bodyMachine;

        public override void OnEnter()
        {
            base.OnEnter();

            bodyMachine = EntityStateMachine.FindByCustomName(gameObject, "Body");
            origMain = bodyMachine.mainStateType;
            bodyMachine.mainStateType = new SerializableEntityStateType(typeof(BarricadeMain));
            bodyMachine.SetNextState(new EnterBarricade());

            characterBody.AddBuff(GIBuffs.armorBuff);
        }

        public override void OnExit()
        {
            base.OnExit();
            bodyMachine.mainStateType = origMain;
            bodyMachine.SetNextStateToMain();

            characterBody.RemoveBuff(GIBuffs.armorBuff);
        }
    }
}