using RA2Mod.Survivors.Tesla.Orbs;
using RoR2;
using RoR2.Orbs;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace RA2Mod.Minions.TeslaTower.States
{
    public class TowerZapMulti : TowerZap
    {

        public static int extraZaps = 3;

        List<HealthComponent> bouncedObjectsList = new List<HealthComponent>();

        protected override void fireOrb()
        {

            for (int i = 0; i < extraZaps; i++)
            {

                PseudoLightningOrb newOrb = GetNewOrb();
                newOrb.target = lightningOrb.PickNextTarget(transform.position);
                if (lightningOrb.target == null)
                    return;

                for (int j = 0; j < 3; j++)
                {

                    OrbManager.instance.AddOrb(newOrb);
                }
            }
        }

        private PseudoLightningOrb GetNewOrb()
        {

            return new PseudoLightningOrb
            {
                origin = FindModelChild("Orb").position,
                damageValue = DamageCoefficient * damageStat,
                isCrit = crit,
                //bouncesRemaining = 1,
                //damageCoefficientPerBounce = BounceDamageMultplier,
                //damageType = DamageType.SlowOnHit,
                teamIndex = teamComponent.teamIndex,
                attacker = gameObject,
                inflictor = gameObject,
                procCoefficient = 1f,
                bouncedObjects = bouncedObjectsList,
                moddedLightningType = GetOrbType,
                damageColorIndex = DamageColorIndex.Default,
                range = TowerIdleSearch.SearchRange,
                canBounceOnSameTarget = true,
                speed = -1
            };
        }
    }

}
