using AliemMod.Modules;
using EntityStates.GlobalSkills.LunarNeedle;
using RoR2;
using UnityEngine;

namespace ModdedEntityStates.Aliem
{
    public class ChargedLunarNeedleFire : RayGunChargedFire
    {
        public override GameObject projectile => Assets.LunarChargedProjectile;

        public override void OnEnter()
        {
            base.OnEnter();

            EffectManager.SimpleMuzzleFlash(FireLunarNeedle.muzzleFlashEffectPrefab, base.gameObject, "Head", false);
            Util.PlaySound("Play_item_lunar_use_utilityReplacement_end", gameObject);
            //Util.PlaySound("Play_item_lunar_secondaryReplace_explode", gameObject);
        }

        protected override void ModifyState()
        {
            base.ModifyState();

            force = 0;
            damageCoefficient = 0.05f;
            attackSoundString = FireLunarNeedle.fireSound;
        }
    }
}
