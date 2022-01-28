using EntityStates;
using RoR2;
using UnityEngine;

namespace JoeMod.ModdedEntityStates.TeslaTrooper
{
    public class TeslaCoil : BaseSkillState
    {
        private struct TotallyOriginalPlacementInfo
        {
            public bool ok;
            public Vector3 position;
            public Quaternion rotation;
        }

        public GameObject blueprintPrefab = HenryMod.Modules.Assets.TestlaCoilBlueprint;
        public GameObject teslacoilPrefab = HenryMod.Modules.Assets.TestlaCoil;

        private float _minTowerHeight = 3;
        private TotallyOriginalPlacementInfo currentPlacementInfo;

        private float entryCountdown = 0;//0.1f;
        private float exitCountdown = 0;//0.25f;
        private bool exitPending = false;

        private const float _deployMaxUp = 1f;
        private const float _deployMaxDown = 4f;
        private const float deployForwardDistance = 6f;

        private bool ConstructionComplete;

        private BlueprintController blueprints;
        

        public override void OnEnter()
        {
            base.OnEnter();
            if (isAuthority)
            {
                currentPlacementInfo = GetPlacementInfo();
                blueprints = Object.Instantiate(blueprintPrefab, currentPlacementInfo.position, currentPlacementInfo.rotation).GetComponent<BlueprintController>();
            }

            //todo:anim
            //base.PlayAnimation("Gesture, Override", "HandOut");
            entryCountdown = 0.1f;
            exitCountdown = 0.25f;
            exitPending = false;
        }

        public override void Update()
        {
            base.Update();
            currentPlacementInfo = GetPlacementInfo();
            if (blueprints)
            {
                blueprints.PushState(currentPlacementInfo.position, currentPlacementInfo.rotation, currentPlacementInfo.ok);
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            StartAimMode();

            PlayAnimation("Gesture, Override", "HandOut");

            if (isAuthority)
            {
                entryCountdown -= Time.fixedDeltaTime;

                if (exitPending)
                {
                    exitCountdown -= Time.fixedDeltaTime;
                    if (exitCountdown <= 0f)
                    {
                        outer.SetNextStateToMain();
                        return;
                    }
                }
                else if (inputBank && entryCountdown <= 0f)
                {
                    if ((inputBank.skill1.down || inputBank.skill4.justPressed) && currentPlacementInfo.ok)
                    {
                        if (characterBody)
                        {
                            //base.characterBody.SendConstructTurret(base.characterBody, this.currentPlacementInfo.position, this.currentPlacementInfo.rotation, MasterCatalog.FindMasterIndex(this.turretMasterPrefab));
                            //GameObject coil = R2API.PrefabAPI.InstantiateClone(teslacoilPrefab, "TestlaCoil", true);
                            //UnityEngine.Networking.NetworkServer.Spawn(teslacoilPrefab);
                            GameObject coil = Object.Instantiate(teslacoilPrefab);
                            coil.transform.position = currentPlacementInfo.position;
                            coil.transform.rotation = currentPlacementInfo.rotation;

                            //base.PlayAnimation("Gesture, Override", "HandOut");
                            ConstructionComplete = true;

                            if (skillLocator)
                            {
                                GenericSkill skill = skillLocator.GetSkill(SkillSlot.Special);
                                if (skill)
                                {
                                    skill.DeductStock(1);
                                }
                            }
                        }
                        //Util.PlaySound(this.placeSoundString, base.gameObject);
                        DestroyBlueprints();
                        exitPending = true;
                    }
                    if (inputBank.skill2.justPressed)
                    {
                        DestroyBlueprints();
                        exitPending = true;
                    }
                }
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            DestroyBlueprints();

            if (!ConstructionComplete)
            {
                PlayCrossfade("Gesture, Override", "BufferEmpty", 0.5f);
            }
        }

        private void DestroyBlueprints()
        {
            if (blueprints)
            {
                Destroy(blueprints.gameObject);
                blueprints = null;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }

        private TotallyOriginalPlacementInfo GetPlacementInfo()
        {
            Ray aimRay = GetAimRay();
            Vector3 aimDirection = aimRay.direction;
            aimDirection.y = 0f;
            aimDirection.Normalize();
            aimRay.direction = aimDirection;

            TotallyOriginalPlacementInfo placementInfo = default;
            placementInfo.ok = false;
            placementInfo.rotation = Util.QuaternionSafeLookRotation(-aimDirection);

            Ray deployRay = new Ray(aimRay.GetPoint(deployForwardDistance) + Vector3.up * _deployMaxUp, Vector3.down);
            float deployLookDown = _deployMaxUp + _deployMaxDown;
            float deployHeightDelta = deployLookDown;
            RaycastHit raycastHit;
            if (Physics.SphereCast(deployRay, 0.5f, out raycastHit, deployLookDown, LayerIndex.world.mask) && raycastHit.normal.y > 0.5f)
            {
                deployHeightDelta = raycastHit.distance;
                placementInfo.ok = true;
            }

            Vector3 deployPoint = deployRay.GetPoint(deployHeightDelta + 0.5f);
            placementInfo.position = deployPoint;
            if (placementInfo.ok)
            {
                float minHeight = Mathf.Max(_minTowerHeight, 0f);
                if (Physics.CheckCapsule(placementInfo.position + Vector3.up * (minHeight - 0.5f), placementInfo.position + Vector3.up * 0.5f, 0.45f, LayerIndex.world.mask | LayerIndex.defaultLayer.mask))
                {
                    placementInfo.ok = false;
                }
            }
            return placementInfo;
        }


    }
}