using EntityStates;
using RoR2;
using UnityEngine.Networking;

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

            EntityState state;
            if(mainUpgrade != null)
            {
                state = EntityStateCatalog.InstantiateState(ref mainUpgrade.enterBarricadeState);
            } else
            {
                state = new EnterBarricade();
            }
            bodyMachine.SetNextState(state);

            if (NetworkServer.active)
            {
                characterBody.AddBuff(GIBuffs.armorBuff);
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            bodyMachine.mainStateType = origMain;
            bodyMachine.SetNextStateToMain();

            if (NetworkServer.active)
            {
                characterBody.RemoveBuff(GIBuffs.armorBuff);
            }
        }
    }
}