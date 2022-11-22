using UnityEngine;
using RoR2;
using UnityEngine.Networking;
using ModdedEntityStates.Desolator;

public class DesolatorAuraController : NetworkBehaviour {

    [SyncVar]
    public GameObject Owner;

    private ParticleSystem[] auraParticles;

    private bool _active;

    private float currentRadius {
        get => _active ? RadiationAura.Radius : 0;
    }

    private BuffWard buffward;
    private CharacterBody cachedOwnerBody;
    private float _scaleVelocity;
    private float _localScale;

    public void Init() {
        cachedOwnerBody = Owner.GetComponent<CharacterBody>();
        buffward = GetComponent<BuffWard>();
        auraParticles = GetComponentsInChildren<ParticleSystem>(true);

        Helpers.LogWarning($"Owner {Owner}");
        Helpers.LogWarning($"cachedOwnerBody {cachedOwnerBody}");

        Activate(true);
    }

    public void Activate(bool shouldActivate) {
        _active = shouldActivate;
        buffward.enabled = shouldActivate;
        buffward.radius = currentRadius;

        if (shouldActivate) {
            OnIciclesActivated();
        } else {
            OnIciclesDeactivated();
        }
    }

    void Update() {

        if (cachedOwnerBody) {
            this.transform.position = cachedOwnerBody.corePosition;

            _localScale = Mathf.SmoothDamp(this.transform.localScale.x, currentRadius, ref _scaleVelocity, 0.5f);
            this.transform.localScale = new Vector3(_localScale, _localScale, _localScale);
        }
    }

    private void OnIciclesActivated() {
        for (int i = 0; i < auraParticles.Length; i++) {
            ParticleSystem.MainModule main = auraParticles[i].main;
            main.loop = true;
            auraParticles[i].Play();
        }
    }

    private void OnIciclesDeactivated() {
        for (int i = 0; i < auraParticles.Length; i++) {
            ParticleSystem.MainModule main = auraParticles[i].main;
            main.loop = false;
        }
    }
}