using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.Characters {
    internal abstract class CharacterBase {

        public abstract string characterName { get; }

        public abstract BodyInfo bodyInfo { get; set; }

        public abstract CustomRendererInfo[] customRendererInfos { get; set; }

        public abstract Type characterMainState { get; }
        public virtual Type characterSpawnState { get; }

        public abstract ItemDisplaysBase itemDisplays { get; }

        public virtual GameObject bodyPrefab { get; set; }
        public virtual CharacterModel bodyCharacterModel { get; set; }
        public BodyIndex bodyIndex;
        public string fullBodyName => characterName + "Body";

        public virtual void Initialize() {
            InitializeCharacter();

            RoR2.ContentManagement.ContentManager.onContentPacksAssigned += lateInit;
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
            bodyPrefab = Modules.Prefabs.CreateBodyPrefab(characterName + "Body", "mdl" + characterName, bodyInfo);
            InitializeCharacterModel();
        }
        protected virtual void InitializeCharacterModel() {
            bodyCharacterModel = Modules.Prefabs.SetupCharacterModel(bodyPrefab, customRendererInfos);
        }

        protected virtual void InitializeCharacterMaster() { }
        protected virtual void InitializeEntityStateMachine() {
            bodyPrefab.GetComponent<EntityStateMachine>().mainStateType = new global::EntityStates.SerializableEntityStateType(characterMainState);
            Content.AddEntityState(characterMainState);
            if (characterSpawnState != null) {
                bodyPrefab.GetComponent<EntityStateMachine>().initialStateType = new global::EntityStates.SerializableEntityStateType(characterSpawnState);
                Content.AddEntityState(characterSpawnState);
            }
        }

        public virtual void InitializeSkills() { }

        public virtual void InitializeHitboxes() { }
        
        public virtual void InitializeSkins() { }

        public virtual void InitializeItemDisplays() {

                ItemDisplayRuleSet itemDisplayRuleSet = ScriptableObject.CreateInstance<ItemDisplayRuleSet>();
                itemDisplayRuleSet.name = "idrs" + characterName;

                bodyCharacterModel.itemDisplayRuleSet = itemDisplayRuleSet;
        }

        public virtual void lateInit(HG.ReadOnlyArray<RoR2.ContentManagement.ReadOnlyContentPack> obj) {

            bodyIndex = BodyCatalog.FindBodyIndex(bodyInfo.bodyName);

            if (itemDisplays != null) {
                itemDisplays.SetItemDIsplays(bodyCharacterModel.itemDisplayRuleSet);
            }
        }
    }
}
