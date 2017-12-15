using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour {

    private AudioSource _audioSource;

    // Public
    public AudioClip MediumRangeAudioClip;
    public AudioClip CloseRangeAudioClip;
    public AudioClip LongRangeAudioClip;
    public static GameManager instance;

    void Awake() {
        instance = this;
    }

	void Start () {
	    _audioSource = GetComponent<AudioSource>();
        // Spawn Random Pickups
        // Spawn Random Enemies
	}
	
	void Update () {
		
	}

    public void CloseRange() {
        _audioSource.clip = CloseRangeAudioClip;
        _audioSource.Play();
    }

    public void MediumRange() {
        _audioSource.clip = MediumRangeAudioClip;
        _audioSource.Play();
    }

    public void LargeRange() {
        _audioSource.clip = LongRangeAudioClip;
        _audioSource.Play();
    }
}
