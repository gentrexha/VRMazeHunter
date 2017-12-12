using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {

    public float lookRadius = 10f;
    private Transform target;
    private NavMeshAgent agent;
    public float health = 50f;

    public void TakeDamage(float amount) {
        health -= amount;
        if (health <= 0f) {
            Die();
        }
    }

    void Die() {
        Destroy(gameObject);
    }

	void Start () {
	    target = PlayerManager.instance.player.transform;
	    agent = GetComponent<NavMeshAgent>();
	}
	
	void Update () {
	    float distance = Vector3.Distance(target.position, transform.position);
	    if (distance <= lookRadius) {
	        agent.SetDestination(target.position);
	    }
	}

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,lookRadius);
    }
}
