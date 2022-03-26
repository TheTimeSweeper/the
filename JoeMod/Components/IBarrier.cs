using RoR2;
using UnityEngine;

public interface IBarrier {
    void BlockedDamage(DamageInfo damageInfo, float actualDamageBlocked);
}

public class ZapBarrierController : MonoBehaviour, IBarrier {

    private bool _recordingDamage;
    private float _recordedDamage;
                                                     //I guess I would need IL for this
    public void BlockedDamage(DamageInfo damageInfo, float actualDamageBlocked) {

        if (!_recordingDamage)
            return;

        _recordedDamage += actualDamageBlocked;
    }

    public void STartRecordingDamage() {
        _recordingDamage = true;
        _recordedDamage = 0;
    }

    public float redeemDamage() {
        float redeemed = _recordedDamage;

        _recordingDamage = false;
        _recordedDamage = 0;

        return redeemed;
    }
}
