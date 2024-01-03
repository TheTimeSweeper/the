using EntityStates;
using PlagueMod.Survivors.Plague.SkillDefs;
using RoR2;
using RoR2.Projectile;
using System;
using UnityEngine;

namespace PlagueMod.Survivors.Plague.Components
{
    public class PlagueBombProjectileBehavior : MonoBehaviour
    {

        [SerializeField]
        private ProjectileController controller;
        [SerializeField]
        private ProjectileImpactExplosion impactExplosion;
        [SerializeField]
        private EntityStateMachine entityStateMachine;
        [SerializeField]
        private PlagueBombColorizer colorizer;

        private PlagueBombPowderSkillDef powderSkillDef;

        void Awake()
        {
            powderSkillDef = controller.owner?.GetComponent<PlagueBombSelectorController>()?.powderSkillDef;

            if (powderSkillDef)
            {
                SetPowder(powderSkillDef);
            }
        }

        public void SetPowder(PlagueBombPowderSkillDef skillDef)
        {
            impactExplosion.childrenProjectilePrefab = skillDef.impactProjectilePrefab;
            impactExplosion.childrenCount = 1;

            entityStateMachine.initialStateType = skillDef.flyingState;

            colorizer.Colorize(skillDef.projectileMaterial);
        }
    }
}