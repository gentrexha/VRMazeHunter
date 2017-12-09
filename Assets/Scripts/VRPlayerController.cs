using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class VRPlayerController : MonoBehaviour {

    public float movSpeed = 3.0f;
//    public float rotateSpeed = 1.5f;
    public bool moveForward;
    private CharacterController _charController;
    private Transform _playerTransform;
    private AudioSource _audioSource;

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
        _audioSource = GetComponent<AudioSource>();
	    _playerTransform = Camera.main.transform;

	    InputEvent_CooldownTimer = 0; // findout what this is for
	    btnTimePressed = -1;
	    btnNumberOfPresses = 0;
    }
	
	void Update () {

        // Input Management
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
        // Moving Management
        if (moveForward) {
            Vector3 forward = _playerTransform.TransformDirection(Vector3.forward);
            _charController.SimpleMove(forward * movSpeed);
        }
        else {
            _charController.SimpleMove(Vector3.zero);
        }
        //	    transform.eulerAngles = new Vector3(transform.localEulerAngles.x,_playerTransform.localEulerAngles.y * rotateSpeed,transform.localEulerAngles.z);
//         Step Sound Management. Doesn't work as of now!
        if (_charController.isGrounded && _charController.velocity.magnitude>2f && !_audioSource.isPlaying) {
	        _audioSource.volume = Random.Range(0.8f,1f);
	        _audioSource.pitch = Random.Range(0.8f,1.1f);
            _audioSource.Play();
        } else if (_charController.velocity.magnitude < 0.1f && _audioSource.isPlaying) {
            _audioSource.Stop();
        }
    }
}
