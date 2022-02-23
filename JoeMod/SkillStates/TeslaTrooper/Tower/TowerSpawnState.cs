using EntityStates;
using RoR2;

namespace JoeMod.ModdedEntityStates.TeslaTrooper.Tower
{
    public class TowerSpawnState : GenericCharacterSpawnState
    {
        public static float TowerSpawnDuration = 1.5f;

        public override void OnEnter()
        {
            duration = TowerSpawnDuration / attackSpeedStat;
            spawnSoundString = "Play_building_bplace";
            base.OnEnter();

            TeslaCoilControllerController controller = GetComponent<MinionOwnership>()?.ownerMaster.GetBodyObject()?.GetComponent<TeslaCoilControllerController>();
            if (controller) {
                controller.addCoil(gameObject);

                GetComponent<Deployable>()?.onUndeploy.AddListener(() => {
                    controller.removeCoil(gameObject);
                });
            }

        }
    }

}
