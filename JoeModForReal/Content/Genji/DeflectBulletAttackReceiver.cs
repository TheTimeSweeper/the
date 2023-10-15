using RoR2;
using UnityEngine;

namespace JoeModForReal.Content.Survivors {
    public class DeflectBulletAttackReceiver : MonoBehaviour, IBulletAttackReceiver {

        public delegate void BulletAttackReceivedEvent(BulletAttack bulletAttack_, BulletAttack.BulletHit hitInfo_);
        public BulletAttackReceivedEvent OnBulletAttackReceived;

        public void receiveBulletAttack(BulletAttack bulletAttack_, BulletAttack.BulletHit hitInfo_) {

            OnBulletAttackReceived?.Invoke(bulletAttack_, hitInfo_);
        }
    }
}