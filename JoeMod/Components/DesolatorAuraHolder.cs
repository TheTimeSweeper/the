using UnityEngine;
using UnityEngine.Networking;

public class DesolatorAuraHolder : NetworkBehaviour {

    [SyncVar]
    private GameObject _spawnedAuraObject = null;
    private DesolatorAuraController _spawnedAura = null;

    void Awake() {
        SpawnAura();
    }

    public void SpawnAura() {

        if (NetworkServer.active) {
            GameObject spawnedAuraObject = Instantiate(Modules.Assets.DesolatorAuraPrefab, base.transform.position, Quaternion.identity);
            NetworkServer.Spawn(spawnedAuraObject);
            _spawnedAuraObject = spawnedAuraObject;
            //_spawnedAura = _spawnedAuraObject.GetComponent<DesolatorAuraController>();
        }
    }

    public void ActivateAura() {

        if (_spawnedAura == null) {
            _spawnedAura = _spawnedAuraObject.GetComponent<DesolatorAuraController>();
            if (NetworkServer.active) {
                _spawnedAura.RpcSetOwner(gameObject);
            }
        }

        if (_spawnedAura == null) {
            Helpers.LogWarning("fuck2");
        }
        _spawnedAura?.Activate(true);
    }

    public void DeactivateAura() {
        _spawnedAura?.Activate(false);
    }
    
    void OnDestroy() {
        if (_spawnedAuraObject) {
            NetworkServer.Destroy(_spawnedAuraObject);
        }
    }
}
