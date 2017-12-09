using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveText : MonoBehaviour {

    public float moveSpeed = 3f;
    public GameObject mainMenu;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    transform.position = new Vector3(transform.position.x,transform.position.y,transform.position.z - moveSpeed * Time.deltaTime);

	    if (transform.position.z < -4) {
	        mainMenu.SetActive(true);
	    }
	    if (transform.position.z < -8)
	    {
            // Destroy itself here. Learn how to do that later
	    }
    }
}
