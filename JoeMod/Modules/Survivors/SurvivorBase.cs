using BepInEx.Configuration;
using RoR2;
using System;
using HenryMod.Modules.Characters;
using UnityEngine;

namespace HenryMod.Modules.Survivors {

    public abstract class SurvivorBase : CharacterBase
    {
        public abstract float sortPosition { get;}

        public abstract UnlockableDef characterUnlockableDef { get; }

        public abstract ConfigEntry<bool> characterEnabledConfig { get; }

        public virtual GameObject displayPrefab { get; set; }

        public override void InitializeCharacter() {

            if (characterEnabledConfig != null && !characterEnabledConfig.Value)
                return;

            base.InitializeCharacter();

            InitializeSurvivor();
            InitializeUnlockables();
        }

        protected virtual void InitializeSurvivor() {
            displayPrefab = Modules.Prefabs.CreateDisplayPrefab(bodyName + "Display", bodyPrefab, bodyInfo);
            Modules.Prefabs.RegisterNewSurvivor(bodyPrefab, displayPrefab, Color.grey, bodyName.ToUpper(), characterUnlockableDef, sortPosition);
        }

        public virtual void InitializeUnlockables()
        {
        }
    }
}
