using EntityStates;
using RoR2.CharacterAI;
using RA2Mod.General.SkillDefs;
using RA2Mod.Survivors.MCV.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace RA2Mod.Survivors.MCV.States
{
    public class MCVSelectTarget : BaseSkillState, IHasSkillDefComponent<MCVUnitTargetTracker>
    {
        public MCVUnitTargetTracker componentFromSkillDef1 { get ; set ; }

        public override void OnEnter()
        {
            base.OnEnter();

            GetComponent<MCVUnitComponent>().selectedUnit = componentFromSkillDef1.GetTrackingTarget().healthComponent.body;

        }
    }

    public class MCVAttackTarget : BaseSkillState, IHasSkillDefComponent<MCVUnitTargetTracker>
    {
        public MCVUnitTargetTracker componentFromSkillDef1 { get; set; }

        public override void OnEnter()
        {
            base.OnEnter();

            BaseAI baseAI = GetComponent<MCVUnitComponent>().selectedUnit.master.gameObject.GetComponent<BaseAI>();

            RoR2.CharacterBody targetBody = componentFromSkillDef1.GetTrackingTarget().healthComponent.body;

            baseAI.currentEnemy.gameObject = targetBody.gameObject;
            baseAI.currentEnemy.bestHurtBox = targetBody.hurtBoxGroup.mainHurtBox;
            baseAI.skillDriverUpdateTimer = 0;
            baseAI.enemyAttention = 100;
            baseAI.targetRefreshTimer = 100;
        }
    }
}
