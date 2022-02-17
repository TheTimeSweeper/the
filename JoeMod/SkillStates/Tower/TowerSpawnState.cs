using EntityStates;

namespace JoeMod.ModdedEntityStates.TeslaTrooper.Tower
{
    public class TowerSpawnState : GenericCharacterSpawnState
    {
        public static float TowerSpawnDuration = 1.5f;

        public override void OnEnter()
        {
            duration = TowerSpawnDuration / attackSpeedStat;
            spawnSoundString = "RA2_bPlace";
            base.OnEnter();
        }
    }

}
