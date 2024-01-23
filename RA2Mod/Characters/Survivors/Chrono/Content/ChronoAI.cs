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
            GameObject master = Modules.Prefabs.CreateBlankMasterPrefab(bodyPrefab, masterName);

            BaseAI baseAI = master.GetComponent<BaseAI>();
            baseAI.aimVectorDampTime = 0.01f;
            baseAI.aimVectorMaxSpeed = 360;

            //TestShootAi(master);

            TestSprintAI(master);

            ////some fields omitted that aren't commonly changed. will be set to default values
            //AISkillDriver shootDriver = master.AddComponent<AISkillDriver>();
            ////Selection Conditions
            //shootDriver.customName = "Use Secondary Shoot";
            //shootDriver.skillSlot = SkillSlot.Secondary;
            //shootDriver.requireSkillReady = true;
            //shootDriver.minDistance = 0;
            //shootDriver.maxDistance = 25;
            //shootDriver.selectionRequiresTargetLoS = false;
            //shootDriver.selectionRequiresOnGround = false;
            //shootDriver.selectionRequiresAimTarget = false;
            //shootDriver.maxTimesSelected = -1;

            ////Behavior
            //shootDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            //shootDriver.activationRequiresTargetLoS = false;
            //shootDriver.activationRequiresAimTargetLoS = false;
            //shootDriver.activationRequiresAimConfirmation = true;
            //shootDriver.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            //shootDriver.moveInputScale = 1;
            //shootDriver.aimType = AISkillDriver.AimType.AtMoveTarget;
            //shootDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold; 

            //AISkillDriver rollDriver = master.AddComponent<AISkillDriver>();
            ////Selection Conditions
            //rollDriver.customName = "Use Utility Roll";
            //rollDriver.skillSlot = SkillSlot.Utility;
            //rollDriver.requireSkillReady = true;
            //rollDriver.minDistance = 8;
            //rollDriver.maxDistance = 20;
            //rollDriver.selectionRequiresTargetLoS = true;
            //rollDriver.selectionRequiresOnGround = false;
            //rollDriver.selectionRequiresAimTarget = false;
            //rollDriver.maxTimesSelected = -1;

            ////Behavior
            //rollDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            //rollDriver.activationRequiresTargetLoS = false;
            //rollDriver.activationRequiresAimTargetLoS = false;
            //rollDriver.activationRequiresAimConfirmation = false;
            //rollDriver.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            //rollDriver.moveInputScale = 1;
            //rollDriver.aimType = AISkillDriver.AimType.AtMoveTarget;
            //rollDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold;


            //AISkillDriver chaseDriver = master.AddComponent<AISkillDriver>();
            ////Selection Conditions
            //chaseDriver.customName = "Chase";
            //chaseDriver.skillSlot = SkillSlot.None;
            //chaseDriver.requireSkillReady = false;
            //chaseDriver.minDistance = 0;
            //chaseDriver.maxDistance = float.PositiveInfinity;

            ////Behavior
            //chaseDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            //chaseDriver.activationRequiresTargetLoS = false;
            //chaseDriver.activationRequiresAimTargetLoS = false;
            //chaseDriver.activationRequiresAimConfirmation = false;
            //chaseDriver.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            //chaseDriver.moveInputScale = 1;
            //chaseDriver.aimType = AISkillDriver.AimType.AtMoveTarget;
            //chaseDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold;

            //recommend taking these for a spin in game, messing with them in runtimeinspector to get a feel for what they should do at certain ranges and such
        }

        private static void TestSprintAI(GameObject master)
        {
            float sprintclose = 20;

            float sprintFar= 80;

            AISkillDriver dontSprintDriver = master.AddComponent<AISkillDriver>();
            //Selection Conditions
            dontSprintDriver.customName = "dontSprint";
            dontSprintDriver.skillSlot = SkillSlot.None;
            dontSprintDriver.requireSkillReady = false;
            dontSprintDriver.minDistance = 0;
            dontSprintDriver.maxDistance = sprintclose;
            dontSprintDriver.selectionRequiresTargetLoS = false;
            dontSprintDriver.selectionRequiresOnGround = false;
            dontSprintDriver.selectionRequiresAimTarget = false;
            dontSprintDriver.maxTimesSelected = -1;

            //Behavior
            dontSprintDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            dontSprintDriver.activationRequiresTargetLoS = false;
            dontSprintDriver.activationRequiresAimTargetLoS = false;
            dontSprintDriver.activationRequiresAimConfirmation = false;
            dontSprintDriver.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            dontSprintDriver.moveInputScale = 1;
            dontSprintDriver.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            dontSprintDriver.buttonPressType = AISkillDriver.ButtonPressType.Abstain;
            dontSprintDriver.shouldSprint = false;

            AISkillDriver sprintDriver = master.AddComponent<AISkillDriver>();
            //Selection Conditions
            sprintDriver.customName = "sprint";
            sprintDriver.skillSlot = SkillSlot.None;
            sprintDriver.requireSkillReady = false;
            sprintDriver.minDistance = sprintclose;
            sprintDriver.maxDistance = sprintFar;
            sprintDriver.selectionRequiresTargetLoS = false;
            sprintDriver.selectionRequiresOnGround = false;
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
            sprintDriver.nextHighPriorityOverride = dontSprintDriver;
            sprintDriver.ignoreNodeGraph = true;

            AISkillDriver dontSprint2Driver = master.AddComponent<AISkillDriver>();
            //Selection Conditions
            dontSprint2Driver.customName = "dontSprint2";
            dontSprint2Driver.skillSlot = SkillSlot.None;
            dontSprint2Driver.requireSkillReady = false;
            dontSprint2Driver.minDistance = sprintFar;
            dontSprint2Driver.maxDistance = float.PositiveInfinity;
            dontSprint2Driver.selectionRequiresTargetLoS = false;
            dontSprint2Driver.selectionRequiresOnGround = false;
            dontSprint2Driver.selectionRequiresAimTarget = false;
            dontSprint2Driver.maxTimesSelected = -1;

            //Behavior
            dontSprint2Driver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            dontSprint2Driver.activationRequiresTargetLoS = false;
            dontSprint2Driver.activationRequiresAimTargetLoS = false;
            dontSprint2Driver.activationRequiresAimConfirmation = false;
            dontSprint2Driver.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            dontSprint2Driver.moveInputScale = 1;
            dontSprint2Driver.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            dontSprint2Driver.buttonPressType = AISkillDriver.ButtonPressType.Abstain;
            dontSprint2Driver.shouldSprint = false;
        }

        private static void TestShootAi(GameObject master)
        {

            //mouse over these fields for tooltips
            AISkillDriver swingDriver = master.AddComponent<AISkillDriver>();
            //Selection Conditions
            swingDriver.customName = "Use Primary Swing";
            swingDriver.skillSlot = SkillSlot.Primary;
            swingDriver.requiredSkill = null; //usually used when you have skills that override other skillslots like engi harpoons
            swingDriver.requireSkillReady = false; //usually false for primaries
            swingDriver.requireEquipmentReady = false;
            swingDriver.minUserHealthFraction = float.NegativeInfinity;
            swingDriver.maxUserHealthFraction = float.PositiveInfinity;
            swingDriver.minTargetHealthFraction = float.NegativeInfinity;
            swingDriver.maxTargetHealthFraction = float.PositiveInfinity;
            swingDriver.minDistance = 0;
            swingDriver.maxDistance = 10;
            swingDriver.selectionRequiresTargetLoS = false;
            swingDriver.selectionRequiresOnGround = false;
            swingDriver.selectionRequiresAimTarget = false;
            swingDriver.maxTimesSelected = -1;

            //Behavior
            swingDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            swingDriver.activationRequiresTargetLoS = false;
            swingDriver.activationRequiresAimTargetLoS = false;
            swingDriver.activationRequiresAimConfirmation = false;
            swingDriver.movementType = AISkillDriver.MovementType.Stop;
            swingDriver.moveInputScale = 1;
            swingDriver.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            swingDriver.ignoreNodeGraph = false; //will chase relentlessly but be kind of stupid
            swingDriver.shouldSprint = false;
            swingDriver.shouldFireEquipment = false;
            swingDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold;

            //Transition Behavior
            swingDriver.driverUpdateTimerOverride = -1;
            swingDriver.resetCurrentEnemyOnNextDriverSelection = false;
            swingDriver.noRepeat = false;
            swingDriver.nextHighPriorityOverride = null;

            AISkillDriver bombDriver = master.AddComponent<AISkillDriver>();
            //Selection Conditions
            bombDriver.customName = "Use Special bomb";
            bombDriver.skillSlot = SkillSlot.Special;
            bombDriver.requireSkillReady = false;
            bombDriver.minDistance = 10;
            bombDriver.maxDistance = 20;
            bombDriver.selectionRequiresTargetLoS = false;
            bombDriver.selectionRequiresOnGround = false;
            bombDriver.selectionRequiresAimTarget = false;
            bombDriver.maxTimesSelected = -1;

            //Behavior
            bombDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            bombDriver.activationRequiresTargetLoS = false;
            bombDriver.activationRequiresAimTargetLoS = false;
            bombDriver.activationRequiresAimConfirmation = false;
            bombDriver.movementType = AISkillDriver.MovementType.Stop;
            bombDriver.moveInputScale = 1;
            bombDriver.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            bombDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold;

            AISkillDriver bstareDriver = master.AddComponent<AISkillDriver>();
            //Selection Conditions
            bstareDriver.customName = "stare";
            bstareDriver.skillSlot = SkillSlot.None;
            bstareDriver.requireSkillReady = false;
            bstareDriver.minDistance = 20;
            bstareDriver.maxDistance = float.PositiveInfinity;
            bstareDriver.selectionRequiresTargetLoS = false;
            bstareDriver.selectionRequiresOnGround = false;
            bstareDriver.selectionRequiresAimTarget = false;
            bstareDriver.maxTimesSelected = -1;

            //Behavior
            bstareDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            bstareDriver.activationRequiresTargetLoS = false;
            bstareDriver.activationRequiresAimTargetLoS = false;
            bstareDriver.activationRequiresAimConfirmation = false;
            bstareDriver.movementType = AISkillDriver.MovementType.Stop;
            bstareDriver.moveInputScale = 1;
            bstareDriver.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            bstareDriver.buttonPressType = AISkillDriver.ButtonPressType.Abstain;
        }
    }
}
