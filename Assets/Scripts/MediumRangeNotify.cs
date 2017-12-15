using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumRangeNotify : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider collider) {
        if (collider.tag == "Player") {
            GameManager.instance.MediumRange();
        }
    }

    void OnTriggerExit(Collider collider) {
        if (collider.tag == "Player") {
            GameManager.instance.LargeRange();
        }
    }
}
