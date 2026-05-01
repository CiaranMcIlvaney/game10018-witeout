using System.Collections;
using UnityEngine;
using TMPro;

public class CheckpointUI : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI checkpointText;

    [Header("Timing")]
    public float visibleTime = 1.5f;
    public float fadeTime = 1f;

    private Coroutine currentRoutine;

    void Start()
    {
        if (checkpointText != null)
        {
            checkpointText.gameObject.SetActive(false);
        }
    }

    public void ShowCheckpointMessage()
    {
        if (checkpointText == null)
            return;

        if (currentRoutine != null)
        {
            StopCoroutine(currentRoutine);
        }

        currentRoutine = StartCoroutine(ShowRoutine());
    }

    IEnumerator ShowRoutine()
    {
        checkpointText.gameObject.SetActive(true);
        checkpointText.text = "CHECKPOINT ACTIVATED";

        Color color = checkpointText.color;
        color.a = 1f;
        checkpointText.color = color;

        yield return new WaitForSeconds(visibleTime);

        float timer = 0f;

        while (timer < fadeTime)
        {
            timer += Time.deltaTime;
            float t = timer / fadeTime;

            color.a = Mathf.Lerp(1f, 0f, t);
            checkpointText.color = color;

            yield return null;
        }

        color.a = 0f;
        checkpointText.color = color;
        checkpointText.gameObject.SetActive(false);
    }
}
