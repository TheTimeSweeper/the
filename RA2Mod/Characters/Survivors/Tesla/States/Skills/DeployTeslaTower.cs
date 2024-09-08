using EntityStates;
using RA2Mod.General;
using RA2Mod.Minions.TeslaTower;
using RA2Mod.Survivors.Tesla;
using RoR2;
using UnityEngine;

namespace RA2Mod.Survivors.Tesla.States
{
    public class DeployTeslaTower : BaseSkillState
    {
        protected struct TotallyOriginalPlacementInfo
        {
            public bool ok;
            public Vector3 position;
            public Quaternion rotation;
        }

        public GameObject blueprintPrefab = TeslaAssets.TeslaCoilBlueprint;
        public GameObject teslacoilPrefab = TeslaAssets.TeslaCoil;

        protected TotallyOriginalPlacementInfo currentPlacementInfo;
        private float _minTowerHeight = 3;

        private float entryCountdown = 0;//0.1f;
        private float exitCountdown = 0;//0.25f;
        private bool exitPending = false;

        private const float _deployMaxUp = 2f;
        private const float _deployMaxDown = 7f;
        private const float _baseDeployForwardDistance = 6f;

        private bool ConstructionComplete;

        private BlueprintController blueprints;
        private GameObject coilMasterPrefab = TeslaTowerNotSurvivor.masterPrefab;

        public override void OnEnter()
        {
            base.OnEnter();
            if (isAuthority)
            {
                currentPlacementInfo = GetPlacementInfo();
                blueprints = Object.Instantiate(blueprintPrefab, currentPlacementInfo.position, currentPlacementInfo.rotation).GetComponent<BlueprintController>();
            }
;
            entryCountdown = 0.1f;
            exitCountdown = 0.25f;
            exitPending = false;

            PlayCrossfade("Gesture, Override", "Placing", 0.1f);
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

            if (isAuthority)
            {
                entryCountdown -= Time.deltaTime;

                if (exitPending)
                {
                    exitCountdown -= Time.deltaTime;
                    if (exitCountdown <= 0f)
                    {
                        outer.SetNextStateToMain();
                        return;
                    }
                }
                else if (inputBank && entryCountdown <= 0f)
                {
                    if (currentPlacementInfo.ok && (inputBank.skill1.down || inputBank.skill4.justPressed || GeneralCompat.IsLocalVRPlayer(characterBody) && inputBank.skill4.justReleased))
                    {
                        HandleConstructCoil();
                    }
                    if (inputBank.skill2.justPressed || inputBank.sprint.justPressed)
                    {
                        DestroyBlueprints();
                        exitPending = true;
                    }
                }
            }
        }

        protected virtual void HandleConstructCoil()
        {
            if (characterBody)
            {

                constructCoil(currentPlacementInfo);

                PlayCrossfade("Gesture, Override", "DoPlace", 0.1f);

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
            DestroyBlueprints();
            exitPending = true;
        }

        protected virtual void constructCoil(TotallyOriginalPlacementInfo placementInfo)
        {
            characterBody.SendConstructTurret(characterBody,
                                                   placementInfo.position,
                                                   placementInfo.rotation,
                                                   MasterCatalog.FindMasterIndex(coilMasterPrefab));
        }

        public override void OnExit()
        {
            base.OnExit();

            DestroyBlueprints();

            if (!ConstructionComplete)
            {
                PlayCrossfade("Gesture, Override", "CancelPlace", 0.1f);
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

        protected TotallyOriginalPlacementInfo GetPlacementInfo()
        {
            RaycastHit raycastHit;
            Ray aimRay = this.GetAimRay(false);

            //quick direct raycast to check if we closer ground
            float deployForwardDistance;
            if (Physics.Raycast(aimRay, out raycastHit, 10, LayerIndex.world.mask))
            {
                Vector3 diff = raycastHit.point - aimRay.origin;
                diff.y = 0;
                deployForwardDistance = Mathf.Min(diff.magnitude, _baseDeployForwardDistance);
            }
            else
            {
                deployForwardDistance = _baseDeployForwardDistance;
            }

            //get aim ray directly forward
            Vector3 aimDirection = aimRay.direction;
            aimDirection.y = 0f;
            aimDirection.Normalize();
            aimRay.direction = aimDirection;

            //init placement info with our direction
            TotallyOriginalPlacementInfo placementInfo = default;
            placementInfo.ok = false;
            placementInfo.rotation = Util.QuaternionSafeLookRotation(-aimDirection);

            //prepare the point we want to check
            Ray deployRay = new Ray(aimRay.GetPoint(deployForwardDistance) + Vector3.up * _deployMaxUp, Vector3.down);
            float deployLookDown = _deployMaxUp + _deployMaxDown;
            float deployHeightDelta = deployLookDown;

            //check, at a set distance in front, a certain range up and down
            if (Physics.SphereCast(deployRay, 0.5f, out raycastHit, deployLookDown, LayerIndex.world.mask) && raycastHit.normal.y > 0.5f)
            {
                deployHeightDelta = raycastHit.distance;
                placementInfo.ok = true;
            }

            //get our result
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