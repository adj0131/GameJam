using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneControl : MonoBehaviour
{
    public Image fadeImage;
    [SerializeField] public float fadeDuration = 1f;

    void Awake()
    {
        DontDestroyOnLoad(gameObject); // makes this persistent. Should be only 1 so don't have to worry about singleton right now
    }
    public void LoadNextScene()
    {
        StartCoroutine(FadeAndLoad());
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    IEnumerator FadeAndLoad()
    {
        float timer = 0f;
        Color color = fadeImage.color;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            color.a = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            fadeImage.color = color;

            yield return null;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // finds index of active scene, adds one, then loads next scene

        // After waiting a single frame (for next scene to load), restore color
        yield return null;
        color.a = Mathf.Lerp(0f, 1f, 0f);
        fadeImage.color = color;
    }
}
