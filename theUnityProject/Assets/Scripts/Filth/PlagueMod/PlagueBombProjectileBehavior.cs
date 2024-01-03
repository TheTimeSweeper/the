using EntityStates;
using RoR2;
using RoR2.Projectile;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlagueMod.Survivors.Plague.Components {
    public class PlagueBombProjectileBehavior : MonoBehaviour {
        [SerializeField]
        private ProjectileImpactExplosion impactExplosion;
        [SerializeField]
        private EntityStateMachine entityStateMachine;
        [SerializeField]
        private PlagueBombColorizer colorizer;
    }
}