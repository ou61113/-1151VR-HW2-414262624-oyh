
using UnityEngine;

public class Finish : MonoBehaviour
{
    public GameUIController gameUI;

    private bool finished = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (finished) return;

        if (other.CompareTag("Player"))
        {
            finished = true;

            // 停止喬治移動
            PlayerController player =
                other.GetComponent<PlayerController>();

            if (player != null)
            {
                player.enabled = false;
            }

            // 停止物理運動
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
                rb.simulated = false;
            }

            // 顯示勝利圖片
            if (gameUI != null)
            {
                gameUI.ShowWin();
            }

            Debug.Log("You Win!");
        }
    }
}