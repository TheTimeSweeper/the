using HellDiverMod.Survivors.HellDiver.SkillDefs;
using RoR2;
using RoR2.Projectile;
using RoR2.Skills;
using UnityEngine;

namespace HellDiverMod.Survivors.HellDiver.Components
{
    public class StratagemProjectile : MonoBehaviour
    {
        [SerializeField]
        private EntityStateMachine stateMachine;

        private StratagemInputController _stratagemController;
        private GenericSkill _stratagemGenericSkill;

        private void OnEnable()
        {
            ProjectileController component = base.GetComponent<ProjectileController>();
            if (component.owner)
            {
                this.AcquireOwner(component);
                return;
            }
            component.onInitialized += this.AcquireOwner;
        }

        private void AcquireOwner(ProjectileController controller)
        {
            controller.onInitialized -= this.AcquireOwner;
            _stratagemController = controller.owner.GetComponent<StratagemInputController>();
            _stratagemGenericSkill = _stratagemController.thrownStratagemQueue.Dequeue();
            stateMachine.commonComponents = new EntityStateMachine.CommonComponentCache(controller.owner);
            stateMachine.commonComponents.transform = transform;
        }

        public void OnStickImpact()
        {
            SkillDef skillDef = _stratagemGenericSkill.skillDef;

            _stratagemGenericSkill.hasExecutedSuccessfully = true;
            //ugly avoid CharacterBody.OnSkillActivated cause it was not intended to be used with extra generic skills woops
            stateMachine.SetInterruptState(skillDef.InstantiateNextState(_stratagemGenericSkill), skillDef.interruptPriority);
            _stratagemGenericSkill.stock -= skillDef.stockToConsume;
        }
    }
}
