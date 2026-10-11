using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("移動設定")]
    public float moveSpeed = 5f;
    public float jumpForce = 8f;

    [Header("圖片設定")]
    public Sprite normalSprite;
    public Sprite deadSprite;

    [Header("死亡特效")]
    public GameObject sausagePrefab;
    public Vector2 launchForce = new Vector2(1.5f, 3f);
    public float restartDelay = 2f;

    [Header("地面判定")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.15f;
    public LayerMask groundLayer;
    [Header("UI")]
    public GameUIController gameUI;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Collider2D playerCollider;

    private bool isGrounded;
    private bool isDead;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<Collider2D>();

        if (normalSprite != null)
        {
            sr.sprite = normalSprite;
        }
    }

    void Update()
    {
        // 死亡後不能移動或跳躍
        if (isDead)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                RestartGame();
            }

            return;
        }

        // 判斷是否站在地面或平台上
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(
                groundCheck.position,
                groundCheckRadius,
                groundLayer
            );
        }

        // 左右移動
        float move = Input.GetAxisRaw("Horizontal");

        rb.linearVelocity = new Vector2(
            move * moveSpeed,
            rb.linearVelocity.y
        );
        
        // 跳躍
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                jumpForce
            );
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Die(collision.transform);
        }
    }

    void Die(Transform grinder)
    {
        if (isDead) return;

        isDead = true;

        // 停止喬治的物理運動
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;

        // 隱藏喬治的圖片與碰撞器
        sr.enabled = false;
        playerCollider.enabled = false;

        // 從絞肉機的右上方生成香腸
        Vector3 spawnPosition =
            grinder.position + new Vector3(0.5f, 0.4f, 0f);

        if (sausagePrefab != null)
        {
            GameObject sausage = Instantiate(
                sausagePrefab,
                spawnPosition,
                Quaternion.identity
            );

            Rigidbody2D sausageRb =
                sausage.GetComponent<Rigidbody2D>();

            if (sausageRb != null)
            {
                // 往右上方彈出
                sausageRb.AddForce(
                    launchForce,
                    ForceMode2D.Impulse
                );

                // 加入旋轉效果
                sausageRb.AddTorque(
                    -2f,
                    ForceMode2D.Impulse
                );
            }
            if (gameUI != null)
            {
                gameUI.ShowFail();
            }
            // 幾秒後移除香腸
            Destroy(sausage, 4f);
            
        }

        Debug.Log("George died!");

        // 延遲後自動重開
        Invoke(nameof(RestartGame), restartDelay);
    }

    void RestartGame()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}
