using BepInEx.Configuration;
using RoR2;
using System;
using HenryMod.Modules.Characters;
using UnityEngine;

namespace HenryMod.Modules.Survivors {

    internal abstract class SurvivorBase : CharacterBase
    {
        internal abstract float sortPosition { get;}

        internal abstract UnlockableDef characterUnlockableDef { get; }

        internal abstract ConfigEntry<bool> characterEnabledConfig { get; }

        internal virtual GameObject displayPrefab { get; set; }

        internal override void InitializeCharacter() {

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

        internal virtual void InitializeUnlockables()
        {
        }
    }
}
