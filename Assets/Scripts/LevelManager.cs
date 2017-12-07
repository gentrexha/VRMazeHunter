using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    public Slider slider;

    public void PlayGame() {
        SceneManager.LoadScene("Sandbox");
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void LoadScene(int sceneIndex) {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously(int sceneIndex) {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        while (!operation.isDone) {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progress;
            yield return null;
        }
    }
}
