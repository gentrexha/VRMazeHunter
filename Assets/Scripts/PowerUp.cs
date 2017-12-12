using System.Collections;
using UnityEngine;

public class PowerUp : MonoBehaviour {

    public GameObject pickupEffect;
    public float pickupDuration = 4f;
    public float movSpeedBonus = 5f;

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            StartCoroutine(PickUp(other));
        }
    }

    IEnumerator PickUp(Collider player) {
        Instantiate(pickupEffect, transform.position, transform.rotation);
        VRPlayerController playerController = player.GetComponent<VRPlayerController>();
        playerController.movSpeed += movSpeedBonus;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(pickupDuration);
        playerController.movSpeed -= movSpeedBonus;
        Destroy(gameObject);
    }
}
