using BepInEx.Configuration;
using RoR2;
using System;
using Modules.Characters;
using UnityEngine;

namespace Modules.Survivors {

    public abstract class SurvivorBase : CharacterBase
    {
        public abstract float sortPosition { get;}

        public abstract string survivorTokenPrefix { get; }

        public abstract UnlockableDef characterUnlockableDef { get; }

        public virtual ConfigEntry<bool> characterEnabledConfig { get; }

        public virtual GameObject displayPrefab { get; set; }

        public override void InitializeCharacter() {

            if (characterEnabledConfig != null && !characterEnabledConfig.Value)
                return;

            InitializeUnlockables();

            base.InitializeCharacter();

            InitializeSurvivor();
        }

        protected virtual void InitializeSurvivor() {
            displayPrefab = Modules.Prefabs.CreateDisplayPrefab(bodyName + "Display", bodyPrefab, bodyInfo);
            RegisterNewSurvivor(bodyPrefab, displayPrefab, Color.grey, survivorTokenPrefix, characterUnlockableDef, sortPosition);
        }

        public virtual void InitializeUnlockables()
        {
        }


        public static void RegisterNewSurvivor(GameObject bodyPrefab, GameObject displayPrefab, Color charColor, string tokenPrefix) { RegisterNewSurvivor(bodyPrefab, displayPrefab, charColor, tokenPrefix, null, 100f); }
        public static void RegisterNewSurvivor(GameObject bodyPrefab, GameObject displayPrefab, Color charColor, string tokenPrefix, float sortPosition) { RegisterNewSurvivor(bodyPrefab, displayPrefab, charColor, tokenPrefix, null, sortPosition); }
        public static void RegisterNewSurvivor(GameObject bodyPrefab, GameObject displayPrefab, Color charColor, string tokenPrefix, UnlockableDef unlockableDef) { RegisterNewSurvivor(bodyPrefab, displayPrefab, charColor, tokenPrefix, unlockableDef, 100f); }
        public static void RegisterNewSurvivor(GameObject bodyPrefab, GameObject displayPrefab, Color charColor, string tokenPrefix, UnlockableDef unlockableDef, float sortPosition) {
            SurvivorDef survivorDef = ScriptableObject.CreateInstance<SurvivorDef>();
            survivorDef.bodyPrefab = bodyPrefab;
            survivorDef.displayPrefab = displayPrefab;
            survivorDef.primaryColor = charColor;

            survivorDef.displayNameToken = tokenPrefix + "NAME";
            survivorDef.descriptionToken = tokenPrefix + "DESCRIPTION";
            survivorDef.outroFlavorToken = tokenPrefix + "OUTRO_FLAVOR";
            survivorDef.mainEndingEscapeFailureFlavorToken = tokenPrefix + "OUTRO_FAILURE";

            survivorDef.desiredSortPosition = sortPosition;
            survivorDef.unlockableDef = unlockableDef;

            Modules.Content.AddSurvivorDef(survivorDef);
        }
    }
}
