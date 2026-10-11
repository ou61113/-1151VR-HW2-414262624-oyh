
using UnityEngine;

public class GameAudioController : MonoBehaviour
{
    [Header("背景馬達聲")]
    public AudioSource motorSource;
    public AudioClip motorClip;

    [Header("結局音效")]
    public AudioClip failSound;
    public AudioClip winBGM;

    private AudioSource effectSource;
    private bool gameEnded = false;

    void Awake()
    {
        // 建立獨立的音效播放來源
        effectSource = gameObject.AddComponent<AudioSource>();
        effectSource.playOnAwake = false;
    }

    void Start()
    {
        // 遊戲開始時播放循環馬達聲
        if (motorSource != null && motorClip != null)
        {
            motorSource.clip = motorClip;
            motorSource.loop = true;
            motorSource.Play();
        }
    }

    public void PlayFail()
    {
        if (gameEnded) return;
        gameEnded = true;

        // 停止馬達聲
        StopMotor();

        // 播放一次失敗音效
        if (failSound != null)
        {
            effectSource.PlayOneShot(failSound);
        }
    }

    public void PlayWin()
    {
        if (gameEnded) return;
        gameEnded = true;

        // 停止馬達聲
        StopMotor();

        // 播放勝利背景音樂
        if (winBGM != null)
        {
            effectSource.clip = winBGM;
            effectSource.loop = true;
            effectSource.Play();
        }
    }

    void StopMotor()
    {
        if (motorSource != null && motorSource.isPlaying)
        {
            motorSource.Stop();
        }
    }
}