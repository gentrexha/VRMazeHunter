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
    public GameObject enemiesGO;
    public GameObject enemy;
    public GameObject powerUpGO;
    public GameObject powerUp;
    public int numPowerUps = 3;
    public int numEnemies = 3;
    public MazeLoader mazeLoader;

    private int rows;
    private int columns;

    void Awake() {
        instance = this;
    }

	void Start () {
	    _audioSource = GetComponent<AudioSource>();
	    mazeLoader = GetComponent<MazeLoader>();
	    rows = mazeLoader.mazeRows;
	    columns = mazeLoader.mazeColumns;
	    // Spawn PowerUps
	    for (int i = 0; i < numPowerUps; i++) {
	        float xCoord = 6 * Random.Range(1,rows) + 1.22f;
	        float zCoord = 6 * Random.Range(1,columns) - 2.134f;
            Vector3 newPos = new Vector3(xCoord,-6.5f,zCoord);
	        Instantiate(powerUp, newPos, transform.rotation,powerUpGO.transform);
        }
        // Spawn Random Enemies
        for (int i = 0; i < numEnemies; i++) {
            int xCoord = 6 * Random.Range(1, rows);
            int zCoord = 6 * Random.Range(1, columns);
            Vector3 newPos = new Vector3(xCoord,-1.6f,zCoord);
            Instantiate(enemy, newPos, transform.rotation,enemiesGO.transform);
        }
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
