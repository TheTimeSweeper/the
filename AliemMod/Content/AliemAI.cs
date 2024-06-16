using AliemMod.Components;
using AliemMod.Content.Survivors;
using AliemMod.Modules;
using RoR2;
using RoR2.CharacterAI;
using UnityEngine;

namespace AliemMod.Content
{
    public static class AliemAI
    {
        public static void Init(GameObject bodyPrefab, string masterName)
        {
            GameObject master = Prefabs.CreateBlankMasterPrefab(bodyPrefab, masterName);

            BaseAI baseAI = master.GetComponent<BaseAI>();
            baseAI.aimVectorDampTime = 0.1f;
            baseAI.aimVectorMaxSpeed = 360;

            AISkillDriver biteDelay = master.AddComponent<AISkillDriver>();
            //Selection Conditions
            biteDelay.customName = "delayThenBite";
            biteDelay.skillSlot = SkillSlot.Utility;
            biteDelay.requiredSkill = AliemSurvivor.ChompSkillDef;
            biteDelay.requireSkillReady = true;
            biteDelay.requireEquipmentReady = false;
            biteDelay.minUserHealthFraction = float.NegativeInfinity;
            biteDelay.maxUserHealthFraction = float.PositiveInfinity;
            biteDelay.minTargetHealthFraction = float.NegativeInfinity;
            biteDelay.maxTargetHealthFraction = float.PositiveInfinity;
            biteDelay.minDistance = 0;
            biteDelay.maxDistance = 5;
            biteDelay.selectionRequiresTargetLoS = false;
            biteDelay.selectionRequiresOnGround = false;
            biteDelay.selectionRequiresAimTarget = false;
            biteDelay.maxTimesSelected = -1;

            //Behavior
            biteDelay.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            biteDelay.activationRequiresTargetLoS = false;
            biteDelay.activationRequiresAimTargetLoS = false;
            biteDelay.activationRequiresAimConfirmation = false;
            biteDelay.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            biteDelay.moveInputScale = 1;
            biteDelay.aimType = AISkillDriver.AimType.AtMoveTarget;
            biteDelay.ignoreNodeGraph = true;
            biteDelay.shouldSprint = false;
            biteDelay.shouldFireEquipment = false;
            biteDelay.buttonPressType = AISkillDriver.ButtonPressType.Abstain;

            //Transition Behavior
            biteDelay.driverUpdateTimerOverride = 1;
            biteDelay.resetCurrentEnemyOnNextDriverSelection = false;
            biteDelay.noRepeat = true;
            biteDelay.nextHighPriorityOverride = null;

            AISkillDriver bite = master.AddComponent<AISkillDriver>();
            //Selection Conditions
            bite.customName = "bite";
            bite.skillSlot = SkillSlot.Utility;
            bite.requiredSkill = AliemSurvivor.ChompSkillDef;
            bite.requireSkillReady = true;
            bite.requireEquipmentReady = false;
            bite.minUserHealthFraction = float.NegativeInfinity;
            bite.maxUserHealthFraction = float.PositiveInfinity;
            bite.minTargetHealthFraction = float.NegativeInfinity;
            bite.maxTargetHealthFraction = float.PositiveInfinity;
            bite.minDistance = 0;
            bite.maxDistance = float.PositiveInfinity;
            bite.selectionRequiresTargetLoS = false;
            bite.selectionRequiresOnGround = false;
            bite.selectionRequiresAimTarget = false;
            bite.maxTimesSelected = -1;

            //Behavior
            bite.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            bite.activationRequiresTargetLoS = false;
            bite.activationRequiresAimTargetLoS = false;
            bite.activationRequiresAimConfirmation = false;
            bite.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            bite.moveInputScale = 1;
            bite.aimType = AISkillDriver.AimType.AtMoveTarget;
            bite.ignoreNodeGraph = true;
            bite.shouldSprint = false;
            bite.shouldFireEquipment = false;
            bite.buttonPressType = AISkillDriver.ButtonPressType.TapContinuous;

            //Transition Behavior
            bite.driverUpdateTimerOverride = -1;
            bite.resetCurrentEnemyOnNextDriverSelection = false;
            bite.noRepeat = true;
            bite.nextHighPriorityOverride = biteDelay;

            AISkillDriver diveNear = master.AddComponent<AISkillDriver>();
            //Selection Conditions
            diveNear.customName = "Dive near";
            diveNear.skillSlot = SkillSlot.Utility;
            diveNear.requiredSkill = null;
            diveNear.requireSkillReady = true;
            diveNear.requireEquipmentReady = false;
            diveNear.minUserHealthFraction = float.NegativeInfinity;
            diveNear.maxUserHealthFraction = 0.5f;
            diveNear.minTargetHealthFraction = float.NegativeInfinity;
            diveNear.maxTargetHealthFraction = float.PositiveInfinity;
            diveNear.minDistance = 0;
            diveNear.maxDistance = 10;
            diveNear.selectionRequiresTargetLoS = true;
            diveNear.selectionRequiresOnGround = false;
            diveNear.selectionRequiresAimTarget = true;
            diveNear.maxTimesSelected = -1;

            //Behavior
            diveNear.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            diveNear.activationRequiresTargetLoS = true;
            diveNear.activationRequiresAimTargetLoS = true;
            diveNear.activationRequiresAimConfirmation = true;
            diveNear.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            diveNear.moveInputScale = 1;
            diveNear.aimType = AISkillDriver.AimType.AtMoveTarget;
            diveNear.ignoreNodeGraph = true;
            diveNear.shouldSprint = false;
            diveNear.shouldFireEquipment = false;
            diveNear.buttonPressType = AISkillDriver.ButtonPressType.Hold;

            //Transition Behavior
            diveNear.driverUpdateTimerOverride = -1;
            diveNear.resetCurrentEnemyOnNextDriverSelection = false;
            diveNear.noRepeat = false;
            diveNear.nextHighPriorityOverride = null;

            AISkillDriver socialDistance = master.AddComponent<AISkillDriver>();
            //Selection Conditions
            socialDistance.customName = "Social Distance";
            socialDistance.skillSlot = SkillSlot.Primary;
            socialDistance.requireSkillReady = false;
            socialDistance.minDistance = 0;
            socialDistance.maxDistance = 11;

            //Behavior
            socialDistance.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            socialDistance.activationRequiresTargetLoS = false;
            socialDistance.activationRequiresAimTargetLoS = false;
            socialDistance.activationRequiresAimConfirmation = false;
            socialDistance.movementType = AISkillDriver.MovementType.FleeMoveTarget;
            socialDistance.moveInputScale = 1;
            socialDistance.aimType = AISkillDriver.AimType.AtMoveTarget;
            socialDistance.buttonPressType = AISkillDriver.ButtonPressType.TapContinuous;

            //AISkillDriver secondaryStrafe = master.AddComponent<AISkillDriver>();
            ////Selection Conditions
            //secondaryStrafe.customName = "Use Secondary Strafe";
            //secondaryStrafe.skillSlot = SkillSlot.Secondary;
            //secondaryStrafe.requiredSkill = null; //usually used when you have skills that override other skillslots like engi harpoons
            //secondaryStrafe.requireSkillReady = true; //usually false for primaries
            //secondaryStrafe.requireEquipmentReady = false;
            //secondaryStrafe.minUserHealthFraction = float.NegativeInfinity;
            //secondaryStrafe.maxUserHealthFraction = float.PositiveInfinity;
            //secondaryStrafe.minTargetHealthFraction = float.NegativeInfinity;
            //secondaryStrafe.maxTargetHealthFraction = float.PositiveInfinity;
            //secondaryStrafe.minDistance = 10;
            //secondaryStrafe.maxDistance = 30;
            //secondaryStrafe.selectionRequiresTargetLoS = true;
            //secondaryStrafe.selectionRequiresOnGround = false;
            //secondaryStrafe.selectionRequiresAimTarget = true;
            //secondaryStrafe.maxTimesSelected = -1;

            ////Behavior
            //secondaryStrafe.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            //secondaryStrafe.activationRequiresTargetLoS = true;
            //secondaryStrafe.activationRequiresAimTargetLoS = true;
            //secondaryStrafe.activationRequiresAimConfirmation = true;
            //secondaryStrafe.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            //secondaryStrafe.moveInputScale = 1;
            //secondaryStrafe.aimType = AISkillDriver.AimType.AtMoveTarget;
            //secondaryStrafe.ignoreNodeGraph = false; //will chase relentlessly but be kind of stupid
            //secondaryStrafe.shouldSprint = false;
            //secondaryStrafe.shouldFireEquipment = false;
            //secondaryStrafe.buttonPressType = AISkillDriver.ButtonPressType.Hold;

            ////Transition Behavior
            //secondaryStrafe.driverUpdateTimerOverride = -1;
            //secondaryStrafe.resetCurrentEnemyOnNextDriverSelection = false;
            //secondaryStrafe.noRepeat = false;
            //secondaryStrafe.nextHighPriorityOverride = null;

            //mouse over these fields for tooltips
            DoubleAISkillDriver primaryStrafe = master.AddComponent<DoubleAISkillDriver>();
            //Selection Conditions
            primaryStrafe.customName = "Use Primary Strafe";
            //primaryStrafe.skillSlot = SkillSlot.Primary;
            primaryStrafe.skillSlot1 = SkillSlot.Primary;
            primaryStrafe.skillSlot2 = SkillSlot.Secondary;
            primaryStrafe.requiredSkill = null; //usually used when you have skills that override other skillslots like engi harpoons
            primaryStrafe.requireSkillReady = false; //usually false for primaries
            primaryStrafe.requireEquipmentReady = false;
            primaryStrafe.minUserHealthFraction = float.NegativeInfinity;
            primaryStrafe.maxUserHealthFraction = float.PositiveInfinity;
            primaryStrafe.minTargetHealthFraction = float.NegativeInfinity;
            primaryStrafe.maxTargetHealthFraction = float.PositiveInfinity;
            primaryStrafe.minDistance = 10;
            primaryStrafe.maxDistance = 30;
            primaryStrafe.selectionRequiresTargetLoS = true;
            primaryStrafe.selectionRequiresOnGround = false;
            primaryStrafe.selectionRequiresAimTarget = true;
            primaryStrafe.maxTimesSelected = -1;

            //Behavior
            primaryStrafe.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            primaryStrafe.activationRequiresTargetLoS = true;
            primaryStrafe.activationRequiresAimTargetLoS = true;
            primaryStrafe.activationRequiresAimConfirmation = true;
            primaryStrafe.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            primaryStrafe.moveInputScale = 1;
            primaryStrafe.aimType = AISkillDriver.AimType.AtMoveTarget;
            primaryStrafe.ignoreNodeGraph = false; //will chase relentlessly but be kind of stupid
            primaryStrafe.shouldSprint = false; 
            primaryStrafe.shouldFireEquipment = false;
            primaryStrafe.buttonPressType = AISkillDriver.ButtonPressType.TapContinuous; 

            //Transition Behavior
            primaryStrafe.driverUpdateTimerOverride = -1;
            primaryStrafe.resetCurrentEnemyOnNextDriverSelection = false;
            primaryStrafe.noRepeat = false;
            primaryStrafe.nextHighPriorityOverride = null;

            AISkillDriver diveCloseDistance = master.AddComponent<AISkillDriver>();
            //Selection Conditions
            diveCloseDistance.customName = "Dive CLose Distance";
            diveCloseDistance.skillSlot = SkillSlot.Utility;
            diveCloseDistance.requiredSkill = null;
            diveCloseDistance.requireSkillReady = true;
            diveCloseDistance.requireEquipmentReady = false;
            diveCloseDistance.minUserHealthFraction = float.NegativeInfinity;
            diveCloseDistance.maxUserHealthFraction = float.PositiveInfinity;
            diveCloseDistance.minTargetHealthFraction = float.NegativeInfinity;
            diveCloseDistance.maxTargetHealthFraction = float.PositiveInfinity;
            diveCloseDistance.minDistance = 35;
            diveCloseDistance.maxDistance = float.PositiveInfinity;
            diveCloseDistance.selectionRequiresTargetLoS = false;
            diveCloseDistance.selectionRequiresOnGround = false;
            diveCloseDistance.selectionRequiresAimTarget = true;
            diveCloseDistance.maxTimesSelected = -1;

            //Behavior
            diveCloseDistance.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            diveCloseDistance.activationRequiresTargetLoS = false;
            diveCloseDistance.activationRequiresAimTargetLoS = false;
            diveCloseDistance.activationRequiresAimConfirmation = false;
            diveCloseDistance.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            diveCloseDistance.moveInputScale = 1;
            diveCloseDistance.aimType = AISkillDriver.AimType.AtMoveTarget;
            diveCloseDistance.ignoreNodeGraph = false; 
            diveCloseDistance.shouldSprint = false;
            diveCloseDistance.shouldFireEquipment = false;
            diveCloseDistance.buttonPressType = AISkillDriver.ButtonPressType.TapContinuous;

            //Transition Behavior
            diveCloseDistance.driverUpdateTimerOverride = -1;
            diveCloseDistance.resetCurrentEnemyOnNextDriverSelection = false;
            diveCloseDistance.noRepeat = false;
            diveCloseDistance.nextHighPriorityOverride = null;

            AISkillDriver bombDriver = master.AddComponent<AISkillDriver>();
            //Selection Conditions
            bombDriver.customName = "Use Special bomb";
            bombDriver.skillSlot = SkillSlot.Special;
            bombDriver.requireSkillReady = true;
            bombDriver.minDistance = 0;
            bombDriver.maxDistance = 30;
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
            bombDriver.aimType = AISkillDriver.AimType.AtMoveTarget;
            bombDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold;

            AISkillDriver chaseDriver = master.AddComponent<AISkillDriver>();
            //Selection Conditions
            chaseDriver.customName = "Chase";
            chaseDriver.skillSlot = SkillSlot.None;
            chaseDriver.requireSkillReady = false;
            chaseDriver.minDistance = 0;
            chaseDriver.maxDistance = float.PositiveInfinity;

            //Behavior
            chaseDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            chaseDriver.activationRequiresTargetLoS = false;
            chaseDriver.activationRequiresAimTargetLoS = false;
            chaseDriver.activationRequiresAimConfirmation = false;
            chaseDriver.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            chaseDriver.moveInputScale = 1;
            chaseDriver.aimType = AISkillDriver.AimType.AtMoveTarget;
            chaseDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold;

            //recommend taking these for a spin in game, messing with them in runtimeinspector to get a feel for what they should do at certain ranges and such
        }
    }
}
