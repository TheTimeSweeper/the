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
            
            if (isAuthority && Input.GetKeyDown(Modules.Config.voiceKey.Value)) {

                playRandomvoiceLine();
            }
            
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

        protected virtual void playRandomvoiceLine(string prefix = "Play_") {
            string sound;
            if (characterBody.outOfCombat) {
                if (characterBody.isSprinting) {
                    sound = $"{prefix}Voiceline_Move";
                } else {
                    sound = $"{prefix}Voiceline_Select";
                }
            } else {
                sound = $"{prefix}Voiceline_Attack";
            }

            RoR2.Util.PlaySound(sound, gameObject);
        }
        
        //reinventing the wheel wwise already does
        //private int getRandomVoiceLine() {
        //    int rand;
        //    if (!characterBody.outOfCombat) {
        //        rand = Random.Range(0, 5);
        //        Helpers.LogWarning("speak combat");
        //    } else {
        //        if (inputBank.moveVector != Vector3.zero) {
        //            rand = Random.Range(5, 11);
        //        } else {
        //            rand = Random.Range(11, 15);

        //            Helpers.LogWarning("speak out combat not moving");
        //        }
        //    }

        //    return rand;
        //}
    }
}