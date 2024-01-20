using UnityEngine;
using UnityEngine.AI;

public class testo: MonoBehaviour {
    NavMeshAgent agent;

    [SerializeField]
    private GameObject target;
    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update() {
        agent.SetDestination(target.transform.position);
    }
}
