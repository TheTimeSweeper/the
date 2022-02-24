using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace HenryMod.Modules.Characters {
    public abstract class CharacterBase {

        public static CharacterBase instance;

        public abstract string bodyName { get; }

        public abstract BodyInfo bodyInfo { get; set; }

        public abstract CustomRendererInfo[] customRendererInfos { get; set; }

        public abstract Type characterMainState { get; }
        public virtual Type characterSpawnState { get; }

        public abstract ItemDisplaysBase itemDisplays { get; }

        public virtual GameObject bodyPrefab { get; set; }
        public virtual CharacterModel characterModel { get; set; }
        public string fullBodyName => bodyName + "Body";

        public virtual void Initialize() {
            instance = this;
            InitializeCharacter();
        }

        public virtual void InitializeCharacter() {

            InitializeCharacterBodyAndModel();
            InitializeCharacterModel();
            InitializeCharacterMaster();

            InitializeEntityStateMachine();
            InitializeSkills();

            InitializeHitboxes();
            InitializeHurtboxes();

            InitializeSkins();
            InitializeItemDisplays();

            //survivor?
            InitializeDoppelganger();
        }

        protected virtual void InitializeCharacterBodyAndModel() {
            bodyPrefab = Modules.Prefabs.CreateBodyPrefab(bodyName + "Body", "mdl" + bodyName, bodyInfo);
        }
        protected virtual void InitializeCharacterModel() {
            characterModel = Modules.Prefabs.SetupCharacterModel(bodyPrefab, customRendererInfos);
        }

        protected virtual void InitializeCharacterMaster() { }
        protected virtual void InitializeEntityStateMachine() {
            bodyPrefab.GetComponent<EntityStateMachine>().mainStateType = new EntityStates.SerializableEntityStateType(characterMainState);
            States.entityStates.Add(characterMainState);
            if (characterSpawnState != null) {
                bodyPrefab.GetComponent<EntityStateMachine>().initialStateType = new EntityStates.SerializableEntityStateType(characterSpawnState);
                States.entityStates.Add(characterSpawnState);
            }
        }

        public abstract void InitializeSkills();

        public virtual void InitializeHitboxes() { }

        public virtual void InitializeHurtboxes() {
            Modules.Prefabs.SetupHurtBoxes(bodyPrefab);
        }

        public virtual void InitializeSkins() { }

        public virtual void InitializeDoppelganger() {
            Modules.Prefabs.CreateGenericDoppelganger(instance.bodyPrefab, bodyName + "MonsterMaster", "Merc");
        }

        public virtual void InitializeItemDisplays() {

            ItemDisplayRuleSet itemDisplayRuleSet = ScriptableObject.CreateInstance<ItemDisplayRuleSet>();
            itemDisplayRuleSet.name = "idrs" + bodyName;

            characterModel.itemDisplayRuleSet = itemDisplayRuleSet;
        }

        public void SetItemDisplays() {
            itemDisplays.SetItemDIsplays(characterModel.itemDisplayRuleSet);
        }

    }
}
