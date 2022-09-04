using UnityEngine;
using RoR2;
using UnityEngine.Networking;

public class DesolatorAuraController : NetworkBehaviour {

    public static float Radius = 40;

    [SyncVar]
    public GameObject Owner;

    private ParticleSystem[] auraParticles;

    private bool _active;

    private float currentRadius {
        get => _active ? Radius : 0;
    }

    private BuffWard buffward;
    private CharacterBody cachedOwnerBody;
    private float _scaleVelocity;
    private float _currentScale;

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

        this.transform.position = cachedOwnerBody.corePosition;
        
        _currentScale = Mathf.SmoothDamp(this.transform.localScale.x, currentRadius, ref _scaleVelocity, 0.5f);
        this.transform.localScale = new Vector3(_currentScale, _currentScale, _currentScale);
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