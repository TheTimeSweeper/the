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

        public PlagueBombCasingSkillDef casingSkillDef => (PlagueBombCasingSkillDef)casingLoadoutSkill.skillDef;
        public PlagueBombPowderSkillDef powderSkillDef => (PlagueBombPowderSkillDef)powderLoadoutSkill.skillDef;

        private GameObject cachedProjectile; //later lol
        private Action<GameObject, Type, Material> projectileSpawnAction;

        public bool initialized { get; private set; }

        public PlagueBombSelectUI bombSelectUI { private get; set; }

        private int tempCasingIndex;
        private int tempPowderIndex;
        private SkillLocator skillLocator;

        void Start()
        {
            skillLocator = GetComponent<SkillLocator>();

            casingLoadoutSkill = skillLocator.FindSkill("LOADOUT_CASING");
            powderLoadoutSkill = skillLocator.FindSkill("LOADOUT_POWDER");
            casingLoadoutSkill.SetSkillFromFamily(0);
            powderLoadoutSkill.SetSkillFromFamily(0);
            
            casingGenericSkills.Init(casingLoadoutSkill.skillFamily);
            powderGenericSkills.Init(powderLoadoutSkill.skillFamily);
            initialized = true;
        }

        public void ShowUI(bool shouldShow = true)
        {
            bombSelectUI.Show(shouldShow);
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
            powderLoadoutSkill.SetBaseSkill(targetSkill.baseSkill);
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