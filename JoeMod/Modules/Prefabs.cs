using R2API;
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
            GameObject model = Assets.LoadSurvivorModel(displayModelName);

            CharacterModel characterModel = model.GetComponent<CharacterModel>();
            if (!characterModel) {
                characterModel = model.AddComponent<CharacterModel>();
            }
            characterModel.baseRendererInfos = prefab.GetComponentInChildren<CharacterModel>().baseRendererInfos;

            Modules.Assets.ConvertAllRenderersToHopooShader(model);

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
                newModel = Assets.LoadSurvivorModel(modelName);
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
            characterModel.temporaryOverlays = new List<TemporaryOverlay>();

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
                    if (!rend) {

                    } else {
                        rendererInfos.Add(new CharacterModel.RendererInfo {
                            renderer = childLocator.FindChild(customInfos[i].childName).GetComponent<Renderer>(),
                            defaultMaterial = customInfos[i].material,
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

        private static void SetupMainHurtboxFromChildLocator(GameObject prefab, GameObject model)
        {
            ChildLocator childLocator = model.GetComponent<ChildLocator>();

            if (!childLocator.FindChild("MainHurtbox"))
            {
                Helpers.LogWarning("Could not set up main hurtbox: make sure you have a transform pair in your prefab's ChildLocator component called 'MainHurtbox'");
                return;
            }

            HurtBoxGroup hurtBoxGroup = model.AddComponent<HurtBoxGroup>();
            HurtBox mainHurtbox = childLocator.FindChild("MainHurtbox").gameObject.AddComponent<HurtBox>();
            mainHurtbox.gameObject.layer = LayerIndex.entityPrecise.intVal;
            mainHurtbox.healthComponent = prefab.GetComponent<HealthComponent>();
            mainHurtbox.isBullseye = true;
            mainHurtbox.damageModifier = HurtBox.DamageModifier.Normal;
            mainHurtbox.hurtBoxGroup = hurtBoxGroup;
            mainHurtbox.indexInGroup = 0;

            hurtBoxGroup.hurtBoxes = new HurtBox[]
            {
                mainHurtbox
            };

            hurtBoxGroup.mainHurtBox = mainHurtbox;
            hurtBoxGroup.bullseyeCount = 1;
        }
        
        public static void SetupHurtBoxGroup(GameObject bodyPrefab, GameObject bodyModel) {

            HealthComponent healthComponent = bodyPrefab.GetComponent<HealthComponent>();
            HurtBoxGroup hurtboxGroup = bodyModel.GetComponent<HurtBoxGroup>();

            if (hurtboxGroup != null) {
                hurtboxGroup.mainHurtBox.healthComponent = healthComponent;
                for (int i = 0; i < hurtboxGroup.hurtBoxes.Length; i++) {
                    hurtboxGroup.hurtBoxes[i].healthComponent = healthComponent;
                    //hurtboxGroup.hurtBoxes[i].gameObject.layer = LayerIndex.entityPrecise.intVal;
                }
            } else {
                SetupMainHurtboxFromChildLocator(bodyPrefab, bodyModel);
            }

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
                    TeslaTrooperPlugin.Log.LogError("missing hitbox for " + hitboxChildNames[i]);
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

/*
[Info   : Unity Log] set tim: 1
[Warning:Tesla Trooper] setting. now 2 overrides
[Info   :Tesla Trooper]   at System.Environment.get_StackTrace () [0x00000] in <44afb4564e9347cf99a1865351ea8f4a>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_SetSkillOverride (On.RoR2.GenericSkill+orig_SetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::SetSkillOverride>?127559168 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <67964cac4f384ff6b53bbe5ecbaee307>:IL_0000
  at AncientScepter.AncientScepterItem.HandleScepterSkill (RoR2.CharacterBody self, System.Boolean forceOff) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at RoR2.CharacterBody.ReplaceSkillIfItemPresent (RoR2.GenericSkill skill, RoR2.ItemIndex itemIndex, RoR2.Skills.SkillDef skillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<OnInventoryChanged>?69243648._RoR2_CharacterBody::OnInventoryChanged (RoR2.CharacterBody this) [0x00000] in <f35d06abecb542c59abe564aada42d55>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::OnInventoryChanged>?-924452864 (RoR2.CharacterBody ) [0x00000] in <b637a1bd61454f7ab5711c37c3b218ee>:IL_0000
  at SillyGlasses.SillyGlassesPlugin.InvChangedHook (On.RoR2.CharacterBody+orig_OnInventoryChanged orig, RoR2.CharacterBody self) [0x00000] in <b1f8657d12fb448c9ef4adffea774a93>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::OnInventoryChanged>?1191347456 (RoR2.CharacterBody ) [0x00000] in <6d0e1c9ef8b14323b22ae4fe85493bdf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::OnInventoryChanged>?1645529088 (RoR2.CharacterBody ) [0x00000] in <b415c93b3cba44c1838e673a31180414>:IL_0000
  at TPDespair.ZetAspects.EffectHooks+<>c.<ApplyAspectBuffOnInventoryChangedHook>b__67_0 (On.RoR2.CharacterBody+orig_OnInventoryChanged orig, RoR2.CharacterBody self) [0x00000] in <4dadcd94303044279e739eddde02c081>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::OnInventoryChanged>?1052907904 (RoR2.CharacterBody ) [0x00000] in <9b5bc9a981d948b98c823e26928e2d80>:IL_0000
  at RoR2.Inventory.HandleInventoryChanged () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.Inventory.GiveItem (RoR2.ItemIndex itemIndex, System.Int32 count) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.Inventory.GiveItem (RoR2.ItemDef itemDef, System.Int32 count) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at TPDespair.ZetAspects.EffectHooks+<>c.<UpdateOnBuffLostHook>b__69_0 (On.RoR2.CharacterBody+orig_OnBuffFinalStackLost orig, RoR2.CharacterBody self, RoR2.BuffDef buffDef) [0x00000] in <4dadcd94303044279e739eddde02c081>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::OnBuffFinalStackLost>?1056918656 (RoR2.CharacterBody , RoR2.BuffDef ) [0x00000] in <3a9b836845dd4e6f845ba18cf6892a5a>:IL_0000
  at RoR2.CharacterBody.SetBuffCount (RoR2.BuffIndex buffType, System.Int32 newCount) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<RemoveBuff>?69243648._RoR2_CharacterBody::RemoveBuff (RoR2.CharacterBody this, RoR2.BuffIndex buffType) [0x00000] in <c2a05f6afefc4b9d8a4f8c06bb19180b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::RemoveBuff>?1506109440 (RoR2.CharacterBody , RoR2.BuffIndex ) [0x00000] in <00bd3db27e8c4d6196343ffe0ce8a458>:IL_0000
  at TPDespair.ZetAspects.EffectHooks.VoidBearFix_RemoveBuff (On.RoR2.CharacterBody+orig_RemoveBuff_BuffIndex orig, RoR2.CharacterBody self, RoR2.BuffIndex buffType) [0x00000] in <4dadcd94303044279e739eddde02c081>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::RemoveBuff>?1765697024 (RoR2.CharacterBody , RoR2.BuffIndex ) [0x00000] in <a95d742e1da9454aa3080782b9781f25>:IL_0000
  at RoR2.CharacterBody.RemoveBuff (RoR2.BuffDef buffDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at ModdedEntityStates.Desolator.DeployIrradiate.OnExit () [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at RoR2.EntityStateMachine.SetState (EntityStates.EntityState newState) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.EntityStateMachine.FixedUpdate () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
[Warning:Tesla Trooper] setting. now 2 overrides
[Info   :Tesla Trooper]   at System.Environment.get_StackTrace () [0x00000] in <44afb4564e9347cf99a1865351ea8f4a>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_SetSkillOverride (On.RoR2.GenericSkill+orig_SetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::SetSkillOverride>?127559168 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <67964cac4f384ff6b53bbe5ecbaee307>:IL_0000
  at AncientScepter.AncientScepterItem.HandleScepterSkill (RoR2.CharacterBody self, System.Boolean forceOff) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at RoR2.CharacterBody.ReplaceSkillIfItemPresent (RoR2.GenericSkill skill, RoR2.ItemIndex itemIndex, RoR2.Skills.SkillDef skillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<OnInventoryChanged>?69243648._RoR2_CharacterBody::OnInventoryChanged (RoR2.CharacterBody this) [0x00000] in <f35d06abecb542c59abe564aada42d55>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::OnInventoryChanged>?-924452864 (RoR2.CharacterBody ) [0x00000] in <b637a1bd61454f7ab5711c37c3b218ee>:IL_0000
  at SillyGlasses.SillyGlassesPlugin.InvChangedHook (On.RoR2.CharacterBody+orig_OnInventoryChanged orig, RoR2.CharacterBody self) [0x00000] in <b1f8657d12fb448c9ef4adffea774a93>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::OnInventoryChanged>?1191347456 (RoR2.CharacterBody ) [0x00000] in <6d0e1c9ef8b14323b22ae4fe85493bdf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::OnInventoryChanged>?1645529088 (RoR2.CharacterBody ) [0x00000] in <b415c93b3cba44c1838e673a31180414>:IL_0000
  at TPDespair.ZetAspects.EffectHooks+<>c.<ApplyAspectBuffOnInventoryChangedHook>b__67_0 (On.RoR2.CharacterBody+orig_OnInventoryChanged orig, RoR2.CharacterBody self) [0x00000] in <4dadcd94303044279e739eddde02c081>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::OnInventoryChanged>?1052907904 (RoR2.CharacterBody ) [0x00000] in <9b5bc9a981d948b98c823e26928e2d80>:IL_0000
  at RoR2.Inventory.HandleInventoryChanged () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.Inventory.GiveItem (RoR2.ItemIndex itemIndex, System.Int32 count) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.Inventory.GiveItem (RoR2.ItemDef itemDef, System.Int32 count) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at TPDespair.ZetAspects.EffectHooks+<>c.<UpdateOnBuffLostHook>b__69_0 (On.RoR2.CharacterBody+orig_OnBuffFinalStackLost orig, RoR2.CharacterBody self, RoR2.BuffDef buffDef) [0x00000] in <4dadcd94303044279e739eddde02c081>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::OnBuffFinalStackLost>?1056918656 (RoR2.CharacterBody , RoR2.BuffDef ) [0x00000] in <3a9b836845dd4e6f845ba18cf6892a5a>:IL_0000
  at RoR2.CharacterBody.SetBuffCount (RoR2.BuffIndex buffType, System.Int32 newCount) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<RemoveBuff>?69243648._RoR2_CharacterBody::RemoveBuff (RoR2.CharacterBody this, RoR2.BuffIndex buffType) [0x00000] in <c2a05f6afefc4b9d8a4f8c06bb19180b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::RemoveBuff>?1506109440 (RoR2.CharacterBody , RoR2.BuffIndex ) [0x00000] in <00bd3db27e8c4d6196343ffe0ce8a458>:IL_0000
  at TPDespair.ZetAspects.EffectHooks.VoidBearFix_RemoveBuff (On.RoR2.CharacterBody+orig_RemoveBuff_BuffIndex orig, RoR2.CharacterBody self, RoR2.BuffIndex buffType) [0x00000] in <4dadcd94303044279e739eddde02c081>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::RemoveBuff>?1765697024 (RoR2.CharacterBody , RoR2.BuffIndex ) [0x00000] in <a95d742e1da9454aa3080782b9781f25>:IL_0000
  at RoR2.CharacterBody.RemoveBuff (RoR2.BuffDef buffDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at ModdedEntityStates.Desolator.DeployIrradiate.OnExit () [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at RoR2.EntityStateMachine.SetState (EntityStates.EntityState newState) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.EntityStateMachine.FixedUpdate () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
[Warning:Tesla Trooper] setting. now 2 overrides
[Info   :Tesla Trooper]   at System.Environment.get_StackTrace () [0x00000] in <44afb4564e9347cf99a1865351ea8f4a>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_SetSkillOverride (On.RoR2.GenericSkill+orig_SetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::SetSkillOverride>?127559168 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <67964cac4f384ff6b53bbe5ecbaee307>:IL_0000
  at AncientScepter.AncientScepterItem.HandleScepterSkill (RoR2.CharacterBody self, System.Boolean forceOff) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at RoR2.CharacterBody.ReplaceSkillIfItemPresent (RoR2.GenericSkill skill, RoR2.ItemIndex itemIndex, RoR2.Skills.SkillDef skillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<OnInventoryChanged>?69243648._RoR2_CharacterBody::OnInventoryChanged (RoR2.CharacterBody this) [0x00000] in <f35d06abecb542c59abe564aada42d55>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::OnInventoryChanged>?-924452864 (RoR2.CharacterBody ) [0x00000] in <b637a1bd61454f7ab5711c37c3b218ee>:IL_0000
  at SillyGlasses.SillyGlassesPlugin.InvChangedHook (On.RoR2.CharacterBody+orig_OnInventoryChanged orig, RoR2.CharacterBody self) [0x00000] in <b1f8657d12fb448c9ef4adffea774a93>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::OnInventoryChanged>?1191347456 (RoR2.CharacterBody ) [0x00000] in <6d0e1c9ef8b14323b22ae4fe85493bdf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::OnInventoryChanged>?1645529088 (RoR2.CharacterBody ) [0x00000] in <b415c93b3cba44c1838e673a31180414>:IL_0000
  at TPDespair.ZetAspects.EffectHooks+<>c.<ApplyAspectBuffOnInventoryChangedHook>b__67_0 (On.RoR2.CharacterBody+orig_OnInventoryChanged orig, RoR2.CharacterBody self) [0x00000] in <4dadcd94303044279e739eddde02c081>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::OnInventoryChanged>?1052907904 (RoR2.CharacterBody ) [0x00000] in <9b5bc9a981d948b98c823e26928e2d80>:IL_0000
  at RoR2.Inventory.HandleInventoryChanged () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.Inventory.GiveItem (RoR2.ItemIndex itemIndex, System.Int32 count) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.Inventory.GiveItem (RoR2.ItemDef itemDef, System.Int32 count) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at TPDespair.ZetAspects.EffectHooks+<>c.<UpdateOnBuffLostHook>b__69_0 (On.RoR2.CharacterBody+orig_OnBuffFinalStackLost orig, RoR2.CharacterBody self, RoR2.BuffDef buffDef) [0x00000] in <4dadcd94303044279e739eddde02c081>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::OnBuffFinalStackLost>?1056918656 (RoR2.CharacterBody , RoR2.BuffDef ) [0x00000] in <3a9b836845dd4e6f845ba18cf6892a5a>:IL_0000
  at RoR2.CharacterBody.SetBuffCount (RoR2.BuffIndex buffType, System.Int32 newCount) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<RemoveBuff>?69243648._RoR2_CharacterBody::RemoveBuff (RoR2.CharacterBody this, RoR2.BuffIndex buffType) [0x00000] in <c2a05f6afefc4b9d8a4f8c06bb19180b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::RemoveBuff>?1506109440 (RoR2.CharacterBody , RoR2.BuffIndex ) [0x00000] in <00bd3db27e8c4d6196343ffe0ce8a458>:IL_0000
  at TPDespair.ZetAspects.EffectHooks.VoidBearFix_RemoveBuff (On.RoR2.CharacterBody+orig_RemoveBuff_BuffIndex orig, RoR2.CharacterBody self, RoR2.BuffIndex buffType) [0x00000] in <4dadcd94303044279e739eddde02c081>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::RemoveBuff>?1765697024 (RoR2.CharacterBody , RoR2.BuffIndex ) [0x00000] in <a95d742e1da9454aa3080782b9781f25>:IL_0000
  at RoR2.CharacterBody.RemoveBuff (RoR2.BuffDef buffDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at ModdedEntityStates.Desolator.DeployIrradiate.OnExit () [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at RoR2.EntityStateMachine.SetState (EntityStates.EntityState newState) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.EntityStateMachine.FixedUpdate () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
[Warning:Tesla Trooper] unsetting. now 2 overrides
[Info   :Tesla Trooper]   at System.Environment.get_StackTrace () [0x00000] in <44afb4564e9347cf99a1865351ea8f4a>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at RoR2.CharacterBody.ReplaceSkillIfItemPresent (RoR2.GenericSkill skill, RoR2.ItemIndex itemIndex, RoR2.Skills.SkillDef skillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<OnInventoryChanged>?69243648._RoR2_CharacterBody::OnInventoryChanged (RoR2.CharacterBody this) [0x00000] in <f35d06abecb542c59abe564aada42d55>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::OnInventoryChanged>?-924452864 (RoR2.CharacterBody ) [0x00000] in <b637a1bd61454f7ab5711c37c3b218ee>:IL_0000
  at SillyGlasses.SillyGlassesPlugin.InvChangedHook (On.RoR2.CharacterBody+orig_OnInventoryChanged orig, RoR2.CharacterBody self) [0x00000] in <b1f8657d12fb448c9ef4adffea774a93>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::OnInventoryChanged>?1191347456 (RoR2.CharacterBody ) [0x00000] in <6d0e1c9ef8b14323b22ae4fe85493bdf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::OnInventoryChanged>?1645529088 (RoR2.CharacterBody ) [0x00000] in <b415c93b3cba44c1838e673a31180414>:IL_0000
  at TPDespair.ZetAspects.EffectHooks+<>c.<ApplyAspectBuffOnInventoryChangedHook>b__67_0 (On.RoR2.CharacterBody+orig_OnInventoryChanged orig, RoR2.CharacterBody self) [0x00000] in <4dadcd94303044279e739eddde02c081>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::OnInventoryChanged>?1052907904 (RoR2.CharacterBody ) [0x00000] in <9b5bc9a981d948b98c823e26928e2d80>:IL_0000
  at RoR2.Inventory.HandleInventoryChanged () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.Inventory.GiveItem (RoR2.ItemIndex itemIndex, System.Int32 count) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.Inventory.GiveItem (RoR2.ItemDef itemDef, System.Int32 count) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at TPDespair.ZetAspects.EffectHooks+<>c.<UpdateOnBuffLostHook>b__69_0 (On.RoR2.CharacterBody+orig_OnBuffFinalStackLost orig, RoR2.CharacterBody self, RoR2.BuffDef buffDef) [0x00000] in <4dadcd94303044279e739eddde02c081>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::OnBuffFinalStackLost>?1056918656 (RoR2.CharacterBody , RoR2.BuffDef ) [0x00000] in <3a9b836845dd4e6f845ba18cf6892a5a>:IL_0000
  at RoR2.CharacterBody.SetBuffCount (RoR2.BuffIndex buffType, System.Int32 newCount) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<RemoveBuff>?69243648._RoR2_CharacterBody::RemoveBuff (RoR2.CharacterBody this, RoR2.BuffIndex buffType) [0x00000] in <c2a05f6afefc4b9d8a4f8c06bb19180b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::RemoveBuff>?1506109440 (RoR2.CharacterBody , RoR2.BuffIndex ) [0x00000] in <00bd3db27e8c4d6196343ffe0ce8a458>:IL_0000
  at TPDespair.ZetAspects.EffectHooks.VoidBearFix_RemoveBuff (On.RoR2.CharacterBody+orig_RemoveBuff_BuffIndex orig, RoR2.CharacterBody self, RoR2.BuffIndex buffType) [0x00000] in <4dadcd94303044279e739eddde02c081>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::RemoveBuff>?1765697024 (RoR2.CharacterBody , RoR2.BuffIndex ) [0x00000] in <a95d742e1da9454aa3080782b9781f25>:IL_0000
  at RoR2.CharacterBody.RemoveBuff (RoR2.BuffDef buffDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at ModdedEntityStates.Desolator.DeployIrradiate.OnExit () [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at RoR2.EntityStateMachine.SetState (EntityStates.EntityState newState) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.EntityStateMachine.FixedUpdate () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
[Warning:Tesla Trooper] setting. now 2 overrides
[Info   :Tesla Trooper]   at System.Environment.get_StackTrace () [0x00000] in <44afb4564e9347cf99a1865351ea8f4a>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_SetSkillOverride (On.RoR2.GenericSkill+orig_SetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::SetSkillOverride>?127559168 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <67964cac4f384ff6b53bbe5ecbaee307>:IL_0000
  at AncientScepter.AncientScepterItem.HandleScepterSkill (RoR2.CharacterBody self, System.Boolean forceOff) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at RoR2.CharacterBody.ReplaceSkillIfItemPresent (RoR2.GenericSkill skill, RoR2.ItemIndex itemIndex, RoR2.Skills.SkillDef skillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<OnInventoryChanged>?69243648._RoR2_CharacterBody::OnInventoryChanged (RoR2.CharacterBody this) [0x00000] in <f35d06abecb542c59abe564aada42d55>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::OnInventoryChanged>?-924452864 (RoR2.CharacterBody ) [0x00000] in <b637a1bd61454f7ab5711c37c3b218ee>:IL_0000
  at SillyGlasses.SillyGlassesPlugin.InvChangedHook (On.RoR2.CharacterBody+orig_OnInventoryChanged orig, RoR2.CharacterBody self) [0x00000] in <b1f8657d12fb448c9ef4adffea774a93>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::OnInventoryChanged>?1191347456 (RoR2.CharacterBody ) [0x00000] in <6d0e1c9ef8b14323b22ae4fe85493bdf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::OnInventoryChanged>?1645529088 (RoR2.CharacterBody ) [0x00000] in <b415c93b3cba44c1838e673a31180414>:IL_0000
  at TPDespair.ZetAspects.EffectHooks+<>c.<ApplyAspectBuffOnInventoryChangedHook>b__67_0 (On.RoR2.CharacterBody+orig_OnInventoryChanged orig, RoR2.CharacterBody self) [0x00000] in <4dadcd94303044279e739eddde02c081>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::OnInventoryChanged>?1052907904 (RoR2.CharacterBody ) [0x00000] in <9b5bc9a981d948b98c823e26928e2d80>:IL_0000
  at RoR2.Inventory.HandleInventoryChanged () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.Inventory.GiveItem (RoR2.ItemIndex itemIndex, System.Int32 count) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.Inventory.GiveItem (RoR2.ItemDef itemDef, System.Int32 count) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at TPDespair.ZetAspects.EffectHooks+<>c.<UpdateOnBuffLostHook>b__69_0 (On.RoR2.CharacterBody+orig_OnBuffFinalStackLost orig, RoR2.CharacterBody self, RoR2.BuffDef buffDef) [0x00000] in <4dadcd94303044279e739eddde02c081>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::OnBuffFinalStackLost>?1056918656 (RoR2.CharacterBody , RoR2.BuffDef ) [0x00000] in <3a9b836845dd4e6f845ba18cf6892a5a>:IL_0000
  at RoR2.CharacterBody.SetBuffCount (RoR2.BuffIndex buffType, System.Int32 newCount) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<RemoveBuff>?69243648._RoR2_CharacterBody::RemoveBuff (RoR2.CharacterBody this, RoR2.BuffIndex buffType) [0x00000] in <c2a05f6afefc4b9d8a4f8c06bb19180b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::RemoveBuff>?1506109440 (RoR2.CharacterBody , RoR2.BuffIndex ) [0x00000] in <00bd3db27e8c4d6196343ffe0ce8a458>:IL_0000
  at TPDespair.ZetAspects.EffectHooks.VoidBearFix_RemoveBuff (On.RoR2.CharacterBody+orig_RemoveBuff_BuffIndex orig, RoR2.CharacterBody self, RoR2.BuffIndex buffType) [0x00000] in <4dadcd94303044279e739eddde02c081>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::RemoveBuff>?1765697024 (RoR2.CharacterBody , RoR2.BuffIndex ) [0x00000] in <a95d742e1da9454aa3080782b9781f25>:IL_0000
  at RoR2.CharacterBody.RemoveBuff (RoR2.BuffDef buffDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at ModdedEntityStates.Desolator.DeployIrradiate.OnExit () [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at RoR2.EntityStateMachine.SetState (EntityStates.EntityState newState) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.EntityStateMachine.FixedUpdate () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
[Warning:Tesla Trooper] setting. now 2 overrides
[Info   :Tesla Trooper]   at System.Environment.get_StackTrace () [0x00000] in <44afb4564e9347cf99a1865351ea8f4a>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_SetSkillOverride (On.RoR2.GenericSkill+orig_SetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::SetSkillOverride>?127559168 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <67964cac4f384ff6b53bbe5ecbaee307>:IL_0000
  at AncientScepter.AncientScepterItem.HandleScepterSkill (RoR2.CharacterBody self, System.Boolean forceOff) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at AncientScepter.AncientScepterItem.On_CBInventoryChangedGlobal (RoR2.CharacterBody body) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<OnInventoryChanged>?69243648._RoR2_CharacterBody::OnInventoryChanged (RoR2.CharacterBody this) [0x00000] in <f35d06abecb542c59abe564aada42d55>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::OnInventoryChanged>?-924452864 (RoR2.CharacterBody ) [0x00000] in <b637a1bd61454f7ab5711c37c3b218ee>:IL_0000
  at SillyGlasses.SillyGlassesPlugin.InvChangedHook (On.RoR2.CharacterBody+orig_OnInventoryChanged orig, RoR2.CharacterBody self) [0x00000] in <b1f8657d12fb448c9ef4adffea774a93>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::OnInventoryChanged>?1191347456 (RoR2.CharacterBody ) [0x00000] in <6d0e1c9ef8b14323b22ae4fe85493bdf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::OnInventoryChanged>?1645529088 (RoR2.CharacterBody ) [0x00000] in <b415c93b3cba44c1838e673a31180414>:IL_0000
  at TPDespair.ZetAspects.EffectHooks+<>c.<ApplyAspectBuffOnInventoryChangedHook>b__67_0 (On.RoR2.CharacterBody+orig_OnInventoryChanged orig, RoR2.CharacterBody self) [0x00000] in <4dadcd94303044279e739eddde02c081>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::OnInventoryChanged>?1052907904 (RoR2.CharacterBody ) [0x00000] in <9b5bc9a981d948b98c823e26928e2d80>:IL_0000
  at RoR2.Inventory.HandleInventoryChanged () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.Inventory.GiveItem (RoR2.ItemIndex itemIndex, System.Int32 count) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.Inventory.GiveItem (RoR2.ItemDef itemDef, System.Int32 count) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at TPDespair.ZetAspects.EffectHooks+<>c.<UpdateOnBuffLostHook>b__69_0 (On.RoR2.CharacterBody+orig_OnBuffFinalStackLost orig, RoR2.CharacterBody self, RoR2.BuffDef buffDef) [0x00000] in <4dadcd94303044279e739eddde02c081>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::OnBuffFinalStackLost>?1056918656 (RoR2.CharacterBody , RoR2.BuffDef ) [0x00000] in <3a9b836845dd4e6f845ba18cf6892a5a>:IL_0000
  at RoR2.CharacterBody.SetBuffCount (RoR2.BuffIndex buffType, System.Int32 newCount) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<RemoveBuff>?69243648._RoR2_CharacterBody::RemoveBuff (RoR2.CharacterBody this, RoR2.BuffIndex buffType) [0x00000] in <c2a05f6afefc4b9d8a4f8c06bb19180b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::RemoveBuff>?1506109440 (RoR2.CharacterBody , RoR2.BuffIndex ) [0x00000] in <00bd3db27e8c4d6196343ffe0ce8a458>:IL_0000
  at TPDespair.ZetAspects.EffectHooks.VoidBearFix_RemoveBuff (On.RoR2.CharacterBody+orig_RemoveBuff_BuffIndex orig, RoR2.CharacterBody self, RoR2.BuffIndex buffType) [0x00000] in <4dadcd94303044279e739eddde02c081>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::RemoveBuff>?1765697024 (RoR2.CharacterBody , RoR2.BuffIndex ) [0x00000] in <a95d742e1da9454aa3080782b9781f25>:IL_0000
  at RoR2.CharacterBody.RemoveBuff (RoR2.BuffDef buffDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at ModdedEntityStates.Desolator.DeployIrradiate.OnExit () [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at RoR2.EntityStateMachine.SetState (EntityStates.EntityState newState) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.EntityStateMachine.FixedUpdate () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
[Warning:Tesla Trooper] unset DeployIrradiate onexit !_complete
[Warning:Tesla Trooper] unsetting. now 0 overrides
[Info   :Tesla Trooper]   at System.Environment.get_StackTrace () [0x00000] in <44afb4564e9347cf99a1865351ea8f4a>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<SetSkillOverride>?69243648._RoR2_GenericSkill::SetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <25459643799e4cd9a3ed7bdddb9af733>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::SetSkillOverride>?1388957696 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <57f520f929fb47d380727fa1409d2fa0>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_SetSkillOverride (On.RoR2.GenericSkill+orig_SetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::SetSkillOverride>?127559168 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <67964cac4f384ff6b53bbe5ecbaee307>:IL_0000
  at AncientScepter.AncientScepterItem.HandleScepterSkill (RoR2.CharacterBody self, System.Boolean forceOff) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at ModdedEntityStates.Desolator.DeployIrradiate.OnExit () [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at RoR2.EntityStateMachine.SetState (EntityStates.EntityState newState) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.EntityStateMachine.FixedUpdate () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
[Warning:Tesla Trooper] unsetting. now 0 overrides
[Info   :Tesla Trooper]   at System.Environment.get_StackTrace () [0x00000] in <44afb4564e9347cf99a1865351ea8f4a>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<SetSkillOverride>?69243648._RoR2_GenericSkill::SetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <25459643799e4cd9a3ed7bdddb9af733>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::SetSkillOverride>?1388957696 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <57f520f929fb47d380727fa1409d2fa0>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_SetSkillOverride (On.RoR2.GenericSkill+orig_SetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::SetSkillOverride>?127559168 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <67964cac4f384ff6b53bbe5ecbaee307>:IL_0000
  at AncientScepter.AncientScepterItem.HandleScepterSkill (RoR2.CharacterBody self, System.Boolean forceOff) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at ModdedEntityStates.Desolator.DeployIrradiate.OnExit () [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at RoR2.EntityStateMachine.SetState (EntityStates.EntityState newState) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.EntityStateMachine.FixedUpdate () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
[Warning:Tesla Trooper] unsetting. now 0 overrides
[Info   :Tesla Trooper]   at System.Environment.get_StackTrace () [0x00000] in <44afb4564e9347cf99a1865351ea8f4a>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<SetSkillOverride>?69243648._RoR2_GenericSkill::SetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <25459643799e4cd9a3ed7bdddb9af733>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::SetSkillOverride>?1388957696 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <57f520f929fb47d380727fa1409d2fa0>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_SetSkillOverride (On.RoR2.GenericSkill+orig_SetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::SetSkillOverride>?127559168 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <67964cac4f384ff6b53bbe5ecbaee307>:IL_0000
  at AncientScepter.AncientScepterItem.HandleScepterSkill (RoR2.CharacterBody self, System.Boolean forceOff) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at ModdedEntityStates.Desolator.DeployIrradiate.OnExit () [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at RoR2.EntityStateMachine.SetState (EntityStates.EntityState newState) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.EntityStateMachine.FixedUpdate () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
[Warning:Tesla Trooper] unsetting. now 0 overrides
[Info   :Tesla Trooper]   at System.Environment.get_StackTrace () [0x00000] in <44afb4564e9347cf99a1865351ea8f4a>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<SetSkillOverride>?69243648._RoR2_GenericSkill::SetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <25459643799e4cd9a3ed7bdddb9af733>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::SetSkillOverride>?1388957696 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <57f520f929fb47d380727fa1409d2fa0>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_SetSkillOverride (On.RoR2.GenericSkill+orig_SetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::SetSkillOverride>?127559168 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <67964cac4f384ff6b53bbe5ecbaee307>:IL_0000
  at AncientScepter.AncientScepterItem.HandleScepterSkill (RoR2.CharacterBody self, System.Boolean forceOff) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at ModdedEntityStates.Desolator.DeployIrradiate.OnExit () [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at RoR2.EntityStateMachine.SetState (EntityStates.EntityState newState) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.EntityStateMachine.FixedUpdate () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
[Warning:Tesla Trooper] unsetting. now 0 overrides
[Info   :Tesla Trooper]   at System.Environment.get_StackTrace () [0x00000] in <44afb4564e9347cf99a1865351ea8f4a>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<SetSkillOverride>?69243648._RoR2_GenericSkill::SetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <25459643799e4cd9a3ed7bdddb9af733>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::SetSkillOverride>?1388957696 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <57f520f929fb47d380727fa1409d2fa0>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_SetSkillOverride (On.RoR2.GenericSkill+orig_SetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::SetSkillOverride>?127559168 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <67964cac4f384ff6b53bbe5ecbaee307>:IL_0000
  at AncientScepter.AncientScepterItem.HandleScepterSkill (RoR2.CharacterBody self, System.Boolean forceOff) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at ModdedEntityStates.Desolator.DeployIrradiate.OnExit () [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at RoR2.EntityStateMachine.SetState (EntityStates.EntityState newState) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.EntityStateMachine.FixedUpdate () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
[Warning:Tesla Trooper] unsetting. now 0 overrides
[Info   :Tesla Trooper]   at System.Environment.get_StackTrace () [0x00000] in <44afb4564e9347cf99a1865351ea8f4a>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<SetSkillOverride>?69243648._RoR2_GenericSkill::SetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <25459643799e4cd9a3ed7bdddb9af733>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::SetSkillOverride>?1388957696 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <57f520f929fb47d380727fa1409d2fa0>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_SetSkillOverride (On.RoR2.GenericSkill+orig_SetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::SetSkillOverride>?127559168 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <67964cac4f384ff6b53bbe5ecbaee307>:IL_0000
  at AncientScepter.AncientScepterItem.HandleScepterSkill (RoR2.CharacterBody self, System.Boolean forceOff) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at ModdedEntityStates.Desolator.DeployIrradiate.OnExit () [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at RoR2.EntityStateMachine.SetState (EntityStates.EntityState newState) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.EntityStateMachine.FixedUpdate () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
[Warning:Tesla Trooper] unsetting. now 0 overrides
[Info   :Tesla Trooper]   at System.Environment.get_StackTrace () [0x00000] in <44afb4564e9347cf99a1865351ea8f4a>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<SetSkillOverride>?69243648._RoR2_GenericSkill::SetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <25459643799e4cd9a3ed7bdddb9af733>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::SetSkillOverride>?1388957696 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <57f520f929fb47d380727fa1409d2fa0>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_SetSkillOverride (On.RoR2.GenericSkill+orig_SetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::SetSkillOverride>?127559168 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <67964cac4f384ff6b53bbe5ecbaee307>:IL_0000
  at AncientScepter.AncientScepterItem.HandleScepterSkill (RoR2.CharacterBody self, System.Boolean forceOff) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at ModdedEntityStates.Desolator.DeployIrradiate.OnExit () [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at RoR2.EntityStateMachine.SetState (EntityStates.EntityState newState) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.EntityStateMachine.FixedUpdate () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
[Warning:Tesla Trooper] setting. now 0 overrides
[Info   :Tesla Trooper]   at System.Environment.get_StackTrace () [0x00000] in <44afb4564e9347cf99a1865351ea8f4a>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_SetSkillOverride (On.RoR2.GenericSkill+orig_SetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::SetSkillOverride>?127559168 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <67964cac4f384ff6b53bbe5ecbaee307>:IL_0000
  at AncientScepter.AncientScepterItem.HandleScepterSkill (RoR2.CharacterBody self, System.Boolean forceOff) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at ModdedEntityStates.Desolator.DeployIrradiate.OnExit () [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at RoR2.EntityStateMachine.SetState (EntityStates.EntityState newState) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.EntityStateMachine.FixedUpdate () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
[Warning:Tesla Trooper] unsetting. now 0 overrides
[Info   :Tesla Trooper]   at System.Environment.get_StackTrace () [0x00000] in <44afb4564e9347cf99a1865351ea8f4a>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at ModdedEntityStates.Desolator.DeployIrradiate.OnExit () [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at RoR2.EntityStateMachine.SetState (EntityStates.EntityState newState) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.EntityStateMachine.FixedUpdate () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
[Warning:Tesla Trooper] unsetting. now 0 overrides
[Info   :Tesla Trooper]   at System.Environment.get_StackTrace () [0x00000] in <44afb4564e9347cf99a1865351ea8f4a>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<SetSkillOverride>?69243648._RoR2_GenericSkill::SetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <25459643799e4cd9a3ed7bdddb9af733>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::SetSkillOverride>?1388957696 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <57f520f929fb47d380727fa1409d2fa0>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_SetSkillOverride (On.RoR2.GenericSkill+orig_SetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::SetSkillOverride>?127559168 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <67964cac4f384ff6b53bbe5ecbaee307>:IL_0000
  at AncientScepter.AncientScepterItem.HandleScepterSkill (RoR2.CharacterBody self, System.Boolean forceOff) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at ModdedEntityStates.Desolator.DeployIrradiate.OnExit () [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at RoR2.EntityStateMachine.SetState (EntityStates.EntityState newState) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.EntityStateMachine.FixedUpdate () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
[Warning:Tesla Trooper] setting. now 0 overrides
[Info   :Tesla Trooper]   at System.Environment.get_StackTrace () [0x00000] in <44afb4564e9347cf99a1865351ea8f4a>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_SetSkillOverride (On.RoR2.GenericSkill+orig_SetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::SetSkillOverride>?127559168 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <67964cac4f384ff6b53bbe5ecbaee307>:IL_0000
  at AncientScepter.AncientScepterItem.HandleScepterSkill (RoR2.CharacterBody self, System.Boolean forceOff) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at ModdedEntityStates.Desolator.DeployIrradiate.OnExit () [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at RoR2.EntityStateMachine.SetState (EntityStates.EntityState newState) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.EntityStateMachine.FixedUpdate () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
[Warning:Tesla Trooper] unsetting. now 0 overrides
[Info   :Tesla Trooper]   at System.Environment.get_StackTrace () [0x00000] in <44afb4564e9347cf99a1865351ea8f4a>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at ModdedEntityStates.Desolator.DeployIrradiate.OnExit () [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at RoR2.EntityStateMachine.SetState (EntityStates.EntityState newState) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.EntityStateMachine.FixedUpdate () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
[Warning:Tesla Trooper] unsetting. now 0 overrides
[Info   :Tesla Trooper]   at System.Environment.get_StackTrace () [0x00000] in <44afb4564e9347cf99a1865351ea8f4a>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<SetSkillOverride>?69243648._RoR2_GenericSkill::SetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <25459643799e4cd9a3ed7bdddb9af733>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::SetSkillOverride>?1388957696 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <57f520f929fb47d380727fa1409d2fa0>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_SetSkillOverride (On.RoR2.GenericSkill+orig_SetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::SetSkillOverride>?127559168 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <67964cac4f384ff6b53bbe5ecbaee307>:IL_0000
  at AncientScepter.AncientScepterItem.HandleScepterSkill (RoR2.CharacterBody self, System.Boolean forceOff) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at ModdedEntityStates.Desolator.DeployIrradiate.OnExit () [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at RoR2.EntityStateMachine.SetState (EntityStates.EntityState newState) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.EntityStateMachine.FixedUpdate () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
[Warning:Tesla Trooper] setting. now 0 overrides
[Info   :Tesla Trooper]   at System.Environment.get_StackTrace () [0x00000] in <44afb4564e9347cf99a1865351ea8f4a>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_SetSkillOverride (On.RoR2.GenericSkill+orig_SetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::SetSkillOverride>?127559168 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <67964cac4f384ff6b53bbe5ecbaee307>:IL_0000
  at AncientScepter.AncientScepterItem.HandleScepterSkill (RoR2.CharacterBody self, System.Boolean forceOff) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at ModdedEntityStates.Desolator.DeployIrradiate.OnExit () [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at RoR2.EntityStateMachine.SetState (EntityStates.EntityState newState) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.EntityStateMachine.FixedUpdate () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
[Warning:Tesla Trooper] unsetting. now 0 overrides
[Info   :Tesla Trooper]   at System.Environment.get_StackTrace () [0x00000] in <44afb4564e9347cf99a1865351ea8f4a>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at ModdedEntityStates.Desolator.DeployIrradiate.OnExit () [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at RoR2.EntityStateMachine.SetState (EntityStates.EntityState newState) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.EntityStateMachine.FixedUpdate () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
[Warning:Tesla Trooper] unsetting. now 0 overrides
[Info   :Tesla Trooper]   at System.Environment.get_StackTrace () [0x00000] in <44afb4564e9347cf99a1865351ea8f4a>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<SetSkillOverride>?69243648._RoR2_GenericSkill::SetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <25459643799e4cd9a3ed7bdddb9af733>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::SetSkillOverride>?1388957696 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <57f520f929fb47d380727fa1409d2fa0>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_SetSkillOverride (On.RoR2.GenericSkill+orig_SetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::SetSkillOverride>?127559168 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <67964cac4f384ff6b53bbe5ecbaee307>:IL_0000
  at AncientScepter.AncientScepterItem.HandleScepterSkill (RoR2.CharacterBody self, System.Boolean forceOff) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at ModdedEntityStates.Desolator.DeployIrradiate.OnExit () [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at RoR2.EntityStateMachine.SetState (EntityStates.EntityState newState) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.EntityStateMachine.FixedUpdate () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
[Warning:Tesla Trooper] setting. now 0 overrides
[Info   :Tesla Trooper]   at System.Environment.get_StackTrace () [0x00000] in <44afb4564e9347cf99a1865351ea8f4a>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_SetSkillOverride (On.RoR2.GenericSkill+orig_SetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::SetSkillOverride>?127559168 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <67964cac4f384ff6b53bbe5ecbaee307>:IL_0000
  at AncientScepter.AncientScepterItem.HandleScepterSkill (RoR2.CharacterBody self, System.Boolean forceOff) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at ModdedEntityStates.Desolator.DeployIrradiate.OnExit () [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at RoR2.EntityStateMachine.SetState (EntityStates.EntityState newState) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.EntityStateMachine.FixedUpdate () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
[Warning:Tesla Trooper] unsetting. now 0 overrides
[Info   :Tesla Trooper]   at System.Environment.get_StackTrace () [0x00000] in <44afb4564e9347cf99a1865351ea8f4a>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at ModdedEntityStates.Desolator.DeployIrradiate.OnExit () [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at RoR2.EntityStateMachine.SetState (EntityStates.EntityState newState) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.EntityStateMachine.FixedUpdate () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
[Warning:Tesla Trooper] unsetting. now 0 overrides
[Info   :Tesla Trooper]   at System.Environment.get_StackTrace () [0x00000] in <44afb4564e9347cf99a1865351ea8f4a>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<SetSkillOverride>?69243648._RoR2_GenericSkill::SetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <25459643799e4cd9a3ed7bdddb9af733>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::SetSkillOverride>?1388957696 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <57f520f929fb47d380727fa1409d2fa0>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_SetSkillOverride (On.RoR2.GenericSkill+orig_SetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::SetSkillOverride>?127559168 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <67964cac4f384ff6b53bbe5ecbaee307>:IL_0000
  at AncientScepter.AncientScepterItem.HandleScepterSkill (RoR2.CharacterBody self, System.Boolean forceOff) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at ModdedEntityStates.Desolator.DeployIrradiate.OnExit () [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at RoR2.EntityStateMachine.SetState (EntityStates.EntityState newState) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.EntityStateMachine.FixedUpdate () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
[Warning:Tesla Trooper] setting. now 0 overrides
[Info   :Tesla Trooper]   at System.Environment.get_StackTrace () [0x00000] in <44afb4564e9347cf99a1865351ea8f4a>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_SetSkillOverride (On.RoR2.GenericSkill+orig_SetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::SetSkillOverride>?127559168 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <67964cac4f384ff6b53bbe5ecbaee307>:IL_0000
  at AncientScepter.AncientScepterItem.HandleScepterSkill (RoR2.CharacterBody self, System.Boolean forceOff) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at ModdedEntityStates.Desolator.DeployIrradiate.OnExit () [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at RoR2.EntityStateMachine.SetState (EntityStates.EntityState newState) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.EntityStateMachine.FixedUpdate () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
[Warning:Tesla Trooper] unsetting. now 0 overrides
[Info   :Tesla Trooper]   at System.Environment.get_StackTrace () [0x00000] in <44afb4564e9347cf99a1865351ea8f4a>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at ModdedEntityStates.Desolator.DeployIrradiate.OnExit () [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at RoR2.EntityStateMachine.SetState (EntityStates.EntityState newState) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.EntityStateMachine.FixedUpdate () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
[Warning:Tesla Trooper] unsetting. now 0 overrides
[Info   :Tesla Trooper]   at System.Environment.get_StackTrace () [0x00000] in <44afb4564e9347cf99a1865351ea8f4a>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<SetSkillOverride>?69243648._RoR2_GenericSkill::SetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <25459643799e4cd9a3ed7bdddb9af733>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::SetSkillOverride>?1388957696 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <57f520f929fb47d380727fa1409d2fa0>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_SetSkillOverride (On.RoR2.GenericSkill+orig_SetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::SetSkillOverride>?127559168 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <67964cac4f384ff6b53bbe5ecbaee307>:IL_0000
  at AncientScepter.AncientScepterItem.HandleScepterSkill (RoR2.CharacterBody self, System.Boolean forceOff) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at ModdedEntityStates.Desolator.DeployIrradiate.OnExit () [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at RoR2.EntityStateMachine.SetState (EntityStates.EntityState newState) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.EntityStateMachine.FixedUpdate () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
[Warning:Tesla Trooper] setting. now 0 overrides
[Info   :Tesla Trooper]   at System.Environment.get_StackTrace () [0x00000] in <44afb4564e9347cf99a1865351ea8f4a>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_SetSkillOverride (On.RoR2.GenericSkill+orig_SetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::SetSkillOverride>?127559168 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <67964cac4f384ff6b53bbe5ecbaee307>:IL_0000
  at AncientScepter.AncientScepterItem.HandleScepterSkill (RoR2.CharacterBody self, System.Boolean forceOff) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at ModdedEntityStates.Desolator.DeployIrradiate.OnExit () [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at RoR2.EntityStateMachine.SetState (EntityStates.EntityState newState) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.EntityStateMachine.FixedUpdate () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
[Warning:Tesla Trooper] unsetting. now 0 overrides
[Info   :Tesla Trooper]   at System.Environment.get_StackTrace () [0x00000] in <44afb4564e9347cf99a1865351ea8f4a>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at ModdedEntityStates.Desolator.DeployIrradiate.OnExit () [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at RoR2.EntityStateMachine.SetState (EntityStates.EntityState newState) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.EntityStateMachine.FixedUpdate () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
[Warning:Tesla Trooper] unsetting. now 0 overrides
[Info   :Tesla Trooper]   at System.Environment.get_StackTrace () [0x00000] in <44afb4564e9347cf99a1865351ea8f4a>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<SetSkillOverride>?69243648._RoR2_GenericSkill::SetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <25459643799e4cd9a3ed7bdddb9af733>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::SetSkillOverride>?1388957696 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <57f520f929fb47d380727fa1409d2fa0>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_SetSkillOverride (On.RoR2.GenericSkill+orig_SetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::SetSkillOverride>?127559168 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <67964cac4f384ff6b53bbe5ecbaee307>:IL_0000
  at AncientScepter.AncientScepterItem.HandleScepterSkill (RoR2.CharacterBody self, System.Boolean forceOff) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at ModdedEntityStates.Desolator.DeployIrradiate.OnExit () [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at RoR2.EntityStateMachine.SetState (EntityStates.EntityState newState) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.EntityStateMachine.FixedUpdate () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
[Warning:Tesla Trooper] setting. now 0 overrides
[Info   :Tesla Trooper]   at System.Environment.get_StackTrace () [0x00000] in <44afb4564e9347cf99a1865351ea8f4a>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_SetSkillOverride (On.RoR2.GenericSkill+orig_SetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::SetSkillOverride>?127559168 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <67964cac4f384ff6b53bbe5ecbaee307>:IL_0000
  at AncientScepter.AncientScepterItem.HandleScepterSkill (RoR2.CharacterBody self, System.Boolean forceOff) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at ModdedEntityStates.Desolator.DeployIrradiate.OnExit () [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at RoR2.EntityStateMachine.SetState (EntityStates.EntityState newState) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.EntityStateMachine.FixedUpdate () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
[Warning:Tesla Trooper] unsetting. now 0 overrides
[Info   :Tesla Trooper]   at System.Environment.get_StackTrace () [0x00000] in <44afb4564e9347cf99a1865351ea8f4a>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at ModdedEntityStates.Desolator.DeployIrradiate.OnExit () [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at RoR2.EntityStateMachine.SetState (EntityStates.EntityState newState) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.EntityStateMachine.FixedUpdate () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
[Warning:Tesla Trooper] unsetting. now 0 overrides
[Info   :Tesla Trooper]   at System.Environment.get_StackTrace () [0x00000] in <44afb4564e9347cf99a1865351ea8f4a>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<SetSkillOverride>?69243648._RoR2_GenericSkill::SetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <25459643799e4cd9a3ed7bdddb9af733>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::SetSkillOverride>?1388957696 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <57f520f929fb47d380727fa1409d2fa0>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_SetSkillOverride (On.RoR2.GenericSkill+orig_SetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::SetSkillOverride>?127559168 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <67964cac4f384ff6b53bbe5ecbaee307>:IL_0000
  at AncientScepter.AncientScepterItem.HandleScepterSkill (RoR2.CharacterBody self, System.Boolean forceOff) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at ModdedEntityStates.Desolator.DeployIrradiate.OnExit () [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at RoR2.EntityStateMachine.SetState (EntityStates.EntityState newState) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.EntityStateMachine.FixedUpdate () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
[Warning:Tesla Trooper] setting. now 0 overrides
[Info   :Tesla Trooper]   at System.Environment.get_StackTrace () [0x00000] in <44afb4564e9347cf99a1865351ea8f4a>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_SetSkillOverride (On.RoR2.GenericSkill+orig_SetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::SetSkillOverride>?127559168 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <67964cac4f384ff6b53bbe5ecbaee307>:IL_0000
  at AncientScepter.AncientScepterItem.HandleScepterSkill (RoR2.CharacterBody self, System.Boolean forceOff) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at ModdedEntityStates.Desolator.DeployIrradiate.OnExit () [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at RoR2.EntityStateMachine.SetState (EntityStates.EntityState newState) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.EntityStateMachine.FixedUpdate () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
[Warning:Tesla Trooper] unsetting. now 0 overrides
[Info   :Tesla Trooper]   at System.Environment.get_StackTrace () [0x00000] in <44afb4564e9347cf99a1865351ea8f4a>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at ModdedEntityStates.Desolator.DeployIrradiate.OnExit () [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at RoR2.EntityStateMachine.SetState (EntityStates.EntityState newState) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.EntityStateMachine.FixedUpdate () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
[Warning:Tesla Trooper] unsetting. now 0 overrides
[Info   :Tesla Trooper]   at System.Environment.get_StackTrace () [0x00000] in <44afb4564e9347cf99a1865351ea8f4a>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<SetSkillOverride>?69243648._RoR2_GenericSkill::SetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <25459643799e4cd9a3ed7bdddb9af733>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::SetSkillOverride>?1388957696 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <57f520f929fb47d380727fa1409d2fa0>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_SetSkillOverride (On.RoR2.GenericSkill+orig_SetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::SetSkillOverride>?127559168 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <67964cac4f384ff6b53bbe5ecbaee307>:IL_0000
  at AncientScepter.AncientScepterItem.HandleScepterSkill (RoR2.CharacterBody self, System.Boolean forceOff) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at ModdedEntityStates.Desolator.DeployIrradiate.OnExit () [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at RoR2.EntityStateMachine.SetState (EntityStates.EntityState newState) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.EntityStateMachine.FixedUpdate () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
[Warning:Tesla Trooper] setting. now 0 overrides
[Info   :Tesla Trooper]   at System.Environment.get_StackTrace () [0x00000] in <44afb4564e9347cf99a1865351ea8f4a>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_SetSkillOverride (On.RoR2.GenericSkill+orig_SetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::SetSkillOverride>?127559168 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <67964cac4f384ff6b53bbe5ecbaee307>:IL_0000
  at AncientScepter.AncientScepterItem.HandleScepterSkill (RoR2.CharacterBody self, System.Boolean forceOff) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at ModdedEntityStates.Desolator.DeployIrradiate.OnExit () [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at RoR2.EntityStateMachine.SetState (EntityStates.EntityState newState) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.EntityStateMachine.FixedUpdate () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
[Warning:Tesla Trooper] unsetting. now 0 overrides
[Info   :Tesla Trooper]   at System.Environment.get_StackTrace () [0x00000] in <44afb4564e9347cf99a1865351ea8f4a>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at ModdedEntityStates.Desolator.DeployIrradiate.OnExit () [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at RoR2.EntityStateMachine.SetState (EntityStates.EntityState newState) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.EntityStateMachine.FixedUpdate () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
[Warning:Tesla Trooper] unsetting. now 0 overrides
[Info   :Tesla Trooper]   at System.Environment.get_StackTrace () [0x00000] in <44afb4564e9347cf99a1865351ea8f4a>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<SetSkillOverride>?69243648._RoR2_GenericSkill::SetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <25459643799e4cd9a3ed7bdddb9af733>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::SetSkillOverride>?1388957696 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <57f520f929fb47d380727fa1409d2fa0>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_SetSkillOverride (On.RoR2.GenericSkill+orig_SetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::SetSkillOverride>?127559168 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <67964cac4f384ff6b53bbe5ecbaee307>:IL_0000
  at AncientScepter.AncientScepterItem.HandleScepterSkill (RoR2.CharacterBody self, System.Boolean forceOff) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at ModdedEntityStates.Desolator.DeployIrradiate.OnExit () [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at RoR2.EntityStateMachine.SetState (EntityStates.EntityState newState) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.EntityStateMachine.FixedUpdate () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
[Warning:Tesla Trooper] setting. now 0 overrides
[Info   :Tesla Trooper]   at System.Environment.get_StackTrace () [0x00000] in <44afb4564e9347cf99a1865351ea8f4a>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_SetSkillOverride (On.RoR2.GenericSkill+orig_SetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::SetSkillOverride>?127559168 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <67964cac4f384ff6b53bbe5ecbaee307>:IL_0000
  at AncientScepter.AncientScepterItem.HandleScepterSkill (RoR2.CharacterBody self, System.Boolean forceOff) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at ModdedEntityStates.Desolator.DeployIrradiate.OnExit () [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at RoR2.EntityStateMachine.SetState (EntityStates.EntityState newState) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.EntityStateMachine.FixedUpdate () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
[Warning:Tesla Trooper] unsetting. now 0 overrides
[Info   :Tesla Trooper]   at System.Environment.get_StackTrace () [0x00000] in <44afb4564e9347cf99a1865351ea8f4a>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at ModdedEntityStates.Desolator.DeployIrradiate.OnExit () [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at RoR2.EntityStateMachine.SetState (EntityStates.EntityState newState) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.EntityStateMachine.FixedUpdate () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
[Warning:Tesla Trooper] unsetting. now 0 overrides
[Info   :Tesla Trooper]   at System.Environment.get_StackTrace () [0x00000] in <44afb4564e9347cf99a1865351ea8f4a>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<SetSkillOverride>?69243648._RoR2_GenericSkill::SetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <25459643799e4cd9a3ed7bdddb9af733>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::SetSkillOverride>?1388957696 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <57f520f929fb47d380727fa1409d2fa0>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_SetSkillOverride (On.RoR2.GenericSkill+orig_SetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::SetSkillOverride>?127559168 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <67964cac4f384ff6b53bbe5ecbaee307>:IL_0000
  at AncientScepter.AncientScepterItem.HandleScepterSkill (RoR2.CharacterBody self, System.Boolean forceOff) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at ModdedEntityStates.Desolator.DeployIrradiate.OnExit () [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at RoR2.EntityStateMachine.SetState (EntityStates.EntityState newState) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.EntityStateMachine.FixedUpdate () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
[Warning:Tesla Trooper] setting. now 0 overrides
[Info   :Tesla Trooper]   at System.Environment.get_StackTrace () [0x00000] in <44afb4564e9347cf99a1865351ea8f4a>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_SetSkillOverride (On.RoR2.GenericSkill+orig_SetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::SetSkillOverride>?127559168 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <67964cac4f384ff6b53bbe5ecbaee307>:IL_0000
  at AncientScepter.AncientScepterItem.HandleScepterSkill (RoR2.CharacterBody self, System.Boolean forceOff) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at ModdedEntityStates.Desolator.DeployIrradiate.OnExit () [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at RoR2.EntityStateMachine.SetState (EntityStates.EntityState newState) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.EntityStateMachine.FixedUpdate () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
[Warning:Tesla Trooper] unsetting. now 0 overrides
[Info   :Tesla Trooper]   at System.Environment.get_StackTrace () [0x00000] in <44afb4564e9347cf99a1865351ea8f4a>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<UnsetSkillOverride>?69243648._RoR2_GenericSkill::UnsetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <c92d0a07eb574b7abdce41a41282375b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?-1664615424 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <82a8519b26a54051a5b13884929e9562>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at ModdedEntityStates.Desolator.DeployIrradiate.OnExit () [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at RoR2.EntityStateMachine.SetState (EntityStates.EntityState newState) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.EntityStateMachine.FixedUpdate () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
[Warning:Tesla Trooper] unsetting. now 0 overrides
[Info   :Tesla Trooper]   at System.Environment.get_StackTrace () [0x00000] in <44afb4564e9347cf99a1865351ea8f4a>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<SetSkillOverride>?69243648._RoR2_GenericSkill::SetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <25459643799e4cd9a3ed7bdddb9af733>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::SetSkillOverride>?1388957696 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <57f520f929fb47d380727fa1409d2fa0>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_SetSkillOverride (On.RoR2.GenericSkill+orig_SetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::SetSkillOverride>?127559168 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <67964cac4f384ff6b53bbe5ecbaee307>:IL_0000
  at AncientScepter.AncientScepterItem.HandleScepterSkill (RoR2.CharacterBody self, System.Boolean forceOff) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at ModdedEntityStates.Desolator.DeployIrradiate.OnExit () [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at RoR2.EntityStateMachine.SetState (EntityStates.EntityState newState) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.EntityStateMachine.FixedUpdate () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
[Warning:Tesla Trooper] setting. now 0 overrides
[Info   :Tesla Trooper]   at System.Environment.get_StackTrace () [0x00000] in <44afb4564e9347cf99a1865351ea8f4a>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_SetSkillOverride (On.RoR2.GenericSkill+orig_SetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::SetSkillOverride>?127559168 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <67964cac4f384ff6b53bbe5ecbaee307>:IL_0000
  at AncientScepter.AncientScepterItem.HandleScepterSkill (RoR2.CharacterBody self, System.Boolean forceOff) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at ModdedEntityStates.Desolator.DeployIrradiate.OnExit () [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at RoR2.EntityStateMachine.SetState (EntityStates.EntityState newState) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.EntityStateMachine.FixedUpdate () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
[Warning:Tesla Trooper] unsetting. now 0 overrides
[Info   :Tesla Trooper]   at System.Environment.get_StackTrace () [0x00000] in <44afb4564e9347cf99a1865351ea8f4a>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at ModdedEntityStates.Desolator.DeployIrradiate.OnExit () [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at RoR2.EntityStateMachine.SetState (EntityStates.EntityState newState) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.EntityStateMachine.FixedUpdate () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
[Warning:Tesla Trooper] unsetting. now 0 overrides
[Info   :Tesla Trooper]   at System.Environment.get_StackTrace () [0x00000] in <44afb4564e9347cf99a1865351ea8f4a>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<SetSkillOverride>?69243648._RoR2_GenericSkill::SetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <25459643799e4cd9a3ed7bdddb9af733>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::SetSkillOverride>?1388957696 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <57f520f929fb47d380727fa1409d2fa0>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_SetSkillOverride (On.RoR2.GenericSkill+orig_SetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::SetSkillOverride>?127559168 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <67964cac4f384ff6b53bbe5ecbaee307>:IL_0000
  at AncientScepter.AncientScepterItem.HandleScepterSkill (RoR2.CharacterBody self, System.Boolean forceOff) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at RoR2.CharacterBody.ReplaceSkillIfItemPresent (RoR2.GenericSkill skill, RoR2.ItemIndex itemIndex, RoR2.Skills.SkillDef skillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<OnInventoryChanged>?69243648._RoR2_CharacterBody::OnInventoryChanged (RoR2.CharacterBody this) [0x00000] in <f35d06abecb542c59abe564aada42d55>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::OnInventoryChanged>?-924452864 (RoR2.CharacterBody ) [0x00000] in <b637a1bd61454f7ab5711c37c3b218ee>:IL_0000
  at SillyGlasses.SillyGlassesPlugin.InvChangedHook (On.RoR2.CharacterBody+orig_OnInventoryChanged orig, RoR2.CharacterBody self) [0x00000] in <b1f8657d12fb448c9ef4adffea774a93>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::OnInventoryChanged>?1191347456 (RoR2.CharacterBody ) [0x00000] in <6d0e1c9ef8b14323b22ae4fe85493bdf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::OnInventoryChanged>?1645529088 (RoR2.CharacterBody ) [0x00000] in <b415c93b3cba44c1838e673a31180414>:IL_0000
  at TPDespair.ZetAspects.EffectHooks+<>c.<ApplyAspectBuffOnInventoryChangedHook>b__67_0 (On.RoR2.CharacterBody+orig_OnInventoryChanged orig, RoR2.CharacterBody self) [0x00000] in <4dadcd94303044279e739eddde02c081>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::OnInventoryChanged>?1052907904 (RoR2.CharacterBody ) [0x00000] in <9b5bc9a981d948b98c823e26928e2d80>:IL_0000
  at RoR2.Inventory.HandleInventoryChanged () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.Inventory.GiveItem (RoR2.ItemIndex itemIndex, System.Int32 count) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.Inventory.GiveItem (RoR2.ItemDef itemDef, System.Int32 count) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at TPDespair.ZetAspects.EffectHooks+<>c.<UpdateOnBuffLostHook>b__69_0 (On.RoR2.CharacterBody+orig_OnBuffFinalStackLost orig, RoR2.CharacterBody self, RoR2.BuffDef buffDef) [0x00000] in <4dadcd94303044279e739eddde02c081>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::OnBuffFinalStackLost>?1056918656 (RoR2.CharacterBody , RoR2.BuffDef ) [0x00000] in <3a9b836845dd4e6f845ba18cf6892a5a>:IL_0000
  at RoR2.CharacterBody.SetBuffCount (RoR2.BuffIndex buffType, System.Int32 newCount) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<RemoveBuff>?69243648._RoR2_CharacterBody::RemoveBuff (RoR2.CharacterBody this, RoR2.BuffIndex buffType) [0x00000] in <c2a05f6afefc4b9d8a4f8c06bb19180b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::RemoveBuff>?1506109440 (RoR2.CharacterBody , RoR2.BuffIndex ) [0x00000] in <00bd3db27e8c4d6196343ffe0ce8a458>:IL_0000
  at TPDespair.ZetAspects.EffectHooks.VoidBearFix_RemoveBuff (On.RoR2.CharacterBody+orig_RemoveBuff_BuffIndex orig, RoR2.CharacterBody self, RoR2.BuffIndex buffType) [0x00000] in <4dadcd94303044279e739eddde02c081>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::RemoveBuff>?1765697024 (RoR2.CharacterBody , RoR2.BuffIndex ) [0x00000] in <a95d742e1da9454aa3080782b9781f25>:IL_0000
  at DMD<UpdateBuffs>?69243648._RoR2_CharacterBody::UpdateBuffs (RoR2.CharacterBody this, System.Single deltaTime) [0x00000] in <7a3821e858c54758888b3b663d3bc096>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::UpdateBuffs>?-1311594496 (RoR2.CharacterBody , System.Single ) [0x00000] in <60758834c88748ea9844b544d73a0a74>:IL_0000
  at AncientScepter.HereticNevermore2.CharacterBody_UpdateBuffs (On.RoR2.CharacterBody+orig_UpdateBuffs orig, RoR2.CharacterBody self, System.Single deltaTime) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::UpdateBuffs>?1154037760 (RoR2.CharacterBody , System.Single ) [0x00000] in <f3a90d4f83034e03bc4ddf4ab6cb43bb>:IL_0000
  at RoR2.CharacterBody.FixedUpdate () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
[Warning:Tesla Trooper] setting. now 0 overrides
[Info   :Tesla Trooper]   at System.Environment.get_StackTrace () [0x00000] in <44afb4564e9347cf99a1865351ea8f4a>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_SetSkillOverride (On.RoR2.GenericSkill+orig_SetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::SetSkillOverride>?127559168 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <67964cac4f384ff6b53bbe5ecbaee307>:IL_0000
  at AncientScepter.AncientScepterItem.HandleScepterSkill (RoR2.CharacterBody self, System.Boolean forceOff) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at RoR2.CharacterBody.ReplaceSkillIfItemPresent (RoR2.GenericSkill skill, RoR2.ItemIndex itemIndex, RoR2.Skills.SkillDef skillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<OnInventoryChanged>?69243648._RoR2_CharacterBody::OnInventoryChanged (RoR2.CharacterBody this) [0x00000] in <f35d06abecb542c59abe564aada42d55>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::OnInventoryChanged>?-924452864 (RoR2.CharacterBody ) [0x00000] in <b637a1bd61454f7ab5711c37c3b218ee>:IL_0000
  at SillyGlasses.SillyGlassesPlugin.InvChangedHook (On.RoR2.CharacterBody+orig_OnInventoryChanged orig, RoR2.CharacterBody self) [0x00000] in <b1f8657d12fb448c9ef4adffea774a93>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::OnInventoryChanged>?1191347456 (RoR2.CharacterBody ) [0x00000] in <6d0e1c9ef8b14323b22ae4fe85493bdf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::OnInventoryChanged>?1645529088 (RoR2.CharacterBody ) [0x00000] in <b415c93b3cba44c1838e673a31180414>:IL_0000
  at TPDespair.ZetAspects.EffectHooks+<>c.<ApplyAspectBuffOnInventoryChangedHook>b__67_0 (On.RoR2.CharacterBody+orig_OnInventoryChanged orig, RoR2.CharacterBody self) [0x00000] in <4dadcd94303044279e739eddde02c081>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::OnInventoryChanged>?1052907904 (RoR2.CharacterBody ) [0x00000] in <9b5bc9a981d948b98c823e26928e2d80>:IL_0000
  at RoR2.Inventory.HandleInventoryChanged () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.Inventory.GiveItem (RoR2.ItemIndex itemIndex, System.Int32 count) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.Inventory.GiveItem (RoR2.ItemDef itemDef, System.Int32 count) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at TPDespair.ZetAspects.EffectHooks+<>c.<UpdateOnBuffLostHook>b__69_0 (On.RoR2.CharacterBody+orig_OnBuffFinalStackLost orig, RoR2.CharacterBody self, RoR2.BuffDef buffDef) [0x00000] in <4dadcd94303044279e739eddde02c081>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::OnBuffFinalStackLost>?1056918656 (RoR2.CharacterBody , RoR2.BuffDef ) [0x00000] in <3a9b836845dd4e6f845ba18cf6892a5a>:IL_0000
  at RoR2.CharacterBody.SetBuffCount (RoR2.BuffIndex buffType, System.Int32 newCount) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<RemoveBuff>?69243648._RoR2_CharacterBody::RemoveBuff (RoR2.CharacterBody this, RoR2.BuffIndex buffType) [0x00000] in <c2a05f6afefc4b9d8a4f8c06bb19180b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::RemoveBuff>?1506109440 (RoR2.CharacterBody , RoR2.BuffIndex ) [0x00000] in <00bd3db27e8c4d6196343ffe0ce8a458>:IL_0000
  at TPDespair.ZetAspects.EffectHooks.VoidBearFix_RemoveBuff (On.RoR2.CharacterBody+orig_RemoveBuff_BuffIndex orig, RoR2.CharacterBody self, RoR2.BuffIndex buffType) [0x00000] in <4dadcd94303044279e739eddde02c081>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::RemoveBuff>?1765697024 (RoR2.CharacterBody , RoR2.BuffIndex ) [0x00000] in <a95d742e1da9454aa3080782b9781f25>:IL_0000
  at DMD<UpdateBuffs>?69243648._RoR2_CharacterBody::UpdateBuffs (RoR2.CharacterBody this, System.Single deltaTime) [0x00000] in <7a3821e858c54758888b3b663d3bc096>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::UpdateBuffs>?-1311594496 (RoR2.CharacterBody , System.Single ) [0x00000] in <60758834c88748ea9844b544d73a0a74>:IL_0000
  at AncientScepter.HereticNevermore2.CharacterBody_UpdateBuffs (On.RoR2.CharacterBody+orig_UpdateBuffs orig, RoR2.CharacterBody self, System.Single deltaTime) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::UpdateBuffs>?1154037760 (RoR2.CharacterBody , System.Single ) [0x00000] in <f3a90d4f83034e03bc4ddf4ab6cb43bb>:IL_0000
  at RoR2.CharacterBody.FixedUpdate () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
[Warning:Tesla Trooper] unsetting. now 0 overrides
[Info   :Tesla Trooper]   at System.Environment.get_StackTrace () [0x00000] in <44afb4564e9347cf99a1865351ea8f4a>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<SetSkillOverride>?69243648._RoR2_GenericSkill::SetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <25459643799e4cd9a3ed7bdddb9af733>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::SetSkillOverride>?1388957696 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <57f520f929fb47d380727fa1409d2fa0>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_SetSkillOverride (On.RoR2.GenericSkill+orig_SetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::SetSkillOverride>?127559168 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <67964cac4f384ff6b53bbe5ecbaee307>:IL_0000
  at AncientScepter.AncientScepterItem.HandleScepterSkill (RoR2.CharacterBody self, System.Boolean forceOff) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at RoR2.CharacterBody.ReplaceSkillIfItemPresent (RoR2.GenericSkill skill, RoR2.ItemIndex itemIndex, RoR2.Skills.SkillDef skillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<OnInventoryChanged>?69243648._RoR2_CharacterBody::OnInventoryChanged (RoR2.CharacterBody this) [0x00000] in <f35d06abecb542c59abe564aada42d55>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::OnInventoryChanged>?-924452864 (RoR2.CharacterBody ) [0x00000] in <b637a1bd61454f7ab5711c37c3b218ee>:IL_0000
  at SillyGlasses.SillyGlassesPlugin.InvChangedHook (On.RoR2.CharacterBody+orig_OnInventoryChanged orig, RoR2.CharacterBody self) [0x00000] in <b1f8657d12fb448c9ef4adffea774a93>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::OnInventoryChanged>?1191347456 (RoR2.CharacterBody ) [0x00000] in <6d0e1c9ef8b14323b22ae4fe85493bdf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::OnInventoryChanged>?1645529088 (RoR2.CharacterBody ) [0x00000] in <b415c93b3cba44c1838e673a31180414>:IL_0000
  at TPDespair.ZetAspects.EffectHooks+<>c.<ApplyAspectBuffOnInventoryChangedHook>b__67_0 (On.RoR2.CharacterBody+orig_OnInventoryChanged orig, RoR2.CharacterBody self) [0x00000] in <4dadcd94303044279e739eddde02c081>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::OnInventoryChanged>?1052907904 (RoR2.CharacterBody ) [0x00000] in <9b5bc9a981d948b98c823e26928e2d80>:IL_0000
  at RoR2.Inventory.HandleInventoryChanged () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.Inventory.GiveItem (RoR2.ItemIndex itemIndex, System.Int32 count) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.Inventory.GiveItem (RoR2.ItemDef itemDef, System.Int32 count) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at TPDespair.ZetAspects.EffectHooks+<>c.<UpdateOnBuffLostHook>b__69_0 (On.RoR2.CharacterBody+orig_OnBuffFinalStackLost orig, RoR2.CharacterBody self, RoR2.BuffDef buffDef) [0x00000] in <4dadcd94303044279e739eddde02c081>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::OnBuffFinalStackLost>?1056918656 (RoR2.CharacterBody , RoR2.BuffDef ) [0x00000] in <3a9b836845dd4e6f845ba18cf6892a5a>:IL_0000
  at RoR2.CharacterBody.SetBuffCount (RoR2.BuffIndex buffType, System.Int32 newCount) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<RemoveBuff>?69243648._RoR2_CharacterBody::RemoveBuff (RoR2.CharacterBody this, RoR2.BuffIndex buffType) [0x00000] in <c2a05f6afefc4b9d8a4f8c06bb19180b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::RemoveBuff>?1506109440 (RoR2.CharacterBody , RoR2.BuffIndex ) [0x00000] in <00bd3db27e8c4d6196343ffe0ce8a458>:IL_0000
  at TPDespair.ZetAspects.EffectHooks.VoidBearFix_RemoveBuff (On.RoR2.CharacterBody+orig_RemoveBuff_BuffIndex orig, RoR2.CharacterBody self, RoR2.BuffIndex buffType) [0x00000] in <4dadcd94303044279e739eddde02c081>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::RemoveBuff>?1765697024 (RoR2.CharacterBody , RoR2.BuffIndex ) [0x00000] in <a95d742e1da9454aa3080782b9781f25>:IL_0000
  at DMD<UpdateBuffs>?69243648._RoR2_CharacterBody::UpdateBuffs (RoR2.CharacterBody this, System.Single deltaTime) [0x00000] in <7a3821e858c54758888b3b663d3bc096>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::UpdateBuffs>?-1311594496 (RoR2.CharacterBody , System.Single ) [0x00000] in <60758834c88748ea9844b544d73a0a74>:IL_0000
  at AncientScepter.HereticNevermore2.CharacterBody_UpdateBuffs (On.RoR2.CharacterBody+orig_UpdateBuffs orig, RoR2.CharacterBody self, System.Single deltaTime) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::UpdateBuffs>?1154037760 (RoR2.CharacterBody , System.Single ) [0x00000] in <f3a90d4f83034e03bc4ddf4ab6cb43bb>:IL_0000
  at RoR2.CharacterBody.FixedUpdate () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
[Warning:Tesla Trooper] setting. now 0 overrides
[Info   :Tesla Trooper]   at System.Environment.get_StackTrace () [0x00000] in <44afb4564e9347cf99a1865351ea8f4a>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_SetSkillOverride (On.RoR2.GenericSkill+orig_SetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::SetSkillOverride>?127559168 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <67964cac4f384ff6b53bbe5ecbaee307>:IL_0000
  at AncientScepter.AncientScepterItem.HandleScepterSkill (RoR2.CharacterBody self, System.Boolean forceOff) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at RoR2.CharacterBody.ReplaceSkillIfItemPresent (RoR2.GenericSkill skill, RoR2.ItemIndex itemIndex, RoR2.Skills.SkillDef skillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<OnInventoryChanged>?69243648._RoR2_CharacterBody::OnInventoryChanged (RoR2.CharacterBody this) [0x00000] in <f35d06abecb542c59abe564aada42d55>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::OnInventoryChanged>?-924452864 (RoR2.CharacterBody ) [0x00000] in <b637a1bd61454f7ab5711c37c3b218ee>:IL_0000
  at SillyGlasses.SillyGlassesPlugin.InvChangedHook (On.RoR2.CharacterBody+orig_OnInventoryChanged orig, RoR2.CharacterBody self) [0x00000] in <b1f8657d12fb448c9ef4adffea774a93>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::OnInventoryChanged>?1191347456 (RoR2.CharacterBody ) [0x00000] in <6d0e1c9ef8b14323b22ae4fe85493bdf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::OnInventoryChanged>?1645529088 (RoR2.CharacterBody ) [0x00000] in <b415c93b3cba44c1838e673a31180414>:IL_0000
  at TPDespair.ZetAspects.EffectHooks+<>c.<ApplyAspectBuffOnInventoryChangedHook>b__67_0 (On.RoR2.CharacterBody+orig_OnInventoryChanged orig, RoR2.CharacterBody self) [0x00000] in <4dadcd94303044279e739eddde02c081>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::OnInventoryChanged>?1052907904 (RoR2.CharacterBody ) [0x00000] in <9b5bc9a981d948b98c823e26928e2d80>:IL_0000
  at RoR2.Inventory.HandleInventoryChanged () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.Inventory.GiveItem (RoR2.ItemIndex itemIndex, System.Int32 count) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.Inventory.GiveItem (RoR2.ItemDef itemDef, System.Int32 count) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at TPDespair.ZetAspects.EffectHooks+<>c.<UpdateOnBuffLostHook>b__69_0 (On.RoR2.CharacterBody+orig_OnBuffFinalStackLost orig, RoR2.CharacterBody self, RoR2.BuffDef buffDef) [0x00000] in <4dadcd94303044279e739eddde02c081>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::OnBuffFinalStackLost>?1056918656 (RoR2.CharacterBody , RoR2.BuffDef ) [0x00000] in <3a9b836845dd4e6f845ba18cf6892a5a>:IL_0000
  at RoR2.CharacterBody.SetBuffCount (RoR2.BuffIndex buffType, System.Int32 newCount) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<RemoveBuff>?69243648._RoR2_CharacterBody::RemoveBuff (RoR2.CharacterBody this, RoR2.BuffIndex buffType) [0x00000] in <c2a05f6afefc4b9d8a4f8c06bb19180b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::RemoveBuff>?1506109440 (RoR2.CharacterBody , RoR2.BuffIndex ) [0x00000] in <00bd3db27e8c4d6196343ffe0ce8a458>:IL_0000
  at TPDespair.ZetAspects.EffectHooks.VoidBearFix_RemoveBuff (On.RoR2.CharacterBody+orig_RemoveBuff_BuffIndex orig, RoR2.CharacterBody self, RoR2.BuffIndex buffType) [0x00000] in <4dadcd94303044279e739eddde02c081>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::RemoveBuff>?1765697024 (RoR2.CharacterBody , RoR2.BuffIndex ) [0x00000] in <a95d742e1da9454aa3080782b9781f25>:IL_0000
  at DMD<UpdateBuffs>?69243648._RoR2_CharacterBody::UpdateBuffs (RoR2.CharacterBody this, System.Single deltaTime) [0x00000] in <7a3821e858c54758888b3b663d3bc096>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::UpdateBuffs>?-1311594496 (RoR2.CharacterBody , System.Single ) [0x00000] in <60758834c88748ea9844b544d73a0a74>:IL_0000
  at AncientScepter.HereticNevermore2.CharacterBody_UpdateBuffs (On.RoR2.CharacterBody+orig_UpdateBuffs orig, RoR2.CharacterBody self, System.Single deltaTime) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::UpdateBuffs>?1154037760 (RoR2.CharacterBody , System.Single ) [0x00000] in <f3a90d4f83034e03bc4ddf4ab6cb43bb>:IL_0000
  at RoR2.CharacterBody.FixedUpdate () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
[Warning:Tesla Trooper] unsetting. now 0 overrides
[Info   :Tesla Trooper]   at System.Environment.get_StackTrace () [0x00000] in <44afb4564e9347cf99a1865351ea8f4a>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<SetSkillOverride>?69243648._RoR2_GenericSkill::SetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <25459643799e4cd9a3ed7bdddb9af733>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::SetSkillOverride>?1388957696 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <57f520f929fb47d380727fa1409d2fa0>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_SetSkillOverride (On.RoR2.GenericSkill+orig_SetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::SetSkillOverride>?127559168 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <67964cac4f384ff6b53bbe5ecbaee307>:IL_0000
  at AncientScepter.AncientScepterItem.HandleScepterSkill (RoR2.CharacterBody self, System.Boolean forceOff) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at RoR2.CharacterBody.ReplaceSkillIfItemPresent (RoR2.GenericSkill skill, RoR2.ItemIndex itemIndex, RoR2.Skills.SkillDef skillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<OnInventoryChanged>?69243648._RoR2_CharacterBody::OnInventoryChanged (RoR2.CharacterBody this) [0x00000] in <f35d06abecb542c59abe564aada42d55>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::OnInventoryChanged>?-924452864 (RoR2.CharacterBody ) [0x00000] in <b637a1bd61454f7ab5711c37c3b218ee>:IL_0000
  at SillyGlasses.SillyGlassesPlugin.InvChangedHook (On.RoR2.CharacterBody+orig_OnInventoryChanged orig, RoR2.CharacterBody self) [0x00000] in <b1f8657d12fb448c9ef4adffea774a93>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::OnInventoryChanged>?1191347456 (RoR2.CharacterBody ) [0x00000] in <6d0e1c9ef8b14323b22ae4fe85493bdf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::OnInventoryChanged>?1645529088 (RoR2.CharacterBody ) [0x00000] in <b415c93b3cba44c1838e673a31180414>:IL_0000
  at TPDespair.ZetAspects.EffectHooks+<>c.<ApplyAspectBuffOnInventoryChangedHook>b__67_0 (On.RoR2.CharacterBody+orig_OnInventoryChanged orig, RoR2.CharacterBody self) [0x00000] in <4dadcd94303044279e739eddde02c081>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::OnInventoryChanged>?1052907904 (RoR2.CharacterBody ) [0x00000] in <9b5bc9a981d948b98c823e26928e2d80>:IL_0000
  at RoR2.Inventory.HandleInventoryChanged () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.Inventory.GiveItem (RoR2.ItemIndex itemIndex, System.Int32 count) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.Inventory.GiveItem (RoR2.ItemDef itemDef, System.Int32 count) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at TPDespair.ZetAspects.EffectHooks+<>c.<UpdateOnBuffLostHook>b__69_0 (On.RoR2.CharacterBody+orig_OnBuffFinalStackLost orig, RoR2.CharacterBody self, RoR2.BuffDef buffDef) [0x00000] in <4dadcd94303044279e739eddde02c081>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::OnBuffFinalStackLost>?1056918656 (RoR2.CharacterBody , RoR2.BuffDef ) [0x00000] in <3a9b836845dd4e6f845ba18cf6892a5a>:IL_0000
  at RoR2.CharacterBody.SetBuffCount (RoR2.BuffIndex buffType, System.Int32 newCount) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<RemoveBuff>?69243648._RoR2_CharacterBody::RemoveBuff (RoR2.CharacterBody this, RoR2.BuffIndex buffType) [0x00000] in <c2a05f6afefc4b9d8a4f8c06bb19180b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::RemoveBuff>?1506109440 (RoR2.CharacterBody , RoR2.BuffIndex ) [0x00000] in <00bd3db27e8c4d6196343ffe0ce8a458>:IL_0000
  at TPDespair.ZetAspects.EffectHooks.VoidBearFix_RemoveBuff (On.RoR2.CharacterBody+orig_RemoveBuff_BuffIndex orig, RoR2.CharacterBody self, RoR2.BuffIndex buffType) [0x00000] in <4dadcd94303044279e739eddde02c081>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::RemoveBuff>?1765697024 (RoR2.CharacterBody , RoR2.BuffIndex ) [0x00000] in <a95d742e1da9454aa3080782b9781f25>:IL_0000
  at DMD<UpdateBuffs>?69243648._RoR2_CharacterBody::UpdateBuffs (RoR2.CharacterBody this, System.Single deltaTime) [0x00000] in <7a3821e858c54758888b3b663d3bc096>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::UpdateBuffs>?-1311594496 (RoR2.CharacterBody , System.Single ) [0x00000] in <60758834c88748ea9844b544d73a0a74>:IL_0000
  at AncientScepter.HereticNevermore2.CharacterBody_UpdateBuffs (On.RoR2.CharacterBody+orig_UpdateBuffs orig, RoR2.CharacterBody self, System.Single deltaTime) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::UpdateBuffs>?1154037760 (RoR2.CharacterBody , System.Single ) [0x00000] in <f3a90d4f83034e03bc4ddf4ab6cb43bb>:IL_0000
  at RoR2.CharacterBody.FixedUpdate () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
[Warning:Tesla Trooper] setting. now 0 overrides
[Info   :Tesla Trooper]   at System.Environment.get_StackTrace () [0x00000] in <44afb4564e9347cf99a1865351ea8f4a>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_SetSkillOverride (On.RoR2.GenericSkill+orig_SetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::SetSkillOverride>?127559168 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <67964cac4f384ff6b53bbe5ecbaee307>:IL_0000
  at AncientScepter.AncientScepterItem.HandleScepterSkill (RoR2.CharacterBody self, System.Boolean forceOff) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at RoR2.CharacterBody.ReplaceSkillIfItemPresent (RoR2.GenericSkill skill, RoR2.ItemIndex itemIndex, RoR2.Skills.SkillDef skillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<OnInventoryChanged>?69243648._RoR2_CharacterBody::OnInventoryChanged (RoR2.CharacterBody this) [0x00000] in <f35d06abecb542c59abe564aada42d55>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::OnInventoryChanged>?-924452864 (RoR2.CharacterBody ) [0x00000] in <b637a1bd61454f7ab5711c37c3b218ee>:IL_0000
  at SillyGlasses.SillyGlassesPlugin.InvChangedHook (On.RoR2.CharacterBody+orig_OnInventoryChanged orig, RoR2.CharacterBody self) [0x00000] in <b1f8657d12fb448c9ef4adffea774a93>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::OnInventoryChanged>?1191347456 (RoR2.CharacterBody ) [0x00000] in <6d0e1c9ef8b14323b22ae4fe85493bdf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::OnInventoryChanged>?1645529088 (RoR2.CharacterBody ) [0x00000] in <b415c93b3cba44c1838e673a31180414>:IL_0000
  at TPDespair.ZetAspects.EffectHooks+<>c.<ApplyAspectBuffOnInventoryChangedHook>b__67_0 (On.RoR2.CharacterBody+orig_OnInventoryChanged orig, RoR2.CharacterBody self) [0x00000] in <4dadcd94303044279e739eddde02c081>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::OnInventoryChanged>?1052907904 (RoR2.CharacterBody ) [0x00000] in <9b5bc9a981d948b98c823e26928e2d80>:IL_0000
  at RoR2.Inventory.HandleInventoryChanged () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.Inventory.GiveItem (RoR2.ItemIndex itemIndex, System.Int32 count) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.Inventory.GiveItem (RoR2.ItemDef itemDef, System.Int32 count) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at TPDespair.ZetAspects.EffectHooks+<>c.<UpdateOnBuffLostHook>b__69_0 (On.RoR2.CharacterBody+orig_OnBuffFinalStackLost orig, RoR2.CharacterBody self, RoR2.BuffDef buffDef) [0x00000] in <4dadcd94303044279e739eddde02c081>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::OnBuffFinalStackLost>?1056918656 (RoR2.CharacterBody , RoR2.BuffDef ) [0x00000] in <3a9b836845dd4e6f845ba18cf6892a5a>:IL_0000
  at RoR2.CharacterBody.SetBuffCount (RoR2.BuffIndex buffType, System.Int32 newCount) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<RemoveBuff>?69243648._RoR2_CharacterBody::RemoveBuff (RoR2.CharacterBody this, RoR2.BuffIndex buffType) [0x00000] in <c2a05f6afefc4b9d8a4f8c06bb19180b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::RemoveBuff>?1506109440 (RoR2.CharacterBody , RoR2.BuffIndex ) [0x00000] in <00bd3db27e8c4d6196343ffe0ce8a458>:IL_0000
  at TPDespair.ZetAspects.EffectHooks.VoidBearFix_RemoveBuff (On.RoR2.CharacterBody+orig_RemoveBuff_BuffIndex orig, RoR2.CharacterBody self, RoR2.BuffIndex buffType) [0x00000] in <4dadcd94303044279e739eddde02c081>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::RemoveBuff>?1765697024 (RoR2.CharacterBody , RoR2.BuffIndex ) [0x00000] in <a95d742e1da9454aa3080782b9781f25>:IL_0000
  at DMD<UpdateBuffs>?69243648._RoR2_CharacterBody::UpdateBuffs (RoR2.CharacterBody this, System.Single deltaTime) [0x00000] in <7a3821e858c54758888b3b663d3bc096>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::UpdateBuffs>?-1311594496 (RoR2.CharacterBody , System.Single ) [0x00000] in <60758834c88748ea9844b544d73a0a74>:IL_0000
  at AncientScepter.HereticNevermore2.CharacterBody_UpdateBuffs (On.RoR2.CharacterBody+orig_UpdateBuffs orig, RoR2.CharacterBody self, System.Single deltaTime) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::UpdateBuffs>?1154037760 (RoR2.CharacterBody , System.Single ) [0x00000] in <f3a90d4f83034e03bc4ddf4ab6cb43bb>:IL_0000
  at RoR2.CharacterBody.FixedUpdate () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
[Warning:Tesla Trooper] unsetting. now 0 overrides
[Info   :Tesla Trooper]   at System.Environment.get_StackTrace () [0x00000] in <44afb4564e9347cf99a1865351ea8f4a>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at RoR2.CharacterBody.ReplaceSkillIfItemPresent (RoR2.GenericSkill skill, RoR2.ItemIndex itemIndex, RoR2.Skills.SkillDef skillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<OnInventoryChanged>?69243648._RoR2_CharacterBody::OnInventoryChanged (RoR2.CharacterBody this) [0x00000] in <f35d06abecb542c59abe564aada42d55>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::OnInventoryChanged>?-924452864 (RoR2.CharacterBody ) [0x00000] in <b637a1bd61454f7ab5711c37c3b218ee>:IL_0000
  at SillyGlasses.SillyGlassesPlugin.InvChangedHook (On.RoR2.CharacterBody+orig_OnInventoryChanged orig, RoR2.CharacterBody self) [0x00000] in <b1f8657d12fb448c9ef4adffea774a93>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::OnInventoryChanged>?1191347456 (RoR2.CharacterBody ) [0x00000] in <6d0e1c9ef8b14323b22ae4fe85493bdf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::OnInventoryChanged>?1645529088 (RoR2.CharacterBody ) [0x00000] in <b415c93b3cba44c1838e673a31180414>:IL_0000
  at TPDespair.ZetAspects.EffectHooks+<>c.<ApplyAspectBuffOnInventoryChangedHook>b__67_0 (On.RoR2.CharacterBody+orig_OnInventoryChanged orig, RoR2.CharacterBody self) [0x00000] in <4dadcd94303044279e739eddde02c081>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::OnInventoryChanged>?1052907904 (RoR2.CharacterBody ) [0x00000] in <9b5bc9a981d948b98c823e26928e2d80>:IL_0000
  at RoR2.Inventory.HandleInventoryChanged () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.Inventory.GiveItem (RoR2.ItemIndex itemIndex, System.Int32 count) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.Inventory.GiveItem (RoR2.ItemDef itemDef, System.Int32 count) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at TPDespair.ZetAspects.EffectHooks+<>c.<UpdateOnBuffLostHook>b__69_0 (On.RoR2.CharacterBody+orig_OnBuffFinalStackLost orig, RoR2.CharacterBody self, RoR2.BuffDef buffDef) [0x00000] in <4dadcd94303044279e739eddde02c081>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::OnBuffFinalStackLost>?1056918656 (RoR2.CharacterBody , RoR2.BuffDef ) [0x00000] in <3a9b836845dd4e6f845ba18cf6892a5a>:IL_0000
  at RoR2.CharacterBody.SetBuffCount (RoR2.BuffIndex buffType, System.Int32 newCount) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<RemoveBuff>?69243648._RoR2_CharacterBody::RemoveBuff (RoR2.CharacterBody this, RoR2.BuffIndex buffType) [0x00000] in <c2a05f6afefc4b9d8a4f8c06bb19180b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::RemoveBuff>?1506109440 (RoR2.CharacterBody , RoR2.BuffIndex ) [0x00000] in <00bd3db27e8c4d6196343ffe0ce8a458>:IL_0000
  at TPDespair.ZetAspects.EffectHooks.VoidBearFix_RemoveBuff (On.RoR2.CharacterBody+orig_RemoveBuff_BuffIndex orig, RoR2.CharacterBody self, RoR2.BuffIndex buffType) [0x00000] in <4dadcd94303044279e739eddde02c081>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::RemoveBuff>?1765697024 (RoR2.CharacterBody , RoR2.BuffIndex ) [0x00000] in <a95d742e1da9454aa3080782b9781f25>:IL_0000
  at DMD<UpdateBuffs>?69243648._RoR2_CharacterBody::UpdateBuffs (RoR2.CharacterBody this, System.Single deltaTime) [0x00000] in <7a3821e858c54758888b3b663d3bc096>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::UpdateBuffs>?-1311594496 (RoR2.CharacterBody , System.Single ) [0x00000] in <60758834c88748ea9844b544d73a0a74>:IL_0000
  at AncientScepter.HereticNevermore2.CharacterBody_UpdateBuffs (On.RoR2.CharacterBody+orig_UpdateBuffs orig, RoR2.CharacterBody self, System.Single deltaTime) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::UpdateBuffs>?1154037760 (RoR2.CharacterBody , System.Single ) [0x00000] in <f3a90d4f83034e03bc4ddf4ab6cb43bb>:IL_0000
  at RoR2.CharacterBody.FixedUpdate () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
[Warning:Tesla Trooper] unsetting. now 0 overrides
[Info   :Tesla Trooper]   at System.Environment.get_StackTrace () [0x00000] in <44afb4564e9347cf99a1865351ea8f4a>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<SetSkillOverride>?69243648._RoR2_GenericSkill::SetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <25459643799e4cd9a3ed7bdddb9af733>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::SetSkillOverride>?1388957696 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <57f520f929fb47d380727fa1409d2fa0>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_SetSkillOverride (On.RoR2.GenericSkill+orig_SetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::SetSkillOverride>?127559168 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <67964cac4f384ff6b53bbe5ecbaee307>:IL_0000
  at AncientScepter.AncientScepterItem.HandleScepterSkill (RoR2.CharacterBody self, System.Boolean forceOff) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at RoR2.CharacterBody.ReplaceSkillIfItemPresent (RoR2.GenericSkill skill, RoR2.ItemIndex itemIndex, RoR2.Skills.SkillDef skillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<OnInventoryChanged>?69243648._RoR2_CharacterBody::OnInventoryChanged (RoR2.CharacterBody this) [0x00000] in <f35d06abecb542c59abe564aada42d55>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::OnInventoryChanged>?-924452864 (RoR2.CharacterBody ) [0x00000] in <b637a1bd61454f7ab5711c37c3b218ee>:IL_0000
  at SillyGlasses.SillyGlassesPlugin.InvChangedHook (On.RoR2.CharacterBody+orig_OnInventoryChanged orig, RoR2.CharacterBody self) [0x00000] in <b1f8657d12fb448c9ef4adffea774a93>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::OnInventoryChanged>?1191347456 (RoR2.CharacterBody ) [0x00000] in <6d0e1c9ef8b14323b22ae4fe85493bdf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::OnInventoryChanged>?1645529088 (RoR2.CharacterBody ) [0x00000] in <b415c93b3cba44c1838e673a31180414>:IL_0000
  at TPDespair.ZetAspects.EffectHooks+<>c.<ApplyAspectBuffOnInventoryChangedHook>b__67_0 (On.RoR2.CharacterBody+orig_OnInventoryChanged orig, RoR2.CharacterBody self) [0x00000] in <4dadcd94303044279e739eddde02c081>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::OnInventoryChanged>?1052907904 (RoR2.CharacterBody ) [0x00000] in <9b5bc9a981d948b98c823e26928e2d80>:IL_0000
  at RoR2.Inventory.HandleInventoryChanged () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.Inventory.GiveItem (RoR2.ItemIndex itemIndex, System.Int32 count) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.Inventory.GiveItem (RoR2.ItemDef itemDef, System.Int32 count) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at TPDespair.ZetAspects.EffectHooks+<>c.<UpdateOnBuffLostHook>b__69_0 (On.RoR2.CharacterBody+orig_OnBuffFinalStackLost orig, RoR2.CharacterBody self, RoR2.BuffDef buffDef) [0x00000] in <4dadcd94303044279e739eddde02c081>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::OnBuffFinalStackLost>?1056918656 (RoR2.CharacterBody , RoR2.BuffDef ) [0x00000] in <3a9b836845dd4e6f845ba18cf6892a5a>:IL_0000
  at RoR2.CharacterBody.SetBuffCount (RoR2.BuffIndex buffType, System.Int32 newCount) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<RemoveBuff>?69243648._RoR2_CharacterBody::RemoveBuff (RoR2.CharacterBody this, RoR2.BuffIndex buffType) [0x00000] in <c2a05f6afefc4b9d8a4f8c06bb19180b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::RemoveBuff>?1506109440 (RoR2.CharacterBody , RoR2.BuffIndex ) [0x00000] in <00bd3db27e8c4d6196343ffe0ce8a458>:IL_0000
  at TPDespair.ZetAspects.EffectHooks.VoidBearFix_RemoveBuff (On.RoR2.CharacterBody+orig_RemoveBuff_BuffIndex orig, RoR2.CharacterBody self, RoR2.BuffIndex buffType) [0x00000] in <4dadcd94303044279e739eddde02c081>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::RemoveBuff>?1765697024 (RoR2.CharacterBody , RoR2.BuffIndex ) [0x00000] in <a95d742e1da9454aa3080782b9781f25>:IL_0000
  at DMD<UpdateBuffs>?69243648._RoR2_CharacterBody::UpdateBuffs (RoR2.CharacterBody this, System.Single deltaTime) [0x00000] in <7a3821e858c54758888b3b663d3bc096>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::UpdateBuffs>?-1311594496 (RoR2.CharacterBody , System.Single ) [0x00000] in <60758834c88748ea9844b544d73a0a74>:IL_0000
  at AncientScepter.HereticNevermore2.CharacterBody_UpdateBuffs (On.RoR2.CharacterBody+orig_UpdateBuffs orig, RoR2.CharacterBody self, System.Single deltaTime) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::UpdateBuffs>?1154037760 (RoR2.CharacterBody , System.Single ) [0x00000] in <f3a90d4f83034e03bc4ddf4ab6cb43bb>:IL_0000
  at RoR2.CharacterBody.FixedUpdate () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
[Warning:Tesla Trooper] setting. now 0 overrides
[Info   :Tesla Trooper]   at System.Environment.get_StackTrace () [0x00000] in <44afb4564e9347cf99a1865351ea8f4a>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_SetSkillOverride (On.RoR2.GenericSkill+orig_SetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::SetSkillOverride>?127559168 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <67964cac4f384ff6b53bbe5ecbaee307>:IL_0000
  at AncientScepter.AncientScepterItem.HandleScepterSkill (RoR2.CharacterBody self, System.Boolean forceOff) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at AncientScepter.AncientScepterItem.On_GSUnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?-64612096 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <1fa6a4169e28454a9f3531af201bc6bf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::UnsetSkillOverride>?953390080 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <0d443271681e44318594871e314bee5b>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at RoR2.CharacterBody.ReplaceSkillIfItemPresent (RoR2.GenericSkill skill, RoR2.ItemIndex itemIndex, RoR2.Skills.SkillDef skillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<OnInventoryChanged>?69243648._RoR2_CharacterBody::OnInventoryChanged (RoR2.CharacterBody this) [0x00000] in <f35d06abecb542c59abe564aada42d55>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::OnInventoryChanged>?-924452864 (RoR2.CharacterBody ) [0x00000] in <b637a1bd61454f7ab5711c37c3b218ee>:IL_0000
  at SillyGlasses.SillyGlassesPlugin.InvChangedHook (On.RoR2.CharacterBody+orig_OnInventoryChanged orig, RoR2.CharacterBody self) [0x00000] in <b1f8657d12fb448c9ef4adffea774a93>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::OnInventoryChanged>?1191347456 (RoR2.CharacterBody ) [0x00000] in <6d0e1c9ef8b14323b22ae4fe85493bdf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::OnInventoryChanged>?1645529088 (RoR2.CharacterBody ) [0x00000] in <b415c93b3cba44c1838e673a31180414>:IL_0000
  at TPDespair.ZetAspects.EffectHooks+<>c.<ApplyAspectBuffOnInventoryChangedHook>b__67_0 (On.RoR2.CharacterBody+orig_OnInventoryChanged orig, RoR2.CharacterBody self) [0x00000] in <4dadcd94303044279e739eddde02c081>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::OnInventoryChanged>?1052907904 (RoR2.CharacterBody ) [0x00000] in <9b5bc9a981d948b98c823e26928e2d80>:IL_0000
  at RoR2.Inventory.HandleInventoryChanged () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.Inventory.GiveItem (RoR2.ItemIndex itemIndex, System.Int32 count) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.Inventory.GiveItem (RoR2.ItemDef itemDef, System.Int32 count) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at TPDespair.ZetAspects.EffectHooks+<>c.<UpdateOnBuffLostHook>b__69_0 (On.RoR2.CharacterBody+orig_OnBuffFinalStackLost orig, RoR2.CharacterBody self, RoR2.BuffDef buffDef) [0x00000] in <4dadcd94303044279e739eddde02c081>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::OnBuffFinalStackLost>?1056918656 (RoR2.CharacterBody , RoR2.BuffDef ) [0x00000] in <3a9b836845dd4e6f845ba18cf6892a5a>:IL_0000
  at RoR2.CharacterBody.SetBuffCount (RoR2.BuffIndex buffType, System.Int32 newCount) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<RemoveBuff>?69243648._RoR2_CharacterBody::RemoveBuff (RoR2.CharacterBody this, RoR2.BuffIndex buffType) [0x00000] in <c2a05f6afefc4b9d8a4f8c06bb19180b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::RemoveBuff>?1506109440 (RoR2.CharacterBody , RoR2.BuffIndex ) [0x00000] in <00bd3db27e8c4d6196343ffe0ce8a458>:IL_0000
  at TPDespair.ZetAspects.EffectHooks.VoidBearFix_RemoveBuff (On.RoR2.CharacterBody+orig_RemoveBuff_BuffIndex orig, RoR2.CharacterBody self, RoR2.BuffIndex buffType) [0x00000] in <4dadcd94303044279e739eddde02c081>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::RemoveBuff>?1765697024 (RoR2.CharacterBody , RoR2.BuffIndex ) [0x00000] in <a95d742e1da9454aa3080782b9781f25>:IL_0000
  at DMD<UpdateBuffs>?69243648._RoR2_CharacterBody::UpdateBuffs (RoR2.CharacterBody this, System.Single deltaTime) [0x00000] in <7a3821e858c54758888b3b663d3bc096>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::UpdateBuffs>?-1311594496 (RoR2.CharacterBody , System.Single ) [0x00000] in <60758834c88748ea9844b544d73a0a74>:IL_0000
  at AncientScepter.HereticNevermore2.CharacterBody_UpdateBuffs (On.RoR2.CharacterBody+orig_UpdateBuffs orig, RoR2.CharacterBody self, System.Single deltaTime) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::UpdateBuffs>?1154037760 (RoR2.CharacterBody , System.Single ) [0x00000] in <f3a90d4f83034e03bc4ddf4ab6cb43bb>:IL_0000
  at RoR2.CharacterBody.FixedUpdate () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
[Warning:Tesla Trooper] unsetting. now 0 overrides
[Info   :Tesla Trooper]   at System.Environment.get_StackTrace () [0x00000] in <44afb4564e9347cf99a1865351ea8f4a>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_UnsetSkillOverride (On.RoR2.GenericSkill+orig_UnsetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::UnsetSkillOverride>?1933440384 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <77f5aacea1c54aabb8ea76b591710acb>:IL_0000
  at AncientScepter.AncientScepterItem+<>c__DisplayClass72_2.<HandleScepterSkill>g__UnsetOverrideLater|2 (RoR2.GenericSkill skill) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at RoR2.GenericSkill.SetSkillInternal (RoR2.Skills.SkillDef newSkillDef) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.GenericSkill.PickCurrentOverride () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<SetSkillOverride>?69243648._RoR2_GenericSkill::SetSkillOverride (RoR2.GenericSkill this, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <25459643799e4cd9a3ed7bdddb9af733>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.GenericSkill::SetSkillOverride>?1388957696 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <57f520f929fb47d380727fa1409d2fa0>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_SetSkillOverride (On.RoR2.GenericSkill+orig_SetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::SetSkillOverride>?127559168 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <67964cac4f384ff6b53bbe5ecbaee307>:IL_0000
  at AncientScepter.AncientScepterItem.HandleScepterSkill (RoR2.CharacterBody self, System.Boolean forceOff) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at AncientScepter.AncientScepterItem.On_CBInventoryChangedGlobal (RoR2.CharacterBody body) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<OnInventoryChanged>?69243648._RoR2_CharacterBody::OnInventoryChanged (RoR2.CharacterBody this) [0x00000] in <f35d06abecb542c59abe564aada42d55>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::OnInventoryChanged>?-924452864 (RoR2.CharacterBody ) [0x00000] in <b637a1bd61454f7ab5711c37c3b218ee>:IL_0000
  at SillyGlasses.SillyGlassesPlugin.InvChangedHook (On.RoR2.CharacterBody+orig_OnInventoryChanged orig, RoR2.CharacterBody self) [0x00000] in <b1f8657d12fb448c9ef4adffea774a93>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::OnInventoryChanged>?1191347456 (RoR2.CharacterBody ) [0x00000] in <6d0e1c9ef8b14323b22ae4fe85493bdf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::OnInventoryChanged>?1645529088 (RoR2.CharacterBody ) [0x00000] in <b415c93b3cba44c1838e673a31180414>:IL_0000
  at TPDespair.ZetAspects.EffectHooks+<>c.<ApplyAspectBuffOnInventoryChangedHook>b__67_0 (On.RoR2.CharacterBody+orig_OnInventoryChanged orig, RoR2.CharacterBody self) [0x00000] in <4dadcd94303044279e739eddde02c081>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::OnInventoryChanged>?1052907904 (RoR2.CharacterBody ) [0x00000] in <9b5bc9a981d948b98c823e26928e2d80>:IL_0000
  at RoR2.Inventory.HandleInventoryChanged () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.Inventory.GiveItem (RoR2.ItemIndex itemIndex, System.Int32 count) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.Inventory.GiveItem (RoR2.ItemDef itemDef, System.Int32 count) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at TPDespair.ZetAspects.EffectHooks+<>c.<UpdateOnBuffLostHook>b__69_0 (On.RoR2.CharacterBody+orig_OnBuffFinalStackLost orig, RoR2.CharacterBody self, RoR2.BuffDef buffDef) [0x00000] in <4dadcd94303044279e739eddde02c081>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::OnBuffFinalStackLost>?1056918656 (RoR2.CharacterBody , RoR2.BuffDef ) [0x00000] in <3a9b836845dd4e6f845ba18cf6892a5a>:IL_0000
  at RoR2.CharacterBody.SetBuffCount (RoR2.BuffIndex buffType, System.Int32 newCount) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<RemoveBuff>?69243648._RoR2_CharacterBody::RemoveBuff (RoR2.CharacterBody this, RoR2.BuffIndex buffType) [0x00000] in <c2a05f6afefc4b9d8a4f8c06bb19180b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::RemoveBuff>?1506109440 (RoR2.CharacterBody , RoR2.BuffIndex ) [0x00000] in <00bd3db27e8c4d6196343ffe0ce8a458>:IL_0000
  at TPDespair.ZetAspects.EffectHooks.VoidBearFix_RemoveBuff (On.RoR2.CharacterBody+orig_RemoveBuff_BuffIndex orig, RoR2.CharacterBody self, RoR2.BuffIndex buffType) [0x00000] in <4dadcd94303044279e739eddde02c081>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::RemoveBuff>?1765697024 (RoR2.CharacterBody , RoR2.BuffIndex ) [0x00000] in <a95d742e1da9454aa3080782b9781f25>:IL_0000
  at DMD<UpdateBuffs>?69243648._RoR2_CharacterBody::UpdateBuffs (RoR2.CharacterBody this, System.Single deltaTime) [0x00000] in <7a3821e858c54758888b3b663d3bc096>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::UpdateBuffs>?-1311594496 (RoR2.CharacterBody , System.Single ) [0x00000] in <60758834c88748ea9844b544d73a0a74>:IL_0000
  at AncientScepter.HereticNevermore2.CharacterBody_UpdateBuffs (On.RoR2.CharacterBody+orig_UpdateBuffs orig, RoR2.CharacterBody self, System.Single deltaTime) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::UpdateBuffs>?1154037760 (RoR2.CharacterBody , System.Single ) [0x00000] in <f3a90d4f83034e03bc4ddf4ab6cb43bb>:IL_0000
  at RoR2.CharacterBody.FixedUpdate () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
[Warning:Tesla Trooper] setting. now 0 overrides
[Info   :Tesla Trooper]   at System.Environment.get_StackTrace () [0x00000] in <44afb4564e9347cf99a1865351ea8f4a>:IL_0000
  at TeslaTrooperPlugin.GenericSkill_SetSkillOverride (On.RoR2.GenericSkill+orig_SetSkillOverride orig, RoR2.GenericSkill self, System.Object source, RoR2.Skills.SkillDef skillDef, RoR2.GenericSkill+SkillOverridePriority priority) [0x00000] in <ad44bb242ded4f05b2a554d945f5f311>:IL_0000
  at DMD<>?69243648.Hook<RoR2.GenericSkill::SetSkillOverride>?127559168 (RoR2.GenericSkill , System.Object , RoR2.Skills.SkillDef , RoR2.GenericSkill+SkillOverridePriority ) [0x00000] in <67964cac4f384ff6b53bbe5ecbaee307>:IL_0000
  at AncientScepter.AncientScepterItem.HandleScepterSkill (RoR2.CharacterBody self, System.Boolean forceOff) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at AncientScepter.AncientScepterItem.On_CBInventoryChangedGlobal (RoR2.CharacterBody body) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<OnInventoryChanged>?69243648._RoR2_CharacterBody::OnInventoryChanged (RoR2.CharacterBody this) [0x00000] in <f35d06abecb542c59abe564aada42d55>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::OnInventoryChanged>?-924452864 (RoR2.CharacterBody ) [0x00000] in <b637a1bd61454f7ab5711c37c3b218ee>:IL_0000
  at SillyGlasses.SillyGlassesPlugin.InvChangedHook (On.RoR2.CharacterBody+orig_OnInventoryChanged orig, RoR2.CharacterBody self) [0x00000] in <b1f8657d12fb448c9ef4adffea774a93>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::OnInventoryChanged>?1191347456 (RoR2.CharacterBody ) [0x00000] in <6d0e1c9ef8b14323b22ae4fe85493bdf>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::OnInventoryChanged>?1645529088 (RoR2.CharacterBody ) [0x00000] in <b415c93b3cba44c1838e673a31180414>:IL_0000
  at TPDespair.ZetAspects.EffectHooks+<>c.<ApplyAspectBuffOnInventoryChangedHook>b__67_0 (On.RoR2.CharacterBody+orig_OnInventoryChanged orig, RoR2.CharacterBody self) [0x00000] in <4dadcd94303044279e739eddde02c081>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::OnInventoryChanged>?1052907904 (RoR2.CharacterBody ) [0x00000] in <9b5bc9a981d948b98c823e26928e2d80>:IL_0000
  at RoR2.Inventory.HandleInventoryChanged () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.Inventory.GiveItem (RoR2.ItemIndex itemIndex, System.Int32 count) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at RoR2.Inventory.GiveItem (RoR2.ItemDef itemDef, System.Int32 count) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at TPDespair.ZetAspects.EffectHooks+<>c.<UpdateOnBuffLostHook>b__69_0 (On.RoR2.CharacterBody+orig_OnBuffFinalStackLost orig, RoR2.CharacterBody self, RoR2.BuffDef buffDef) [0x00000] in <4dadcd94303044279e739eddde02c081>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::OnBuffFinalStackLost>?1056918656 (RoR2.CharacterBody , RoR2.BuffDef ) [0x00000] in <3a9b836845dd4e6f845ba18cf6892a5a>:IL_0000
  at RoR2.CharacterBody.SetBuffCount (RoR2.BuffIndex buffType, System.Int32 newCount) [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
  at DMD<RemoveBuff>?69243648._RoR2_CharacterBody::RemoveBuff (RoR2.CharacterBody this, RoR2.BuffIndex buffType) [0x00000] in <c2a05f6afefc4b9d8a4f8c06bb19180b>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::RemoveBuff>?1506109440 (RoR2.CharacterBody , RoR2.BuffIndex ) [0x00000] in <00bd3db27e8c4d6196343ffe0ce8a458>:IL_0000
  at TPDespair.ZetAspects.EffectHooks.VoidBearFix_RemoveBuff (On.RoR2.CharacterBody+orig_RemoveBuff_BuffIndex orig, RoR2.CharacterBody self, RoR2.BuffIndex buffType) [0x00000] in <4dadcd94303044279e739eddde02c081>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::RemoveBuff>?1765697024 (RoR2.CharacterBody , RoR2.BuffIndex ) [0x00000] in <a95d742e1da9454aa3080782b9781f25>:IL_0000
  at DMD<UpdateBuffs>?69243648._RoR2_CharacterBody::UpdateBuffs (RoR2.CharacterBody this, System.Single deltaTime) [0x00000] in <7a3821e858c54758888b3b663d3bc096>:IL_0000
  at DMD<>?69243648.Trampoline<RoR2.CharacterBody::UpdateBuffs>?-1311594496 (RoR2.CharacterBody , System.Single ) [0x00000] in <60758834c88748ea9844b544d73a0a74>:IL_0000
  at AncientScepter.HereticNevermore2.CharacterBody_UpdateBuffs (On.RoR2.CharacterBody+orig_UpdateBuffs orig, RoR2.CharacterBody self, System.Single deltaTime) [0x00000] in <71677d642263459ca5d3a59e7aff844b>:IL_0000
  at DMD<>?69243648.Hook<RoR2.CharacterBody::UpdateBuffs>?1154037760 (RoR2.CharacterBody , System.Single ) [0x00000] in <f3a90d4f83034e03bc4ddf4ab6cb43bb>:IL_0000
  at RoR2.CharacterBody.FixedUpdate () [0x00000] in <1d532be543be416b9db3594e4b62447d>:IL_0000
*/