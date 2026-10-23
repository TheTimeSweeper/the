using GorpMod.Modules;
using RoR2;
using RoR2BepInExPack.GameAssetPathsBetter;
using UnityEngine;

namespace GorpMod.Enemies.Gorp.Content
{
    public static class CharacterAssets
    {
        public static GameObject GipMaster;
        public static CharacterSpawnCard GipSpawnCard;

        public static void Init()
        {
            Asset.LoadAssetAsync<GameObject>(RoR2_DLC1_Gup.GipMaster_prefab, (gipMaster) => GipMaster = gipMaster);
            Asset.LoadAssetAsync<CharacterSpawnCard>(RoR2_DLC1_Gup.cscGipBody_asset, (gipSpawnCard) => GipSpawnCard = gipSpawnCard);
        }
    }
}
