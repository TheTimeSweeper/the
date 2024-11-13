using RoR2;

namespace Matchmaker.MatchGrid
{
    [System.Serializable]
    public class SpecialTileInfo
    {
        public MatchTile tilePrefab;
        public ItemDef itemDef;
        public float spawnChance;
        public float SpawnChance => spawnChance / 100f;

        public SpecialTileInfo(MatchTile tilePrefab, ItemDef itemDef, float spawnChance)
        {
            this.tilePrefab = tilePrefab;
            this.itemDef = itemDef;
            this.spawnChance = spawnChance;
        }
    }
}