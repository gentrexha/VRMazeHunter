using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinZone : MonoBehaviour {

    public void OnTriggerEnter(Collider collider) {
        GameObject.FindGameObjectWithTag("Player").SendMessage("Finished");
    }
}
