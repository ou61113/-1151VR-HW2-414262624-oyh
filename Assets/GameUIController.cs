
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameUIController : MonoBehaviour
{
    [Header("失敗圖片")]
    public CanvasGroup failGroup;

    [Header("勝利圖片")]
    public CanvasGroup winGroup;
    [Header("音效控制器")]
    public GameAudioController gameAudio;
    [Header("動畫設定")]
    public float fadeDuration = 1.0f;
    public float popDuration = 0.35f;
    public float winScale = 1.2f;

    private bool gameEnded = false;

    void Start()
    {
        // 遊戲開始時，兩張圖片都隱藏
        if (failGroup != null)
        {
            failGroup.alpha = 0f;
            failGroup.gameObject.SetActive(true);
            failGroup.interactable = false;
            failGroup.blocksRaycasts = false;
        }

        if (winGroup != null)
        {
            winGroup.alpha = 0f;
            winGroup.gameObject.SetActive(true);
            winGroup.interactable = false;
            winGroup.blocksRaycasts = false;

            winGroup.transform.localScale = Vector3.zero;
        }
    }

    public void ShowFail()
    {
        if (gameEnded) return;

        gameEnded = true;
        if (gameAudio != null)
        {
            gameAudio.PlayFail();
        }
        StartCoroutine(FadeInFail());
    }

    public void ShowWin()
    {
        if (gameEnded) return;

        gameEnded = true;
        if (gameAudio != null)
        {
            gameAudio.PlayWin();
        }
        StartCoroutine(PopWin());
    }

    IEnumerator FadeInFail()
    {
        float time = 0f;
        failGroup.alpha = 0f;

        while (time < fadeDuration)
        {
            time += Time.unscaledDeltaTime;
            failGroup.alpha = Mathf.Clamp01(time / fadeDuration);
            yield return null;
        }

        failGroup.alpha = 1f;
    }

    IEnumerator PopWin()
    {
        float time = 0f;
        winGroup.alpha = 1f;
        winGroup.transform.localScale = Vector3.zero;

        while (time < popDuration)
        {
            time += Time.unscaledDeltaTime;

            float t = Mathf.Clamp01(time / popDuration);

            // 先放大，再逐漸回到正常大小
            float eased = 1f - Mathf.Pow(1f - t, 3f);
            float scale = Mathf.Lerp(0f, 1f, eased);

            winGroup.transform.localScale =
                Vector3.one * scale * winScale;

            yield return null;
        }

        winGroup.transform.localScale = Vector3.one;
    }

    void Update()
    {
        // 失敗後按 R 重新載入目前的場景
        if (gameEnded && failGroup != null &&
            failGroup.alpha >= 1f &&
            Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(
                SceneManager.GetActiveScene().buildIndex
            );
        }
    }
}
