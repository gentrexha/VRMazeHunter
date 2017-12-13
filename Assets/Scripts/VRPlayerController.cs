using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class VRPlayerController : MonoBehaviour {

    public float movSpeed = 3.0f;
    public bool moveForward;
    public GameObject footprint;
    public TextMeshProUGUI timerText;
    public GameObject highscoreText;
    public GameObject winText;
    public Transform bottomCanvas;
    public GameObject loadingArrow;
    public bool allowMovement;
    public GameObject endGameMenu;
    // Gun
    public float damage = 10f;
    public float range = 100f;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    public float impactForce = 30f;
    public float fireRate = 15f;
    public bool hasWeapon = false;
    public GameObject gun;

    private CharacterController _charController;
    private Transform _playerTransform;
    private AudioSource _audioSource;
    private float InputEvent_CooldownTimer;
    private float btnTimePressed;
    private float btnNumberOfPresses;
    private float _totalTime = 0f;
    private float _startTime;
    private bool _finished = false;
    private string _currentSceneName;
    private float _fillAmount;
    // Gun
    private float _nextTimeToFire = 0f;


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
        _currentSceneName = SceneManager.GetActiveScene().name;
        allowMovement = true;
    }
	
	void Update () {
//        Gun Management
	    if (hasWeapon) {
	        gun.SetActive(true);
	    } else if (!hasWeapon) {
	        gun.SetActive(false);
	    }
//        BottomCavnas rotation
	    bottomCanvas.transform.eulerAngles = new Vector3(90f,_playerTransform.transform.eulerAngles.y,0f);
//        Footprints Management
	    _totalTime += Time.deltaTime;
	    if (_totalTime>.5f) {
	        Instantiate(footprint, new Vector3(_playerTransform.position.x,_playerTransform.position.y-1.5f,_playerTransform.position.z), Quaternion.Euler(90f, _playerTransform.eulerAngles.y, 0));
	        _totalTime = 0;
	    }
//         Input Management. Needs improvement and to check if it's working properly or not.
	    if (Input.GetButton("Fire1") && InputEvent_CooldownTimer >= 0) {
	        btnTimePressed += 1;
	        loadingArrow.SetActive(true);
	        loadingArrow.GetComponent<Image>().fillAmount = btnTimePressed / 40f; // TODO: Don't hardcode!
	    }
	    else if (!Input.GetButton("Fire1") && btnTimePressed >= 0 && InputEvent_CooldownTimer > 0) {
	        btnTimePressed -= 1;
	        loadingArrow.GetComponent<Image>().fillAmount = btnTimePressed / 40f; // TODO: Don't hardcode!
	        if (btnTimePressed == -1f) loadingArrow.SetActive(false);
	    }
	    if (Input.GetButtonUp("Fire1") && InputEvent_CooldownTimer >= 0)
	        if (btnTimePressed < 40 && btnNumberOfPresses < 2) {
	            // Trigger OneQuickPress Event Here
	            btnTimePressed = 0;
	            lastInputEvent = InputEvent.OneShortPress;
	            InputEvent_CooldownTimer = 30;
	            btnTimePressed = -1;
	            loadingArrow.SetActive(false);
	            // Shot event
	            if (hasWeapon) {
	                if (Time.time >= _nextTimeToFire) {
	                    _nextTimeToFire = Time.time + 1f / fireRate;
	                    Shoot();
	                }
	            }
	        }
	        // Disabled double press for now
	        //            else if (btnTimePressed >= 15 && btnTimePressed <= 40) {
	        //                if (btnNumberOfPresses < 1) {
	        //                    btnNumberOfPresses += 1;
	        //                }
	        //                else {
	        //                    // Trigger DoublePressEvent Here
	        //                    lastInputEvent = InputEvent.DoublePress;
	        //                    InputEvent_CooldownTimer = 30;
	        //                    btnNumberOfPresses = 0;
	        //                }
	        //            }
	        else if (btnTimePressed > 40) {
	            // Trigger LongHeldEvent Here
	            lastInputEvent = InputEvent.LongHeldPress;
	            InputEvent_CooldownTimer = 30;
	            // Walk forward
	            moveForward = !moveForward;
	            btnTimePressed = -1;
	            loadingArrow.SetActive(false);
	        }
//         Moving Management
	    if (moveForward) {
	        var forward = _playerTransform.TransformDirection(Vector3.forward);
	        _charController.SimpleMove(forward * movSpeed);
	    }
	    else {
	        _charController.SimpleMove(Vector3.zero);
	    }
	    //	    transform.eulerAngles = new Vector3(transform.localEulerAngles.x,_playerTransform.localEulerAngles.y * rotateSpeed,transform.localEulerAngles.z);
//         Step Sound Management. Needs improvement!
	    if (_charController.isGrounded && _charController.velocity.magnitude > 2f && !_audioSource.isPlaying) {
	        _audioSource.volume = Random.Range(0.8f, 1f);
	        _audioSource.pitch = Random.Range(0.8f, 1.1f);
	        _audioSource.Play();
	    }
	    else if (_charController.velocity.magnitude < 0.1f && _audioSource.isPlaying) {
	        _audioSource.Stop();
	    }
	    
//         Timer Management!
	    if (!_finished) {
	        float t = Time.time - _startTime;
	        string minutes = ((int)t / 60).ToString();
	        string seconds = (t % 60).ToString("f2");
	        timerText.text = "00:" + minutes + ":" + seconds;
        }
	}

    void Finished() {
        timerText.faceColor = Color.yellow;
        _finished = true;
        winText.SetActive(true);
        StoreTime(timerText.text);
        // Ask user if he want's to restart or go back
    }

    IEnumerator Restart() {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("");
    }

    public void StoreTime(string timeVal)
    {
        string strCurrentHighscore = PlayerPrefs.GetString("Highscore"+_currentSceneName);
        if (strCurrentHighscore != "")
        {
            var newTimeSpan = TimeSpan.Parse(timeVal);
            var highscoreTimeSpan = TimeSpan.Parse(strCurrentHighscore);
            if (newTimeSpan < highscoreTimeSpan)
            {
                PlayerPrefs.SetString("Highscore"+_currentSceneName, timeVal);
                highscoreText.SetActive(true);
            }
        }
        else
        {
            PlayerPrefs.SetString("Highscore"+_currentSceneName, timeVal);
            highscoreText.SetActive(true);
        }
    }

    public void DeleteHighscore() {
        PlayerPrefs.DeleteKey("Highscore"+_currentSceneName);
    }

    void Shoot() {
        muzzleFlash.Play();
        RaycastHit hitInfo;
        if (Physics.Raycast(_playerTransform.transform.position, _playerTransform.transform.forward, out hitInfo, range)) {
            EnemyController _enemy = hitInfo.transform.GetComponent<EnemyController>();
            if (_enemy != null) {
                _enemy.TakeDamage(damage);
            }
            if (hitInfo.rigidbody != null) {
                hitInfo.rigidbody.AddForce(-hitInfo.normal * impactForce);
            }
            GameObject impactGameObject = Instantiate(impactEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            Destroy(impactGameObject,2f);
        }
    }
}