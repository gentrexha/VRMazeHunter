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
	        if (distance <= agent.stoppingDistance) {
	            // Attack the target
                FaceTarget();
	        }
	    }
	}

    void FaceTarget() {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x,0,direction.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,lookRadius);
    }
}
