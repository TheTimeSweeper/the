using RoR2;
using UnityEngine;
using PlagueMod.Survivors.Plague.SkillDefs;
using PlagueMod.Modules;
using System;

namespace PlagueMod.Survivors.Plague.Components
{
    public class PlagueBombSelectorController : MonoBehaviour
    {
        [SerializeField]
        public GenericSkillHolder casingGenericSkills;

        [SerializeField]
        public GenericSkillHolder powderGenericSkills;

        [HideInInspector]
        public GenericSkill casingLoadoutSkill;
        [HideInInspector]
        public GenericSkill powderLoadoutSkill;

        [Obsolete("were goin hard now")]
        public PlagueBombCasingSkillDef casingSkillDef => (PlagueBombCasingSkillDef)casingLoadoutSkill.skillDef;
        [Obsolete("were goin hard now")]
        public PlagueBombPowderSkillDef powderSkillDef => (PlagueBombPowderSkillDef)powderLoadoutSkill.skillDef;

        private GameObject cachedProjectile; //later lol
        private Action<GameObject, Type, Material> projectileSpawnAction;

        private int tempCasingIndex;
        private int tempPowderIndex;
        private SkillLocator skillLocator;

        void Start()
        {
            skillLocator = GetComponent<SkillLocator>();

            casingLoadoutSkill = skillLocator.FindSkill("casing");
            powderLoadoutSkill = skillLocator.FindSkill("powder");

            casingGenericSkills.Init(casingLoadoutSkill.skillFamily);
            powderGenericSkills.Init(powderLoadoutSkill.skillFamily);
        }

        public GameObject GetSelectedProjectile()
        {
            return casingSkillDef.projectilePrefab;
        }

        public void SetCasingSkillDef(GenericSkill targetSkill)
        {
            casingLoadoutSkill.SetBaseSkill(targetSkill.baseSkill);
        }

        //fuck what do we do for lunarprimaryreplacement
        public void SetPrimaryGenericSkill(GenericSkill targetSkill)
        {
            skillLocator.primary = targetSkill;
        }

        public void ChangeSelectedCasing()
        {
            tempPowderIndex++;
            if (tempPowderIndex > 1) tempPowderIndex = 0;

            casingLoadoutSkill.SetSkillFromFamily(tempPowderIndex);
            
        }

        public void ChangeSelectedPowder()
        {
            tempCasingIndex++;
            if (tempCasingIndex > 1) tempCasingIndex = 0;
            
            powderLoadoutSkill.SetSkillFromFamily(tempCasingIndex);
        }
    }
}