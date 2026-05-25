using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneControl : MonoBehaviour
{
    public Image fadeImage;
    [SerializeField] public float fadeDuration = 1f;

    [Header("Game Over")]
    public GameObject gameOverPanel;

    [Header("You Win")]
    public GameObject youWinPanel;

    // Gun upgrades — stored here because SceneControl persists across scenes
    [HideInInspector] public bool hasTripleShot = false;
    [HideInInspector] public bool hasIceShot = false;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void LoadNextScene()
    {
        // If this is the last scene, trigger you win instead of crashing
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextIndex >= SceneManager.sceneCountInBuildSettings)
            StartCoroutine(YouWinSequence());
        else
            StartCoroutine(FadeAndLoad(nextIndex));
    }

    public void LoadGameOver()
    {
        StartCoroutine(GameOverSequence());
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator YouWinSequence()
    {
        yield return StartCoroutine(FadeIn());

        if (youWinPanel != null)
        {
            youWinPanel.SetActive(true);
            yield return new WaitForSeconds(3f);
            youWinPanel.SetActive(false);
        }
        else
        {
            yield return new WaitForSeconds(2f);
        }

        // Reset upgrades
        hasTripleShot = false;
        hasIceShot = false;

        SceneManager.LoadScene("MainMenu");
        yield return null;
        yield return StartCoroutine(FadeOut());
    }

    IEnumerator GameOverSequence()
    {
        yield return StartCoroutine(FadeIn());

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            yield return new WaitForSeconds(2f);
            gameOverPanel.SetActive(false);
        }
        else
        {
            yield return new WaitForSeconds(1f);
        }

        // Reset upgrades
        hasTripleShot = false;
        hasIceShot = false;

        SceneManager.LoadScene("MainMenu");
        yield return null;
        yield return StartCoroutine(FadeOut());
    }

    IEnumerator FadeAndLoad(int sceneIndex)
    {
        yield return StartCoroutine(FadeIn());
        SceneManager.LoadScene(sceneIndex);
        yield return null;
        yield return StartCoroutine(FadeOut());
    }

    IEnumerator FadeIn()
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
    }

    IEnumerator FadeOut()
    {
        float timer = 0f;
        Color color = fadeImage.color;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }
    }
}