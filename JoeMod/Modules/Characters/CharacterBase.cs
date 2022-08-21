using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.Characters {
    internal abstract class CharacterBase {

        public abstract string bodyName { get; }

        public abstract BodyInfo bodyInfo { get; set; }

        public abstract CustomRendererInfo[] customRendererInfos { get; set; }

        public abstract Type characterMainState { get; }
        public virtual Type characterSpawnState { get; }

        public abstract ItemDisplaysBase itemDisplays { get; }

        public virtual GameObject bodyPrefab { get; set; }
        public virtual CharacterModel bodyCharacterModel { get; set; }
        public string fullBodyName => bodyName + "Body";

        public virtual void Initialize() {
            InitializeCharacter();
        }

        public virtual void InitializeCharacter() {

            InitializeCharacterBodyAndModel();
            InitializeCharacterMaster();

            InitializeEntityStateMachine();
            InitializeSkills();

            InitializeHitboxes();

            InitializeSkins();
            InitializeItemDisplays();
        }
        
        protected virtual void InitializeCharacterBodyAndModel() {
            bodyPrefab = Modules.Prefabs.CreateBodyPrefab(bodyName + "Body", "mdl" + bodyName, bodyInfo);
            InitializeCharacterModel();
        }
        protected virtual void InitializeCharacterModel() {
            bodyCharacterModel = Modules.Prefabs.SetupCharacterModel(bodyPrefab, customRendererInfos);
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

        public virtual void InitializeSkills() { }

        public virtual void InitializeHitboxes() { }
        
        public virtual void InitializeSkins() { }

        public virtual void InitializeItemDisplays() {

                ItemDisplayRuleSet itemDisplayRuleSet = ScriptableObject.CreateInstance<ItemDisplayRuleSet>();
                itemDisplayRuleSet.name = "idrs" + bodyName;

                bodyCharacterModel.itemDisplayRuleSet = itemDisplayRuleSet;

            if (itemDisplays != null) {
                RoR2.ContentManagement.ContentManager.onContentPacksAssigned += SetItemDisplays;
            }
        }

        public void SetItemDisplays(HG.ReadOnlyArray<RoR2.ContentManagement.ReadOnlyContentPack> obj) {
                itemDisplays.SetItemDIsplays(bodyCharacterModel.itemDisplayRuleSet);
        }

    }
}
