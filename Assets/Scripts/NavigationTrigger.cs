using UnityEngine;

public class NavigationTrigger : MonoBehaviour {

    public void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player") {
            GameObject.FindGameObjectWithTag("GameController").SendMessage("BuildNavigation");
        }
    }
}
