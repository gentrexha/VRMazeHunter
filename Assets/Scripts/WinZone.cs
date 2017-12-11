using UnityEngine;

public class WinZone : MonoBehaviour {

    public void OnTriggerEnter(Collider collider) {
        if (collider.tag == "Player") {
            GameObject.FindGameObjectWithTag("Player").SendMessage("Finished");
        }
    }    
}
