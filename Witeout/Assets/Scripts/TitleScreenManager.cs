using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour
{
    [Header("Fade")]
    public Image fadeImage;
    public float fadeDuration = 1.5f;

    [Header("Menu")]
    public GameObject menuGroup;

    [Header("Scene")]
    public string gameSceneName = "Witeout";

    private bool isTransitioning = false;

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        Color color = fadeImage.color;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / fadeDuration;

            color.a = Mathf.Lerp(1f, 0f, t);
            fadeImage.color = color;

            yield return null;
        }

        color.a = 0f;
        fadeImage.color = color;
    }

    public void PlayGame()
    {
        if (isTransitioning) return;
        StartCoroutine(FadeOutAndLoad());
    }

    IEnumerator FadeOutAndLoad()
    {
        isTransitioning = true;

        if (menuGroup != null)
        {
            menuGroup.SetActive(false);
        }

        Color color = fadeImage.color;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / fadeDuration;

            color.a = Mathf.Lerp(0f, 1f, t);
            fadeImage.color = color;

            yield return null;
        }

        color.a = 1f;
        fadeImage.color = color;

        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");

        Application.Quit();
    }
}