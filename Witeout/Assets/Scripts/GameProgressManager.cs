using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameProgressManager : MonoBehaviour
{
    public static GameProgressManager Instance;

    [Header("Generator Goal")]
    public int generatorsRequiredToWin = 4;
    public int generatorsRepaired = 0;

    [Header("Fade")]
    public Image fadeImage;
    public float fadeDuration = 2f;

    [Header("Scene To Load")]
    public string sceneToLoad = "TitleScreen";

    private bool endingTriggered = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        if (fadeImage != null)
        {
            Color color = fadeImage.color;
            color.a = 0f;
            fadeImage.color = color;
        }
    }

    public void RegisterGeneratorRepair()
    {
        if (endingTriggered) return;

        generatorsRepaired++;
        Debug.Log("Generators repaired: " + generatorsRepaired + " / " + generatorsRequiredToWin);

        if (generatorsRepaired >= generatorsRequiredToWin)
        {
            StartCoroutine(FadeOutAndLoadScene());
        }
    }

    IEnumerator FadeOutAndLoadScene()
    {
        endingTriggered = true;

        if (fadeImage != null)
        {
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
        }

        SceneManager.LoadScene(sceneToLoad);
    }
}