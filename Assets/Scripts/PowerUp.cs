using System.Collections;
using Assets.Scripts;
using UnityEngine;

public class PowerUp : MonoBehaviour {

    private Transform target;
    private VrPlayerController _playerStats;
    public GameObject pickupEffect;
    public float pickupDuration = 4f;
    public float movSpeedBonus = 5f;

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            StartCoroutine(PickUp(other));
        }
    }

    void Start() {
	    target = PlayerManager.instance.player.transform;
	    _playerStats = target.GetComponent<VrPlayerController>();
    }

    IEnumerator PickUp(Collider player) {
        Instantiate(pickupEffect, transform.position, transform.rotation);
        _playerStats.movSpeed += movSpeedBonus;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(pickupDuration);
        _playerStats.movSpeed -= movSpeedBonus;
        Destroy(transform.parent.gameObject);
    }
}
