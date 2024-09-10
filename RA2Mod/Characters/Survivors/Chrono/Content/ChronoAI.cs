using RA2Mod.Modules;
using RoR2;
using RoR2.CharacterAI;
using System;
using UnityEngine;

namespace RA2Mod.Survivors.Chrono
{
    public static class ChronoAI
    {
        public static void Init(GameObject bodyPrefab, string masterName)
        {
             ContentPacks.asyncLoadCoroutines.Add(Prefabs.CreateBlankMasterPrefabAsync(bodyPrefab, masterName, (result) => {

                GameObject master = result;

                BaseAI baseAI = master.GetComponent<BaseAI>();
                baseAI.aimVectorDampTime = 0.01f;
                baseAI.aimVectorMaxSpeed = 360;

                CreateAI(master);
            }));
        }

        private static void CreateAI(GameObject master)
        {
            float sprintclose = 40;

            float sprintFar= 80;

            AISkillDriver bombDriver = master.AddComponent<AISkillDriver>();
            //Selection Conditions
            bombDriver.customName = "Secondary get close Bomb";
            bombDriver.skillSlot = SkillSlot.Secondary;
            bombDriver.requireSkillReady = true;
            bombDriver.minDistance = 0;
            bombDriver.maxDistance = 10;
            bombDriver.selectionRequiresTargetLoS = false;
            bombDriver.selectionRequiresOnGround = false;
            bombDriver.selectionRequiresAimTarget = false;
            bombDriver.maxTimesSelected = -1;
            //Behavior
            bombDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            bombDriver.activationRequiresTargetLoS = false;
            bombDriver.activationRequiresAimTargetLoS = false;
            bombDriver.activationRequiresAimConfirmation = false;
            bombDriver.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            bombDriver.moveInputScale = 1;
            bombDriver.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            bombDriver.buttonPressType = AISkillDriver.ButtonPressType.TapContinuous;
            bombDriver.ignoreNodeGraph = true;

            //mouse over these fields for tooltips
            AISkillDriver specialDriver = master.AddComponent<AISkillDriver>();
            //Selection Conditions
            specialDriver.customName = "Special";
            specialDriver.skillSlot = SkillSlot.Special;
            specialDriver.requiredSkill = null; //usually used when you have skills that override other skillslots like engi harpoons
            specialDriver.requireSkillReady = true; //usually false for primaries
            specialDriver.requireEquipmentReady = false;
            specialDriver.minUserHealthFraction = float.NegativeInfinity;
            specialDriver.maxUserHealthFraction = float.PositiveInfinity;
            specialDriver.minTargetHealthFraction = float.NegativeInfinity;
            specialDriver.maxTargetHealthFraction = float.PositiveInfinity;
            specialDriver.minDistance = 5;
            specialDriver.maxDistance = 20;
            specialDriver.selectionRequiresTargetLoS = false;
            specialDriver.selectionRequiresOnGround = false;
            specialDriver.selectionRequiresAimTarget = false;
            specialDriver.maxTimesSelected = -1;
            //Behavior
            specialDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            specialDriver.activationRequiresTargetLoS = false;
            specialDriver.activationRequiresAimTargetLoS = false;
            specialDriver.activationRequiresAimConfirmation = true;
            specialDriver.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            specialDriver.moveInputScale = 1;
            specialDriver.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            specialDriver.ignoreNodeGraph = false; //will chase relentlessly but be kind of stupid
            specialDriver.shouldSprint = false;
            specialDriver.shouldFireEquipment = false;
            specialDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold;
            //Transition Behavior
            specialDriver.driverUpdateTimerOverride = -1;
            specialDriver.resetCurrentEnemyOnNextDriverSelection = false;
            specialDriver.noRepeat = false;
            specialDriver.nextHighPriorityOverride = null;

            //mouse over these fields for tooltips
            AISkillDriver primaryCloseDriver = master.AddComponent<AISkillDriver>();
            //Selection Conditions
            primaryCloseDriver.customName = "Primary get distance";
            primaryCloseDriver.skillSlot = SkillSlot.Primary;
            primaryCloseDriver.requiredSkill = null; //usually used when you have skills that override other skillslots like engi harpoons
            primaryCloseDriver.requireSkillReady = false; //usually false for primaries
            primaryCloseDriver.requireEquipmentReady = false;
            primaryCloseDriver.minUserHealthFraction = float.NegativeInfinity;
            primaryCloseDriver.maxUserHealthFraction = float.PositiveInfinity;
            primaryCloseDriver.minTargetHealthFraction = float.NegativeInfinity;
            primaryCloseDriver.maxTargetHealthFraction = float.PositiveInfinity;
            primaryCloseDriver.minDistance = 0;
            primaryCloseDriver.maxDistance = 10;
            primaryCloseDriver.selectionRequiresTargetLoS = false;
            primaryCloseDriver.selectionRequiresOnGround = false;
            primaryCloseDriver.selectionRequiresAimTarget = false;
            primaryCloseDriver.maxTimesSelected = -1;
            //Behavior
            primaryCloseDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            primaryCloseDriver.activationRequiresTargetLoS = false;
            primaryCloseDriver.activationRequiresAimTargetLoS = false;
            primaryCloseDriver.activationRequiresAimConfirmation = true;
            primaryCloseDriver.movementType = AISkillDriver.MovementType.FleeMoveTarget;
            primaryCloseDriver.moveInputScale = 1;
            primaryCloseDriver.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            primaryCloseDriver.ignoreNodeGraph = false; //will chase relentlessly but be kind of stupid
            primaryCloseDriver.shouldSprint = false;
            primaryCloseDriver.shouldFireEquipment = false;
            primaryCloseDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold;
            //Transition Behavior
            primaryCloseDriver.driverUpdateTimerOverride = -1;
            primaryCloseDriver.resetCurrentEnemyOnNextDriverSelection = false;
            primaryCloseDriver.noRepeat = false;
            primaryCloseDriver.nextHighPriorityOverride = bombDriver;

            //mouse over these fields for tooltips
            AISkillDriver primaryDriver = master.AddComponent<AISkillDriver>();
            //Selection Conditions
            primaryDriver.customName = "Primary strafe";
            primaryDriver.skillSlot = SkillSlot.Primary;
            primaryDriver.requiredSkill = null; //usually used when you have skills that override other skillslots like engi harpoons
            primaryDriver.requireSkillReady = false; //usually false for primaries
            primaryDriver.requireEquipmentReady = false;
            primaryDriver.minUserHealthFraction = float.NegativeInfinity;
            primaryDriver.maxUserHealthFraction = float.PositiveInfinity;
            primaryDriver.minTargetHealthFraction = float.NegativeInfinity;
            primaryDriver.maxTargetHealthFraction = float.PositiveInfinity;
            primaryDriver.minDistance = 5;
            primaryDriver.maxDistance = 20;
            primaryDriver.selectionRequiresTargetLoS = false;
            primaryDriver.selectionRequiresOnGround = false;
            primaryDriver.selectionRequiresAimTarget = false;
            primaryDriver.maxTimesSelected = -1;
            //Behavior
            primaryDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            primaryDriver.activationRequiresTargetLoS = false;
            primaryDriver.activationRequiresAimTargetLoS = false;
            primaryDriver.activationRequiresAimConfirmation = true;
            primaryDriver.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            primaryDriver.moveInputScale = 1;
            primaryDriver.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            primaryDriver.ignoreNodeGraph = false; //will chase relentlessly but be kind of stupid
            primaryDriver.shouldSprint = false;
            primaryDriver.shouldFireEquipment = false;
            primaryDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold;
            //Transition Behavior
            primaryDriver.driverUpdateTimerOverride = -1;
            primaryDriver.resetCurrentEnemyOnNextDriverSelection = false;
            primaryDriver.noRepeat = false;
            primaryDriver.nextHighPriorityOverride = null;

            AISkillDriver sprintDriver = master.AddComponent<AISkillDriver>();
            //Selection Conditions
            sprintDriver.customName = "sprint";
            sprintDriver.skillSlot = SkillSlot.None;
            sprintDriver.requireSkillReady = false;
            sprintDriver.minDistance = sprintclose;
            sprintDriver.maxDistance = sprintFar;
            sprintDriver.selectionRequiresTargetLoS = false;
            sprintDriver.selectionRequiresOnGround = true;
            sprintDriver.selectionRequiresAimTarget = true;
            sprintDriver.maxTimesSelected = -1;
            //Behavior
            sprintDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            sprintDriver.activationRequiresTargetLoS = false;
            sprintDriver.activationRequiresAimTargetLoS = false;
            sprintDriver.activationRequiresAimConfirmation = false;
            sprintDriver.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            sprintDriver.moveInputScale = 1;
            sprintDriver.aimType = AISkillDriver.AimType.AtMoveTarget;
            sprintDriver.buttonPressType = AISkillDriver.ButtonPressType.Abstain;
            sprintDriver.shouldSprint = true;
            sprintDriver.nextHighPriorityOverride = null;
            sprintDriver.ignoreNodeGraph = false;
            //transition
            sprintDriver.driverUpdateTimerOverride = -1;
            sprintDriver.resetCurrentEnemyOnNextDriverSelection = false;
            sprintDriver.noRepeat = true;
            sprintDriver.nextHighPriorityOverride = null;

            AISkillDriver chaseNotSPrintDriver = master.AddComponent<AISkillDriver>();
            //Selection Conditions
            chaseNotSPrintDriver.customName = "chaseDontsprint";
            chaseNotSPrintDriver.skillSlot = SkillSlot.None;
            chaseNotSPrintDriver.requireSkillReady = false;
            chaseNotSPrintDriver.minDistance = sprintclose;
            chaseNotSPrintDriver.maxDistance = sprintFar;
            chaseNotSPrintDriver.selectionRequiresTargetLoS = false;
            chaseNotSPrintDriver.selectionRequiresOnGround = false;
            chaseNotSPrintDriver.selectionRequiresAimTarget = true;
            chaseNotSPrintDriver.maxTimesSelected = -1;
            //Behavior
            chaseNotSPrintDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            chaseNotSPrintDriver.activationRequiresTargetLoS = false;
            chaseNotSPrintDriver.activationRequiresAimTargetLoS = false;
            chaseNotSPrintDriver.activationRequiresAimConfirmation = false;
            chaseNotSPrintDriver.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            chaseNotSPrintDriver.moveInputScale = 1;
            chaseNotSPrintDriver.aimType = AISkillDriver.AimType.AtMoveTarget;
            chaseNotSPrintDriver.buttonPressType = AISkillDriver.ButtonPressType.Abstain;
            chaseNotSPrintDriver.shouldSprint = false;
            chaseNotSPrintDriver.nextHighPriorityOverride = null;
            chaseNotSPrintDriver.ignoreNodeGraph = false;
            //transition
            chaseNotSPrintDriver.driverUpdateTimerOverride = -1;
            chaseNotSPrintDriver.resetCurrentEnemyOnNextDriverSelection = false;
            chaseNotSPrintDriver.noRepeat = true;
            chaseNotSPrintDriver.nextHighPriorityOverride = null;


            AISkillDriver stopSprintingFarDriver = master.AddComponent<AISkillDriver>();
            //Selection Conditions
            stopSprintingFarDriver.customName = "dontSprint2";
            stopSprintingFarDriver.skillSlot = SkillSlot.None;
            stopSprintingFarDriver.requireSkillReady = false;
            stopSprintingFarDriver.minDistance = sprintFar;
            stopSprintingFarDriver.maxDistance = float.PositiveInfinity;
            stopSprintingFarDriver.selectionRequiresTargetLoS = false;
            stopSprintingFarDriver.selectionRequiresOnGround = false;
            stopSprintingFarDriver.selectionRequiresAimTarget = false;
            stopSprintingFarDriver.maxTimesSelected = -1;
            //Behavior
            stopSprintingFarDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            stopSprintingFarDriver.activationRequiresTargetLoS = false;
            stopSprintingFarDriver.activationRequiresAimTargetLoS = false;
            stopSprintingFarDriver.activationRequiresAimConfirmation = false;
            stopSprintingFarDriver.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            stopSprintingFarDriver.moveInputScale = 1;
            stopSprintingFarDriver.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            stopSprintingFarDriver.buttonPressType = AISkillDriver.ButtonPressType.Abstain;
            stopSprintingFarDriver.shouldSprint = false;
        }
    }
}
