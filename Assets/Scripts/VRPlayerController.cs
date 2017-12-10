using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class VRPlayerController : MonoBehaviour {

    public float movSpeed = 3.0f;
//    public float rotateSpeed = 1.5f;
    public bool moveForward;
    public GameObject footprint;
    public TextMeshProUGUI timerText;

    private CharacterController _charController;
    private Transform _playerTransform;
    private AudioSource _audioSource;
    private float InputEvent_CooldownTimer;
    private float btnTimePressed;
    private float btnNumberOfPresses;
    private float _totalTime = 0f;
    private float _startTime;
    private bool _finished = false;

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

        _startTime = Time.time;
    }
	
	void Update () {
	    _totalTime += Time.deltaTime;
	    if (_totalTime>.5f) {
	        Instantiate(footprint, new Vector3(_playerTransform.position.x,_playerTransform.position.y-1.5f,_playerTransform.position.z), Quaternion.Euler(90f, _playerTransform.eulerAngles.y, 0));
	        _totalTime = 0;
	    }
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
//         Step Sound Management. Needs improvement!
        if (_charController.isGrounded && _charController.velocity.magnitude>2f && !_audioSource.isPlaying) {
	        _audioSource.volume = Random.Range(0.8f,1f);
	        _audioSource.pitch = Random.Range(0.8f,1.1f);
            _audioSource.Play();
        } else if (_charController.velocity.magnitude < 0.1f && _audioSource.isPlaying) {
            _audioSource.Stop();
        }
        // Timer Management!
	    if (!_finished) {
	        float t = Time.time - _startTime;
	        string minutes = ((int)t / 60).ToString();
	        string seconds = (t % 60).ToString("f2");
	        timerText.text = minutes + ":" + seconds;
        }
	}

    void Finished() {
        timerText.faceColor = Color.yellow;
        _finished = true;
    }
}
