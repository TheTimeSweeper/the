﻿using R2API;
using RoR2;
using System.Collections.Generic;
using UnityEngine;
using Modules.Characters;

namespace Modules {
    // module for creating body prefabs and whatnot
    // recommended to simply avoid touching this unless you REALLY need to

    internal static class Prefabs
    {
        // cache this just to give our ragdolls the same physic material as vanilla stuff
        private static PhysicMaterial ragdollMaterial;

        public static GameObject CreateDisplayPrefab(string displayModelName, GameObject prefab, BodyInfo bodyInfo)
        {
            GameObject model = Asset.LoadSurvivorModel(displayModelName);

            CharacterModel characterModel = model.GetComponent<CharacterModel>();
            if (!characterModel) {
                characterModel = model.AddComponent<CharacterModel>();
            }
            characterModel.baseRendererInfos = prefab.GetComponentInChildren<CharacterModel>().baseRendererInfos;

            Modules.Asset.ConvertAllRenderersToHopooShader(model);

            return model.gameObject;
        }

        public static GameObject CreateBodyPrefab(string bodyName, string modelName, BodyInfo bodyInfo)
        {
            if (!RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/" + bodyInfo.bodyNameToClone + "Body"))
            {
                Debug.LogError(bodyInfo.bodyNameToClone + "Body is not a valid body, character creation failed");
                return null;
            }

            GameObject newBodyPrefab = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/" + bodyInfo.bodyNameToClone + "Body"), bodyName);

            Transform modelBaseTransform = null;
            GameObject newModel = null;
            if (modelName != "mdl")
            {
                newModel = Asset.LoadSurvivorModel(modelName);
                //if load fails, just use body from the clone
                if (newModel == null) 
                    newModel = newBodyPrefab.GetComponentInChildren<CharacterModel>().gameObject;

                    modelBaseTransform = AddCharacterModelToSurvivorBody(newBodyPrefab, newModel.transform, bodyInfo);
            }

            #region CharacterBody
            CharacterBody bodyComponent = newBodyPrefab.GetComponent<CharacterBody>();
            //identity
            bodyComponent.name = bodyInfo.bodyName;
            bodyComponent.baseNameToken = bodyInfo.bodyNameToken;
            bodyComponent.subtitleNameToken = bodyInfo.subtitleNameToken;
            bodyComponent.portraitIcon = bodyInfo.characterPortrait;
            bodyComponent.bodyColor = bodyInfo.bodyColor;

            bodyComponent._defaultCrosshairPrefab = bodyInfo.crosshair;
            bodyComponent.hideCrosshair = false;
            bodyComponent.preferredPodPrefab = bodyInfo.podPrefab;
            
            //stats
            bodyComponent.baseMaxHealth = bodyInfo.maxHealth;
            bodyComponent.baseRegen = bodyInfo.healthRegen;
            bodyComponent.baseArmor = bodyInfo.armor;
            bodyComponent.baseMaxShield = bodyInfo.shield;

            bodyComponent.baseDamage = bodyInfo.damage;
            bodyComponent.baseAttackSpeed = bodyInfo.attackSpeed;
            bodyComponent.baseCrit = bodyInfo.crit;

            bodyComponent.baseMoveSpeed = bodyInfo.moveSpeed;
            bodyComponent.baseJumpPower = bodyInfo.jumpPower;

            //level stats
            bodyComponent.autoCalculateLevelStats = bodyInfo.autoCalculateLevelStats;

            if (bodyInfo.autoCalculateLevelStats) {

                bodyComponent.levelMaxHealth = Mathf.Round(bodyComponent.baseMaxHealth * 0.3f);
                bodyComponent.levelMaxShield = Mathf.Round(bodyComponent.baseMaxShield * 0.3f);
                bodyComponent.levelRegen = bodyComponent.baseRegen * 0.2f;

                bodyComponent.levelMoveSpeed = 0f;
                bodyComponent.levelJumpPower = 0f;

                bodyComponent.levelDamage = bodyComponent.baseDamage * 0.2f;
                bodyComponent.levelAttackSpeed = 0f;
                bodyComponent.levelCrit = 0f;

                bodyComponent.levelArmor = 0f;

            } else {

                bodyComponent.levelMaxHealth = bodyInfo.healthGrowth;
                bodyComponent.levelMaxShield = bodyInfo.shieldGrowth;
                bodyComponent.levelRegen = bodyInfo.regenGrowth;

                bodyComponent.levelMoveSpeed = bodyInfo.moveSpeedGrowth;
                bodyComponent.levelJumpPower = bodyInfo.jumpPowerGrowth;

                bodyComponent.levelDamage = bodyInfo.damageGrowth;
                bodyComponent.levelAttackSpeed = bodyInfo.attackSpeedGrowth;
                bodyComponent.levelCrit = bodyInfo.critGrowth;

                bodyComponent.levelArmor = bodyInfo.armorGrowth;
            }
            //other
            bodyComponent.baseAcceleration = bodyInfo.acceleration;

            bodyComponent.baseJumpCount = bodyInfo.jumpCount;

            bodyComponent.sprintingSpeedMultiplier = 1.45f;

            bodyComponent.bodyFlags = bodyInfo.bodyFlags;
            bodyComponent.rootMotionInMainState = false;

            bodyComponent.hullClassification = HullClassification.Human;

            bodyComponent.isChampion = false;
            #endregion

            SetupCameraTargetParams(newBodyPrefab, bodyInfo);
            SetupModelLocator(newBodyPrefab, modelBaseTransform, newModel.transform);
            //SetupRigidbody(newPrefab);
            SetupCapsuleCollider(newBodyPrefab);
            SetupHurtBoxGroup(newBodyPrefab, newModel);

            SetupAimAnimator(newBodyPrefab, newModel);

            //funny hack
            //TODO: properly null check these functions
            if (bodyInfo.bodyNameToClone != "EngiTurret") {
                if (modelBaseTransform != null) SetupCharacterDirection(newBodyPrefab, modelBaseTransform, newModel.transform);
                    SetupFootstepController(newModel);
                    SetupRagdoll(newModel);
            }

            Modules.Content.AddCharacterBodyPrefab(newBodyPrefab);

            return newBodyPrefab;
        }

        public static void CreateGenericDoppelganger(GameObject bodyPrefab, string masterName, string masterToCopy)
        {
            GameObject newMaster = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterMasters/" + masterToCopy + "MonsterMaster"), masterName, true);
            newMaster.GetComponent<CharacterMaster>().bodyPrefab = bodyPrefab;

            Modules.Content.AddMasterPrefab(newMaster);
        }

        #region ModelSetup

        private static Transform AddCharacterModelToSurvivorBody(GameObject bodyPrefab, Transform modelTransform, BodyInfo bodyInfo) 
        {
            for (int i = bodyPrefab.transform.childCount - 1; i >= 0; i--) {

                Object.DestroyImmediate(bodyPrefab.transform.GetChild(i).gameObject);
            }

            Transform modelBase = new GameObject("ModelBase").transform;
            modelBase.parent = bodyPrefab.transform;
            modelBase.localPosition = bodyInfo.modelBasePosition;
            modelBase.localRotation = Quaternion.identity;

            modelTransform.parent = modelBase.transform;
            modelTransform.localPosition = Vector3.zero;
            modelTransform.localRotation = Quaternion.identity;

            GameObject cameraPivot = new GameObject("CameraPivot");
            cameraPivot.transform.parent = bodyPrefab.transform;
            cameraPivot.transform.localPosition = bodyInfo.cameraPivotPosition;
            cameraPivot.transform.localRotation = Quaternion.identity;

            GameObject aimOrigin = new GameObject("AimOrigin");
            aimOrigin.transform.parent = bodyPrefab.transform;
            aimOrigin.transform.localPosition = bodyInfo.aimOriginPosition;
            aimOrigin.transform.localRotation = Quaternion.identity;
            bodyPrefab.GetComponent<CharacterBody>().aimOriginTransform = aimOrigin.transform;

            return modelBase.transform;
        }
        public static CharacterModel SetupCharacterModel(GameObject prefab) => SetupCharacterModel(prefab, null);
        public static CharacterModel SetupCharacterModel(GameObject prefab, CustomRendererInfo[] customInfos) {

            CharacterModel characterModel = prefab.GetComponent<ModelLocator>().modelTransform.gameObject.GetComponent<CharacterModel>();
            bool preattached = characterModel != null;
            if (!preattached)
                characterModel = prefab.GetComponent<ModelLocator>().modelTransform.gameObject.AddComponent<CharacterModel>();

            characterModel.body = prefab.GetComponent<CharacterBody>();

            characterModel.autoPopulateLightInfos = true;
            characterModel.invisibilityCount = 0;
            characterModel.temporaryOverlays = new List<TemporaryOverlayInstance>();

            if (!preattached) {
                SetupCustomRendererInfos(characterModel, customInfos);
            }
            else {
                SetupPreAttachedRendererInfos(characterModel);
            }
            return characterModel;
        }

        public static void SetupPreAttachedRendererInfos(CharacterModel characterModel) {
            for (int i = 0; i < characterModel.baseRendererInfos.Length; i++) {
                if (characterModel.baseRendererInfos[i].defaultMaterial == null)
                    characterModel.baseRendererInfos[i].defaultMaterial = characterModel.baseRendererInfos[i].renderer.sharedMaterial;
                characterModel.baseRendererInfos[i].defaultMaterial.SetHotpooMaterial();
            }
        }

        public static void SetupCustomRendererInfos(CharacterModel characterModel, CustomRendererInfo[] customInfos) {

            ChildLocator childLocator = characterModel.GetComponent<ChildLocator>();
            if (!childLocator) {
                Debug.LogError("Failed CharacterModel setup: ChildLocator component does not exist on the model");
                return;
            }

            List<CharacterModel.RendererInfo> rendererInfos = new List<CharacterModel.RendererInfo>();

            for (int i = 0; i < customInfos.Length; i++) {
                if (!childLocator.FindChild(customInfos[i].childName)) {
                    Debug.LogError("Trying to add a RendererInfo for a renderer that does not exist: " + customInfos[i].childName);
                } else {
                    Renderer rend = childLocator.FindChild(customInfos[i].childName).GetComponent<Renderer>();
                    if (rend) {

                        Material mat = customInfos[i].material;

                        if (mat == null) {
                            mat = rend.material.SetHotpooMaterial();
                        }

                        rendererInfos.Add(new CharacterModel.RendererInfo {
                            renderer = rend,
                            defaultMaterial = mat,
                            ignoreOverlays = customInfos[i].ignoreOverlays,
                            defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On
                        });
                    }
                }
            }

            characterModel.baseRendererInfos = rendererInfos.ToArray();
        }
        #endregion

        #region ComponentSetup
        private static void SetupCharacterDirection(GameObject prefab, Transform modelBaseTransform, Transform modelTransform)
        {
            if (!prefab.GetComponent<CharacterDirection>())
                return;

            CharacterDirection characterDirection = prefab.GetComponent<CharacterDirection>();
            characterDirection.targetTransform = modelBaseTransform;
            characterDirection.overrideAnimatorForwardTransform = null;
            characterDirection.rootMotionAccumulator = null;
            characterDirection.modelAnimator = modelTransform.GetComponent<Animator>();
            characterDirection.driveFromRootRotation = false;
            characterDirection.turnSpeed = 720f;
        }

        private static void SetupCameraTargetParams(GameObject prefab, BodyInfo bodyInfo)
        {
            CameraTargetParams cameraTargetParams = prefab.GetComponent<CameraTargetParams>();
            cameraTargetParams.cameraParams = bodyInfo.cameraParams;
            cameraTargetParams.cameraPivotTransform = prefab.transform.Find("CameraPivot");
            //cameraTargetParams.aimMode = CameraTargetParams.AimType.Standard;
        }

        private static void SetupModelLocator(GameObject prefab, Transform modelBaseTransform, Transform modelTransform)
        {
            ModelLocator modelLocator = prefab.GetComponent<ModelLocator>();
            modelLocator.modelTransform = modelTransform;
            modelLocator.modelBaseTransform = modelBaseTransform;
        }

        //private static void SetupRigidbody(GameObject prefab)
        //{
        //    Rigidbody rigidbody = prefab.GetComponent<Rigidbody>();
        //    rigidbody.mass = 100f;
        //}

        private static void SetupCapsuleCollider(GameObject prefab) {
            CapsuleCollider capsuleCollider = prefab.GetComponent<CapsuleCollider>();
            capsuleCollider.center = new Vector3(0f, 0f, 0f);
            capsuleCollider.radius = 0.5f;
            capsuleCollider.height = 1.82f;
            capsuleCollider.direction = 1;
        }

        public static void SetupHurtBoxGroup(GameObject bodyPrefab, GameObject bodyModel) {

            HealthComponent healthComponent = bodyPrefab.GetComponent<HealthComponent>();
            HurtBoxGroup hurtboxGroup = bodyModel.GetComponent<HurtBoxGroup>();

            if (hurtboxGroup != null) {
                hurtboxGroup.mainHurtBox.healthComponent = healthComponent;
                for (int i = 0; i < hurtboxGroup.hurtBoxes.Length; i++) {
                    hurtboxGroup.hurtBoxes[i].healthComponent = healthComponent;
                }
            } else {
                SetupMainHurtboxesFromChildLocator(bodyPrefab, bodyModel);
            }

        }

        private static void SetupMainHurtboxesFromChildLocator(GameObject prefab, GameObject model)
        {
            ChildLocator childLocator = model.GetComponent<ChildLocator>();

            if (!childLocator.FindChild("MainHurtbox"))
            {
                Helpers.LogWarning("Could not set up main hurtbox: make sure you have a transform pair in your prefab's ChildLocator component called 'MainHurtbox'");
                return;
            }

            HurtBoxGroup hurtBoxGroup = model.AddComponent<HurtBoxGroup>();

            HurtBox headHurtbox = null;
            GameObject headHurtboxObject = childLocator.FindChildGameObject("HeadHurtbox");
            if (headHurtboxObject) {
                headHurtbox = headHurtboxObject.AddComponent<HurtBox>();
                headHurtbox.gameObject.layer = LayerIndex.entityPrecise.intVal;
                headHurtbox.healthComponent = prefab.GetComponent<HealthComponent>();
                headHurtbox.isBullseye = false;
                headHurtbox.isSniperTarget = true;
                headHurtbox.damageModifier = HurtBox.DamageModifier.Normal;
                headHurtbox.hurtBoxGroup = hurtBoxGroup;
                headHurtbox.indexInGroup = 1;
            }

            HurtBox mainHurtbox = childLocator.FindChild("MainHurtbox").gameObject.AddComponent<HurtBox>();
            mainHurtbox.gameObject.layer = LayerIndex.entityPrecise.intVal;
            mainHurtbox.healthComponent = prefab.GetComponent<HealthComponent>();
            mainHurtbox.isBullseye = true;
            mainHurtbox.isSniperTarget = headHurtbox == null;
            mainHurtbox.damageModifier = HurtBox.DamageModifier.Normal;
            mainHurtbox.hurtBoxGroup = hurtBoxGroup;
            mainHurtbox.indexInGroup = 0;

            if (headHurtbox) {
                hurtBoxGroup.hurtBoxes = new HurtBox[]
                {
                    mainHurtbox,
                    headHurtbox
                };
            } else {
                hurtBoxGroup.hurtBoxes = new HurtBox[]
                {
                    mainHurtbox,
                };
            }
            hurtBoxGroup.mainHurtBox = mainHurtbox;
            hurtBoxGroup.bullseyeCount = 1;
        }

        private static void SetupFootstepController(GameObject model)
        {
            FootstepHandler footstepHandler = model.AddComponent<FootstepHandler>();
            footstepHandler.baseFootstepString = "Play_player_footstep";
            footstepHandler.sprintFootstepOverrideString = "";
            footstepHandler.enableFootstepDust = true;
            footstepHandler.footstepDustPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/GenericFootstepDust");
        }

        private static void SetupRagdoll(GameObject model)
        {
            RagdollController ragdollController = model.GetComponent<RagdollController>();

            if (!ragdollController) return;

            if (ragdollMaterial == null) ragdollMaterial = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<RagdollController>().bones[1].GetComponent<Collider>().material;

            foreach (Transform boneTransform in ragdollController.bones)
            {
                if (boneTransform)
                {
                    boneTransform.gameObject.layer = LayerIndex.ragdoll.intVal;
                    Collider boneCollider = boneTransform.GetComponent<Collider>();
                    if (boneCollider)
                    {
                        boneCollider.material = ragdollMaterial;
                        boneCollider.sharedMaterial = ragdollMaterial;
                    }
                }
            }
        }

        private static void SetupAimAnimator(GameObject prefab, GameObject model)
        {
            AimAnimator aimAnimator = model.AddComponent<AimAnimator>();
            aimAnimator.directionComponent = prefab.GetComponent<CharacterDirection>();
            aimAnimator.pitchRangeMax = 60f;
            aimAnimator.pitchRangeMin = -60f;
            aimAnimator.yawRangeMin = -80f;
            aimAnimator.yawRangeMax = 80f;
            aimAnimator.pitchGiveupRange = 30f;
            aimAnimator.yawGiveupRange = 10f;
            aimAnimator.giveupDuration = 3f;
            aimAnimator.inputBank = prefab.GetComponent<InputBankTest>();
        }

        public static void SetupHitbox(GameObject modelPrefab, string groupName, params string[] hitboxChildNames) {

            ChildLocator childLocator = modelPrefab.GetComponent<ChildLocator>();

            Transform[] hitboxTransforms = new Transform[hitboxChildNames.Length];
            for (int i = 0; i < hitboxChildNames.Length; i++) {
                hitboxTransforms[i] = childLocator.FindChild(hitboxChildNames[i]);

                if (hitboxTransforms[i] == null) {
                    JoeModForReal.FacelessJoePlugin.Log.LogError("missing hitbox for " + hitboxChildNames[i]);
                }
            }
            SetupHitbox(modelPrefab, groupName, hitboxTransforms);
        }

        //backward compat
        public static void SetupHitbox(GameObject modelPrefab, Transform hitboxTransform, string hitboxName) => SetupHitbox(modelPrefab, hitboxName, hitboxTransform);
        public static void SetupHitbox(GameObject modelPrefab, string hitboxName, params Transform[] hitboxTransforms)
        {
            HitBoxGroup hitBoxGroup = modelPrefab.AddComponent<HitBoxGroup>();
            List<HitBox> hitBoxes = new List<HitBox>();

            foreach (Transform hitboxTransform in hitboxTransforms)
            {
                HitBox hitBox = hitboxTransform.gameObject.AddComponent<HitBox>();
                hitboxTransform.gameObject.layer = LayerIndex.projectile.intVal;
                hitBoxes.Add(hitBox);
            }

            hitBoxGroup.hitBoxes = hitBoxes.ToArray();

            hitBoxGroup.groupName = hitboxName;
        }

        #endregion ComponentSetup
    }
}