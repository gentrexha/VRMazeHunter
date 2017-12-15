using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
public class EnemyController : MonoBehaviour {

    public float lookRadius = 10f;
    private Transform target;
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private AudioSource _audioSource;
    public float health = 50f;

    public void TakeDamage(float amount) {
        health -= amount;
        if (health <= 0f) {
            Die();
        }
    }

    void Die() {
        _animator.SetBool("IsDead",true);
        _navMeshAgent.isStopped = true;
        Destroy(gameObject,2f);
    }

	void Start () {
	    target = PlayerManager.instance.player.transform;
	    _navMeshAgent = GetComponent<NavMeshAgent>();
	    _animator = GetComponent<Animator>();
	    _audioSource = GetComponent<AudioSource>();
	}
	
	void Update () {
	    _animator.SetFloat("Velocity", _navMeshAgent.velocity.magnitude);
        float distance = Vector3.Distance(target.position, transform.position);
	    if (distance <= lookRadius) {
	        _navMeshAgent.SetDestination(target.position);
	        if (distance <= _navMeshAgent.stoppingDistance) {
                FaceTarget();
	            // Attack the target
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
