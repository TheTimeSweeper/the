using UnityEngine;
using UnityEngine.Networking;

public class DesolatorAuraHolder : NetworkBehaviour {

    [SyncVar]
    private GameObject _spawnedAuraObject = null;
    private DesolatorAuraController _spawnedAura = null;

    public void ActivateAura() {
        if (_spawnedAuraObject == null) {
            SpawnAura();
        }

        if (_spawnedAura == null) {
            Helpers.LogWarning("fuck");
        }
        _spawnedAura?.Activate(true);
    }

    public void DeactivateAura() {
        _spawnedAura?.Activate(false);
    }

    public void SpawnAura() {

        if (NetworkServer.active) {
            GameObject spawnedAuraObject = Instantiate(Modules.Assets.DesolatorAuraPrefab, base.transform.position, Quaternion.identity);
            _spawnedAura = spawnedAuraObject.GetComponent<DesolatorAuraController>();
            _spawnedAura.Owner = gameObject;
            NetworkServer.Spawn(spawnedAuraObject);
            _spawnedAuraObject = spawnedAuraObject;
            _spawnedAura.Init();
        }
    }
    
    void OnDestroy() {
        if (_spawnedAuraObject) {
            NetworkServer.Destroy(_spawnedAuraObject);
        }
    }
}
