using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class VRAutoWalk : MonoBehaviour {

    public float movSpeed = 3f;
    public bool moveForward;
    private CharacterController _charController;
    private Transform _playerTransform;

    private float InputEvent_CooldownTimer;
    private float btnTimePressed;
    private float btnNumberOfPresses;

    //OneShortPress - 0
    //DoublePress - 1
    //LongHeldPress - 2

    public enum InputEvent { OneShortPress, DoublePress, LongHeldPress, Idle }
    public InputEvent lastInputEvent = InputEvent.Idle;

    void Start () {
	    _charController = GetComponent<CharacterController>();
	    _playerTransform = Camera.main.transform;

	    InputEvent_CooldownTimer = 0; // findout what this is for
	    btnTimePressed = -1;
	    btnNumberOfPresses = 0;
    }
	
	void Update () {
        if (Input.GetButton("Fire1") && InputEvent_CooldownTimer >= 0) {
            btnTimePressed += 1;
        } else if (!Input.GetButton("Fire1") && btnTimePressed >= 0 && InputEvent_CooldownTimer > 0) {
            btnTimePressed -= 1;
        }

        if (Input.GetButtonUp("Fire1") && InputEvent_CooldownTimer >= 0) {
            if (btnTimePressed < 10 && btnNumberOfPresses < 2) {
                //Trigger OneQuickPress Event Here
                btnTimePressed = 0;
                lastInputEvent = InputEvent.OneShortPress;
                InputEvent_CooldownTimer = 30;
                moveForward = !moveForward;
            }
            else if (btnTimePressed >= 10 && btnTimePressed <= 40) {
                if (btnNumberOfPresses < 1) {
                    btnNumberOfPresses += 1;
                }
                else {
                    //Trigger DoublePressEvent Here
                    lastInputEvent = InputEvent.DoublePress;
                    InputEvent_CooldownTimer = 30;
                    btnNumberOfPresses = 0;
                }
            }
            else if (btnTimePressed > 40) {
                //Trigger LongHeldEvent Here
                lastInputEvent = InputEvent.LongHeldPress;
                InputEvent_CooldownTimer = 30;
            }
        }
        if (moveForward) {
            Vector3 forward = _playerTransform.TransformDirection(Vector3.forward);
            _charController.SimpleMove(forward * movSpeed);
        }
    }
}
