using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour {

    public int timeToDestroy = 30;

    void Start () {
		Destroy(gameObject,timeToDestroy);
	}
}
