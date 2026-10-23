using EntityStates.Gup;
using GorpMod.Enemies.Gorp.Content;

namespace GorpMod.Characters.Enemies.Gorp.States
{
    public class GorpSplitDeath : BaseSplitDeath
    {
        public override void OnEnter()
        {
            characterSpawnCard = CharacterAssets.GipSpawnCard;
            spawnCount = CharacterConfig.DeathGips;
            deathDelay = 0.1f;
            moneyMultiplier = 0;
            base.OnEnter();
        }
    }
}
