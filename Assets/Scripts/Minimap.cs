using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour {

    private Transform _playerTransform;
    public Transform _iconTransform;

    private void Start() {
        _playerTransform = Camera.main.transform;
    }

    private void LateUpdate() {
        Vector3 newPos = _playerTransform.position;
        newPos.y = _playerTransform.position.y;
        transform.position = newPos;
        transform.eulerAngles = new Vector3(transform.localEulerAngles.x, _playerTransform.localEulerAngles.y, transform.localEulerAngles.z);
        _iconTransform.eulerAngles = new Vector3(transform.localEulerAngles.x, _playerTransform.localEulerAngles.y, transform.localEulerAngles.z);
    }
}
