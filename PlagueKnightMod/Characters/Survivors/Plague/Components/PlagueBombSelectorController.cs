using RoR2;
using UnityEngine;
using PlagueMod.Survivors.Plague.SkillDefs;
using PlagueMod.Modules;
using System;

namespace PlagueMod.Survivors.Plague.Components
{
    public class PlagueBombSelectorController : MonoBehaviour
    {
        public GenericSkill casingGenericSkill;
        public GenericSkill powderGenericSkill;

        public PlagueBombCasingSkillDef casingSkillDef => (PlagueBombCasingSkillDef)casingGenericSkill.skillDef;
        public PlagueBombPowderSkillDef powderSkillDef => (PlagueBombPowderSkillDef)powderGenericSkill.skillDef;

        private GameObject casingProjectile; //later lol
        private Action<GameObject, Type, Material> projectileSpawnAction;

        private int tempCasingIndex;
        private int tempPowderIndex;

        void Start()
        {
            casingGenericSkill = GetComponent<SkillLocator>().FindSkill("casing");
            powderGenericSkill = GetComponent<SkillLocator>().FindSkill("powder");
        }

        public GameObject GetSelectedProjectile()
        {
            return casingSkillDef.projectilePrefab;
        }

        public void ChangeSelectedCasing()
        {
            tempPowderIndex++;
            if (tempPowderIndex > 1) tempPowderIndex = 0;

            casingGenericSkill.SetSkillFromFamily(tempPowderIndex);

        }

        public void ChangeSelectedPowder()
        {
            tempCasingIndex++;
            if (tempCasingIndex > 1) tempCasingIndex = 0;

            powderGenericSkill.SetSkillFromFamily(tempCasingIndex);
        }
    }
}