using RoR2;
using RoR2.CharacterAI;
using UnityEngine;

namespace MatcherMod.Survivors.Matcher.MatcherContent
{
    public static class AI
    {
        public static void Init(GameObject bodyPrefab, string masterName)
        {
            Modules.Prefabs.CreateBlankMasterPrefabAsync(bodyPrefab, masterName, (master) =>
            {
                SetupAI(master);
            });
        }

        private static void SetupAI(GameObject master)
        {
            BaseAI baseAI = master.GetComponent<BaseAI>();
            baseAI.aimVectorDampTime = 0.1f;
            baseAI.aimVectorMaxSpeed = 360;

            //AISkillDriver matchDriverFlee = master.AddComponent<AISkillDriver>();
            ////Selection Conditions
            //matchDriverFlee.customName = "Match matches close";
            //matchDriverFlee.skillSlot = SkillSlot.Special;
            //matchDriverFlee.requireSkillReady = true;
            //matchDriverFlee.minDistance = 0;
            //matchDriverFlee.maxDistance = 20;
            //matchDriverFlee.selectionRequiresTargetLoS = false;
            //matchDriverFlee.selectionRequiresOnGround = false;
            //matchDriverFlee.selectionRequiresAimTarget = false;
            //matchDriverFlee.maxTimesSelected = -1;

            ////Behavior
            //matchDriverFlee.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            //matchDriverFlee.activationRequiresTargetLoS = false;
            //matchDriverFlee.activationRequiresAimTargetLoS = false;
            //matchDriverFlee.activationRequiresAimConfirmation = false;
            //matchDriverFlee.movementType = AISkillDriver.MovementType.FleeMoveTarget;
            //matchDriverFlee.moveInputScale = 1;
            //matchDriverFlee.aimType = AISkillDriver.AimType.AtMoveTarget;
            //matchDriverFlee.buttonPressType = AISkillDriver.ButtonPressType.Hold;

            AISkillDriver matchDriverChase = master.AddComponent<AISkillDriver>();
            //Selection Conditions
            matchDriverChase.customName = "match matches far";
            matchDriverChase.skillSlot = SkillSlot.Special;
            matchDriverChase.requireSkillReady = true;
            matchDriverChase.minDistance = 0;
            matchDriverChase.maxDistance = float.NegativeInfinity;
            matchDriverChase.selectionRequiresTargetLoS = false;
            matchDriverChase.selectionRequiresOnGround = false;
            matchDriverChase.selectionRequiresAimTarget = false;
            matchDriverChase.maxTimesSelected = -1;

            //Behavior
            matchDriverChase.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            matchDriverChase.activationRequiresTargetLoS = false;
            matchDriverChase.activationRequiresAimTargetLoS = false;
            matchDriverChase.activationRequiresAimConfirmation = false;
            matchDriverChase.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            matchDriverChase.moveInputScale = 1;
            matchDriverChase.aimType = AISkillDriver.AimType.AtMoveTarget;
            matchDriverChase.buttonPressType = AISkillDriver.ButtonPressType.Hold;

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
            swingDriver.maxDistance = 8;
            swingDriver.selectionRequiresTargetLoS = false;
            swingDriver.selectionRequiresOnGround = false;
            swingDriver.selectionRequiresAimTarget = false;
            swingDriver.maxTimesSelected = -1;

            //Behavior
            swingDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            swingDriver.activationRequiresTargetLoS = false;
            swingDriver.activationRequiresAimTargetLoS = false;
            swingDriver.activationRequiresAimConfirmation = false;
            swingDriver.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            swingDriver.moveInputScale = 1;
            swingDriver.aimType = AISkillDriver.AimType.AtMoveTarget;
            swingDriver.ignoreNodeGraph = false; //will chase relentlessly but be kind of stupid
            swingDriver.shouldSprint = false;
            swingDriver.shouldFireEquipment = false;
            swingDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold;

            //Transition Behavior
            swingDriver.driverUpdateTimerOverride = -1;
            swingDriver.resetCurrentEnemyOnNextDriverSelection = false;
            swingDriver.noRepeat = false;
            swingDriver.nextHighPriorityOverride = null;

            //some fields omitted that aren't commonly changed. will be set to default values
            AISkillDriver shootDriver = master.AddComponent<AISkillDriver>();
            //Selection Conditions
            shootDriver.customName = "Use Secondary Shoot";
            shootDriver.skillSlot = SkillSlot.Secondary;
            shootDriver.requireSkillReady = true;
            shootDriver.minDistance = 0;
            shootDriver.maxDistance = 25;
            shootDriver.selectionRequiresTargetLoS = false;
            shootDriver.selectionRequiresOnGround = false;
            shootDriver.selectionRequiresAimTarget = false;
            shootDriver.maxTimesSelected = -1;

            //Behavior
            shootDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            shootDriver.activationRequiresTargetLoS = false;
            shootDriver.activationRequiresAimTargetLoS = false;
            shootDriver.activationRequiresAimConfirmation = true;
            shootDriver.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            shootDriver.moveInputScale = 1;
            shootDriver.aimType = AISkillDriver.AimType.AtMoveTarget;
            shootDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold;

            AISkillDriver rollCloseDriver = master.AddComponent<AISkillDriver>();
            //Selection Conditions
            rollCloseDriver.customName = "UtilityClose";
            rollCloseDriver.skillSlot = SkillSlot.Utility;
            rollCloseDriver.requireSkillReady = true;
            rollCloseDriver.minDistance = 10;
            rollCloseDriver.maxDistance = 30;
            rollCloseDriver.selectionRequiresTargetLoS = true;
            rollCloseDriver.selectionRequiresOnGround = false;
            rollCloseDriver.selectionRequiresAimTarget = false;
            rollCloseDriver.maxTimesSelected = -1;

            //Behavior
            rollCloseDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            rollCloseDriver.activationRequiresTargetLoS = false;
            rollCloseDriver.activationRequiresAimTargetLoS = false;
            rollCloseDriver.activationRequiresAimConfirmation = false;
            rollCloseDriver.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            rollCloseDriver.moveInputScale = 1;
            rollCloseDriver.aimType = AISkillDriver.AimType.AtMoveTarget;
            rollCloseDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold;


            AISkillDriver chaseDriver = master.AddComponent<AISkillDriver>();
            //Selection Conditions
            chaseDriver.customName = "Chase and match";
            chaseDriver.skillSlot = SkillSlot.Special;
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

            AISkillDriver rollDriver = master.AddComponent<AISkillDriver>();
            //Selection Conditions
            rollDriver.customName = "UtilityCloseDistance";
            rollDriver.skillSlot = SkillSlot.Utility;
            rollDriver.requireSkillReady = true;
            rollDriver.minDistance = 50;
            rollDriver.maxDistance = float.PositiveInfinity;
            rollDriver.selectionRequiresTargetLoS = true;
            rollDriver.selectionRequiresOnGround = false;
            rollDriver.selectionRequiresAimTarget = false;
            rollDriver.maxTimesSelected = -1;

            //Behavior
            rollDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            rollDriver.activationRequiresTargetLoS = false;
            rollDriver.activationRequiresAimTargetLoS = false;
            rollDriver.activationRequiresAimConfirmation = false;
            rollDriver.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            rollDriver.moveInputScale = 1;
            rollDriver.aimType = AISkillDriver.AimType.AtMoveTarget;
            rollDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold;
        }
    }
}
