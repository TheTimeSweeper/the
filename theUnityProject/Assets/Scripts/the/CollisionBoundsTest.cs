using UnityEngine;

public class CollisionBoundsTest : MonoBehaviour {

    [SerializeField]
    private Transform _target;
    [SerializeField]
    private Collider _collider;

    private void Update() {
        _target.position = _collider.bounds.center + Vector3.up * (_collider.bounds.max.y - _collider.bounds.center.y);
    }
}
