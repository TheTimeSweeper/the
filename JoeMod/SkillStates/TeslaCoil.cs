using EntityStates;
using RoR2;
using UnityEngine;

namespace HenryMod.ModdedEntityStates.Joe
{
    public class TeslaCoil : BaseSkillState
    {
        private struct TotallyOriginalPlacementInfo
        {
            public bool ok;
            public Vector3 position;
            public Quaternion rotation;
        }

        public GameObject blueprintPrefab = Modules.Assets.TestlaCoilBlueprint;
        public GameObject teslacoilPrefab = Modules.Assets.TestlaCoil;

        private float _minTowerHeight = 3;
        private TeslaCoil.TotallyOriginalPlacementInfo currentPlacementInfo;

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
            if (base.isAuthority)
            {
                currentPlacementInfo = this.GetPlacementInfo();
                this.blueprints = UnityEngine.Object.Instantiate(this.blueprintPrefab, this.currentPlacementInfo.position, this.currentPlacementInfo.rotation).GetComponent<BlueprintController>();
            }

            base.PlayAnimation("Arms, Override", "Chrag1");
            this.entryCountdown = 0.1f;
            this.exitCountdown = 0.25f;
            this.exitPending = false;
        }

        public override void Update()
        {
            base.Update();
            this.currentPlacementInfo = this.GetPlacementInfo();
            if (this.blueprints)
            {
                this.blueprints.PushState(this.currentPlacementInfo.position, this.currentPlacementInfo.rotation, this.currentPlacementInfo.ok);
            }
        }
        
        public override void FixedUpdate()
        {
            base.FixedUpdate();

            StartAimMode();
            
            if(base.isAuthority)
            {
                this.entryCountdown -= Time.fixedDeltaTime;

                if (this.exitPending)
                {
                    this.exitCountdown -= Time.fixedDeltaTime;
                    if (this.exitCountdown <= 0f)
                    {
                        this.outer.SetNextStateToMain();
                        return;
                    }
                }
                else if (base.inputBank && this.entryCountdown <= 0f)
                {
                    if ((base.inputBank.skill1.down || base.inputBank.skill4.justPressed) && this.currentPlacementInfo.ok)
                    {
                        if (base.characterBody)
                        {
                            //base.characterBody.SendConstructTurret(base.characterBody, this.currentPlacementInfo.position, this.currentPlacementInfo.rotation, MasterCatalog.FindMasterIndex(this.turretMasterPrefab));
                            //GameObject coil = R2API.PrefabAPI.InstantiateClone(teslacoilPrefab, "TestlaCoil", true);
                            //UnityEngine.Networking.NetworkServer.Spawn(teslacoilPrefab);
                            GameObject coil = UnityEngine.Object.Instantiate(teslacoilPrefab);
                            coil.transform.position = currentPlacementInfo.position;
                            coil.transform.rotation = currentPlacementInfo.rotation;

                            base.PlayAnimation("Arms, Override", "swing1 v2");
                            ConstructionComplete = true;

                            if (base.skillLocator)
                            {
                                GenericSkill skill = base.skillLocator.GetSkill(SkillSlot.Special);
                                if (skill)
                                {
                                    skill.DeductStock(1);
                                }
                            }
                        }
                        //Util.PlaySound(this.placeSoundString, base.gameObject);
                        this.DestroyBlueprints();
                        this.exitPending = true;
                    }
                    if (base.inputBank.skill2.justPressed)
                    {
                        this.DestroyBlueprints();
                        this.exitPending = true;
                    }
                }
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            this.DestroyBlueprints();

            if(!ConstructionComplete)
            {
                PlayCrossfade("Arms, Override", "BufferEmpty", 0.5f);
            }
        }

        private void DestroyBlueprints()
        {
            if (this.blueprints)
            {
                EntityState.Destroy(this.blueprints.gameObject);
                this.blueprints = null;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }

        private TeslaCoil.TotallyOriginalPlacementInfo GetPlacementInfo()
        {
            Ray aimRay = base.GetAimRay();
            Vector3 aimDirection = aimRay.direction;
            aimDirection.y = 0f;
            aimDirection.Normalize();
            aimRay.direction = aimDirection;

            TeslaCoil.TotallyOriginalPlacementInfo placementInfo = default(TeslaCoil.TotallyOriginalPlacementInfo);
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