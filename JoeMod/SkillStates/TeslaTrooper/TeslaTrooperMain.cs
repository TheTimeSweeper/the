using EntityStates;
using Modules;
using RoR2;
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
            
            if (Input.GetKeyDown(KeyCode.CapsLock)) {

                playRandomvoiceLine();
                //Util.PlaySound("Play_itesatd", gameObject);
            }
        }

        private void playRandomvoiceLine() {
            string sound;
            if (characterBody.outOfCombat) {
                if (inputBank.moveVector != Vector3.zero) {
                    sound = "Play_Voiceline_Move";
                } else {
                    sound = "Play_Voiceline_Select";
                }
            } else {
                sound = "Play_Voiceline_Attack";
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