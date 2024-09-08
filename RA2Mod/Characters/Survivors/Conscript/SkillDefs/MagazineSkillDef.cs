using EntityStates;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using System.Text;
using UnityEngine;

namespace RA2Mod.Survivors.Conscript.SkillDefs
{
    public class MagazineSkillDef : SkillDef
    {
        // Token: 0x040043E8 RID: 17384
        [Header("Reload Parameters")]
        [Tooltip("The reload state to go into, when stock is less than max.")]
        public SerializableEntityStateType reloadState;

        public SerializableEntityStateType hasMagazineReloadState;

        // Token: 0x040043E9 RID: 17385
        [Tooltip("The priority of this reload state.")]
        public InterruptPriority reloadInterruptPriority = InterruptPriority.Skill;

        // Token: 0x040043EA RID: 17386
        [Tooltip("The amount of time to wait between when we COULD reload, and when we actually start")]
        public float graceDuration;

        // Token: 0x02000C13 RID: 3091
        protected class InstanceData : SkillDef.BaseSkillInstanceData
        {
            // Token: 0x040043EB RID: 17387
            public int currentStock;

            // Token: 0x040043EC RID: 17388
            public float graceStopwatch;
        }

        // Token: 0x060045F0 RID: 17904 RVA: 0x00122B28 File Offset: 0x00120D28
        public override SkillDef.BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot)
        {
            return new InstanceData();
        }

        // Token: 0x060045F2 RID: 17906 RVA: 0x00122B30 File Offset: 0x00120D30
        public override void OnFixedUpdate([NotNull] GenericSkill skillSlot, float deltaTime)
        {
            base.OnFixedUpdate(skillSlot, deltaTime);
            InstanceData instanceData = (InstanceData)skillSlot.skillInstanceData;
            instanceData.currentStock = skillSlot.stock;

            if (instanceData.currentStock < this.GetMaxStock(skillSlot))
            {
                if (skillSlot.stateMachine && !skillSlot.stateMachine.HasPendingState() && skillSlot.stateMachine.CanInterruptState(this.reloadInterruptPriority))
                {
                    instanceData.graceStopwatch += deltaTime;
                    if ((instanceData.graceStopwatch >= this.graceDuration || instanceData.currentStock == 0) && skillSlot.cooldownRemaining <= 0)
                    {
                        int magazines = skillSlot.GetComponent<CharacterBody>().GetBuffCount(ConscriptBuffs.magazineBuff);
                        SerializableEntityStateType newReloadState = magazines >= 1? hasMagazineReloadState : this.reloadState;

                        EntityState entityState = EntityStateCatalog.InstantiateState(ref newReloadState);
                        ISkillState skillState;
                        if ((skillState = (entityState as ISkillState)) != null) 
                        {
                            skillState.activatorSkillSlot = skillSlot;
                        }

                        skillSlot.stateMachine.SetNextState(entityState);
                        return;
                    }
                }
                else
                {
                    instanceData.graceStopwatch = 0f;
                }
            }
        }
        // Token: 0x060045F3 RID: 17907 RVA: 0x00122BE2 File Offset: 0x00120DE2
        public override void OnExecute([NotNull] GenericSkill skillSlot)
        {
            base.OnExecute(skillSlot);
            ((InstanceData)skillSlot.skillInstanceData).currentStock = skillSlot.stock;
        }
    }
}
