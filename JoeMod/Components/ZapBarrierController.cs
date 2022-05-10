using RoR2;
using UnityEngine;

public class ZapBarrierController : MonoBehaviour, IBarrier {

    private bool _recordingDamage;
    private float _recordedDamage;
    public delegate void BlockedDamageEvent(float damageBlocked);
    public BlockedDamageEvent onBlockedDamage;
                                                     //I guess I would need IL for this
    public void BlockedDamage(DamageInfo damageInfo, float actualDamageBlocked) {

        if (_recordingDamage) {

            _recordedDamage += actualDamageBlocked;
            onBlockedDamage?.Invoke(actualDamageBlocked);
            //Helpers.LogWarning("zap blocked " + actualDamageBlocked + "/" + _recordedDamage);
        }
    }

    public void StartRecordingDamage() {
        _recordingDamage = true;
    }

    public float RedeemDamage() {
        float redeemed = _recordedDamage;

        _recordingDamage = false;
        _recordedDamage = 0;

        //Helpers.LogWarning("zap redeeming " + redeemed + " damage");
        return redeemed;
    }
}
