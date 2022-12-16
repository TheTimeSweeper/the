using BepInEx.Configuration;
using EntityStates;
using Modules;
using RoR2;
using System;
using UnityEngine;

namespace ModdedEntityStates.TeslaTrooper {

    public class TeslaTrooperMain : GenericCharacterMain {
        
        private Animator cachedAnimator;

        public override void OnEnter() {
            base.OnEnter();
            cachedAnimator = GetModelAnimator();
        }

        public override void Update() {
            base.Update();
            bool combat = !characterBody.outOfCombat;

            cachedAnimator.SetBool("inCombat", combat);

            if (base.isAuthority && base.characterMotor.isGrounded) {

                CheckEmote(Modules.Config.restKeybind.Value, new Rest());
            }
        }

        private void CheckEmote(KeyCode keybind, EntityState state) {
            if (Input.GetKeyDown(keybind)) {
                if (!LocalUserManager.readOnlyLocalUsersList[0].isUIFocused) {
                    outer.SetInterruptState(state, InterruptPriority.Any);
                }
            }
        }
    }
}