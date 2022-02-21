using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace HenryMod.Modules.Characters {
    internal abstract class CharacterBase {

        internal static CharacterBase instance;

        internal abstract string bodyName { get; }

        internal abstract BodyInfo bodyInfo { get; set; }

        internal abstract int mainRendererIndex { get; }
        internal abstract CustomRendererInfo[] customRendererInfos { get; set; }

        internal abstract Type characterMainState { get; } 

        internal abstract ItemDisplaysBase itemDisplays { get; }

        internal virtual GameObject bodyPrefab { get; set; }
        internal virtual CharacterModel characterModel { get; set; }
        internal string fullBodyName => bodyName + "Body";

        internal virtual void Initialize() {
            instance = this;
            InitializeCharacter();
        }

        internal virtual void InitializeCharacter() {

            InitializeCharacterBody();
            InitializeCharacterMaster();
            InitializeCharacterModel();

            InitializeEntityStateMachine();
            InitializeSkills();

            InitializeHitboxes();
            InitializeHurtboxes();

            InitializeSkins();
            InitializeItemDisplays();

            //survivor?
            InitializeDoppelganger();
        }

        protected virtual void InitializeCharacterBody() {
            bodyPrefab = Modules.Prefabs.CreateBodyPrefab(bodyName + "Body", "mdl" + bodyName, bodyInfo);
        }
        protected virtual void InitializeCharacterMaster() {
        }
        protected virtual void InitializeCharacterModel() {
            characterModel = Modules.Prefabs.SetupCharacterModel(bodyPrefab, customRendererInfos, mainRendererIndex);
        }

        protected virtual void InitializeEntityStateMachine() {
            bodyPrefab.GetComponent<EntityStateMachine>().mainStateType = new EntityStates.SerializableEntityStateType(characterMainState);
        }

        internal abstract void InitializeSkills();

        internal virtual void InitializeHitboxes() {
        }

        internal virtual void InitializeHurtboxes() {
        }

        internal virtual void InitializeSkins() {
        }

        internal virtual void InitializeDoppelganger() {
            Modules.Prefabs.CreateGenericDoppelganger(instance.bodyPrefab, bodyName + "MonsterMaster", "Merc");
        }

        internal virtual void InitializeItemDisplays() {

            ItemDisplayRuleSet itemDisplayRuleSet = ScriptableObject.CreateInstance<ItemDisplayRuleSet>();
            itemDisplayRuleSet.name = "idrs" + bodyName;

            characterModel.itemDisplayRuleSet = itemDisplayRuleSet;
        }

        internal void SetItemDisplays() {
            itemDisplays.SetItemDIsplays(characterModel.itemDisplayRuleSet);
        }

    }
}
