using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour {

    public AudioMixer audioMixer;
    public TextMeshProUGUI highscoreEasy;
    public TextMeshProUGUI highscoreMedium;
    public TextMeshProUGUI highscoreHard;

    public void SetVolume(float volume) {
        audioMixer.SetFloat("Volume", volume);
    }

    public void SetQuality(int index) {
        QualitySettings.SetQualityLevel(index);
    }

    public void Start() {
        SetHighscores();
    }

    private void SetHighscores() {
        string strHighscoreEasy = PlayerPrefs.GetString("HighscoreEasy");
        string strHighscoreMedium = PlayerPrefs.GetString("HighscoreMedium");
        string strHighscoreHard = PlayerPrefs.GetString("HighscoreHard");
        // Easy
        if (strHighscoreEasy != "") {
            highscoreEasy.text = PlayerPrefs.GetString("HighscoreEasy");
        }
        else {
            highscoreEasy.text = "N/A";
        }
        // Medium
        if (strHighscoreMedium != "") {
            highscoreMedium.text = PlayerPrefs.GetString("HighscoreMedium");
        }
        else {
            highscoreMedium.text = "N/A";
        }
        // Hard
        if (strHighscoreHard != "") {
            highscoreHard.text = PlayerPrefs.GetString("HighscoreMedium");
        }
        else {
            highscoreHard.text = "N/A";
        }
    }
}
