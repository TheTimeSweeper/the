using RoR2;

namespace JoeModForReal.Content.Survivors {
    public interface IBulletAttackReceiver {
        void receiveBulletAttack(BulletAttack bulletAttack_, BulletAttack.BulletHit hitInfo_);
    }
}