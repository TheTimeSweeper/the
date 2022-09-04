using RoR2;
using UnityEngine;

public class TeslaZapBarrierController : MonoBehaviour, IReflectionBarrier {

    private bool _recordingDamage;
    private float _recordedDamage;
    
    public delegate void StoredDamageEvent(float damageStored);
    public StoredDamageEvent OnStoredDamage;
    
    public void StoreDamage(DamageInfo damageInfo, float damageStored) {
        if (_recordingDamage) {
            //Helpers.LogWarning("zap blocked " + damageStored + "/" + _recordedDamage);
            OnStoredDamage?.Invoke(damageStored);
            _recordedDamage += damageStored;
        }
    }

    public void StartRecordingDamage() {
        _recordingDamage = true;
    }

    public float GetReflectedDamage() {
        float redeemed = _recordedDamage;

        _recordingDamage = false;
        _recordedDamage = 0;

        //Helpers.LogWarning("zap redeeming " + redeemed + " damage");
        return redeemed;
    }
}
