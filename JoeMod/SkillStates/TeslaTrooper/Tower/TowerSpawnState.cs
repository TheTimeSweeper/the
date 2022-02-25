using EntityStates;
using RoR2;

namespace JoeMod.ModdedEntityStates.TeslaTrooper.Tower
{
    public class TowerSpawnState : BaseState
    {
        public static float TowerSpawnDuration = 1.5f;
        private float duration;

        public override void OnEnter()
        {
            duration = TowerSpawnDuration / attackSpeedStat;

            Util.PlaySound("Play_building_bplace", base.gameObject);
            base.PlayAnimation("Body", "ConstructionComplete", "construct.playbackRate", this.duration);
            base.OnEnter();

            TeslaCoilControllerController controller = characterBody.masterObject.GetComponent<MinionOwnership>()?.ownerMaster.GetBodyObject()?.GetComponent<TeslaCoilControllerController>();

            if (controller) {
                controller.addTower(gameObject);

                characterBody.masterObject.GetComponent<Deployable>()?.onUndeploy.AddListener(() => {
                    controller.removeTower(gameObject);
                });
            }

        }

        public override void FixedUpdate() {
            base.FixedUpdate();
            if (base.fixedAge >= this.duration && base.isAuthority) {
                this.outer.SetNextState(new TowerIdleSearch {
                    justSpawned = true
                });
            }
        }
    }

}
