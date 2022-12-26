using BepInEx.Configuration;
using EntityStates;
using Modules;
using RoR2;
using System;
using UnityEngine;

namespace ModdedEntityStates.TeslaTrooper {

    public class TeslaTrooperMain : GenericCharacterMain {
        
        private Animator cachedAnimator;

        public LocalUser localUser;

        public override void OnEnter() {
            base.OnEnter();
            cachedAnimator = GetModelAnimator();
        }

        public override void Update() {
            base.Update();
            bool combat = !characterBody.outOfCombat;

            cachedAnimator.SetBool("inCombat", combat);

            if (base.isAuthority && base.characterMotor.isGrounded) {

                CheckEmote<Rest>(Modules.Config.restKeybind);
            }
        }

        private void FindLocalUser() {
            if (localUser == null) {
                if (base.characterBody) {
                    foreach (LocalUser lu in LocalUserManager.readOnlyLocalUsersList) {
                        if (lu.cachedBody == base.characterBody) {
                            this.localUser = lu;
                            break;
                        }
                    }
                }
            }
        }

        private void CheckEmote<T>(ConfigEntry<KeyboardShortcut> keybind) where T : EntityState, new() {
            if (Modules.Config.GetKeyPressed(keybind)) {

                FindLocalUser();
                
                if (localUser != null && !localUser.isUIFocused) {
                    outer.SetInterruptState(new T(), InterruptPriority.Any);
                }
            }
        }
    }
}