using RoR2;
using UnityEngine;

namespace JoeModForReal.Content.Survivors {
    public class DeflectAttackReceiver : MonoBehaviour, IBulletAttackReceiver, IGolemLaserReceiver {

        public delegate void BulletAttackReceivedEvent(BulletAttack bulletAttack_, BulletAttack.BulletHit hitInfo_);
        public BulletAttackReceivedEvent OnBulletAttackReceived;

        public delegate void GolemLaserReceivedEvent();
        public GolemLaserReceivedEvent OnGolemLaserReceived;


        public void receiveBulletAttack(BulletAttack bulletAttack_, BulletAttack.BulletHit hitInfo_) {

            OnBulletAttackReceived?.Invoke(bulletAttack_, hitInfo_);
        }

        public void receiveGolemLaser() {
            OnGolemLaserReceived?.Invoke();
        }
    }
}