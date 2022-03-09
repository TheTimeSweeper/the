using EntityStates;
using Modules;
using RoR2;
using UnityEngine;

namespace ModdedEntityStates.TeslaTrooper {
    public class TeslaTrooperMain : GenericCharacterMain {

        private int lastVoice = -1;

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
            int rand = getRandomVoiceLine();
            while (rand == lastVoice) {
                rand = getRandomVoiceLine();
            }

            RoR2.Util.PlaySound(Helpers.GetVoiceLineString((TeslaVoiceLine)rand), gameObject);
        }

        private int getRandomVoiceLine() {
            int rand;
            if (!characterBody.outOfCombat) {
                rand = Random.Range(0, 5);
                Debug.LogWarning("speak combat");
            } else {
                if (inputBank.moveVector != Vector3.zero) {
                    rand = Random.Range(5, 11);

                    Debug.LogWarning("speak out combat moving " + inputBank.moveVector);
                } else {
                    rand = Random.Range(11, 15);

                    Debug.LogWarning("speak out combat not moving");
                }
            }


            return rand;
        }
    }
}