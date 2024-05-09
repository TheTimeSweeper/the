using RA2Mod.Survivors.Tesla.States;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

public class TeslaZapBarrierController : NetworkBehaviour, IReflectionBarrier {

    private bool _recordingDamage;
    private float _recordedDamage;
    
    public delegate void StoredDamageEvent(float damageStored);
    public StoredDamageEvent OnStoredDamage;
    
    public void StoreDamage(DamageInfo damageInfo, float damageStored) {
        if (_recordingDamage) {
            //Helpers.LogWarning("zap blocked " + damageStored + "/" + _recordedDamage);
            OnStoredDamage?.Invoke(damageStored);
            _recordedDamage += damageStored;

            PlaySound(Mathf.Max(ShieldZapReleaseDamage.MaxDamageCoefficient, _recordedDamage / 5));
        }
    }

    public void PlaySound(float pitch)
    {
        //Helpers.LogWarning(pitch);
        RpcPlaySoundToClients(pitch);
        PlaySoundInternal(pitch);
    }
    [ClientRpc]
    public void RpcPlaySoundToClients(float pitch)
    {
        PlaySoundInternal(pitch);
    }

    private void PlaySoundInternal(float pitch)
    {
        var num = Util.PlaySound("Play_Tesla_ShieldTakeDamage", gameObject);
        AkSoundEngine.SetRTPCValueByPlayingID("Pitch_TeslaCharge", pitch, num);
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
