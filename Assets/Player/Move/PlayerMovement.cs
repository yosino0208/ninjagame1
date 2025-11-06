using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))] 
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerStatus status;

    [Header("Jump Settings")]
    [SerializeField] private int maxJumps = 2; // 最大ジャンプ回数 (例: 2段ジャンプを許可)
    private int currentJumps = 0; // 現在のジャンプ回数

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        status = GetComponent<PlayerStatus>();

    }


    public void Move(float direction)
    {
        rb.velocity = new Vector2(direction * status.moveSpeed, rb.velocity.y);

        // Xスケールの絶対値 (左右の反転処理で使うため)
        float absX = Mathf.Abs(transform.localScale.x);
        // Yスケールの絶対値 (上下反転を防ぐため、常に正の値を使う)
        float absY = Mathf.Abs(transform.localScale.y);
        // 現在のZスケールを保持
        float currentZ = transform.localScale.z;

        if (direction > 0)
        {
            // 右向き: Xを絶対値 (正の値) にし、Yは絶対値 (正の値) を維持
            transform.localScale = new Vector3(absX, absY, currentZ);
        }
        else if (direction < 0)
        {
            // 左向き: Xをマイナス絶対値にし、Yは絶対値 (正の値) を維持
            transform.localScale = new Vector3(-absX, absY, currentZ);
        }


    }

    public void Jump()
    {
        // --- 【修正】ジャンプ制限のチェック ---
        if (currentJumps < maxJumps)
        {
            // Y軸方向の速度をリセットしてからジャンプ力を適用 (二段目以降のジャンプをスムーズにするため)
            rb.velocity = new Vector2(rb.velocity.x, 0f);

            rb.AddForce(Vector2.up * status.jumpPower, ForceMode2D.Impulse);

            currentJumps++; // ジャンプ回数をインクリメント
            Debug.Log($"Jump! Current Jumps: {currentJumps}");
        }
        // ----------------------------------------
    }

    // --- 【追加】地面に触れた時の処理 (ジャンプ回数リセット) ---
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 接触したオブジェクトが地面であると仮定し、ジャンプ回数をリセット
        // ※より厳密には、地面として設定したTagなどをチェックすべきです。
        // ここでは、下方向からの接触があればリセットする、とします。

        // 接触点の法線ベクトルを確認し、上方向への接触（つまり足元が地面）かを判断
        foreach (ContactPoint2D contact in collision.contacts)
        {
            // 接触面の法線がほぼ上向き (0, 1) であることを確認
            // Y座標が0.9f以上であれば地面とみなす (傾斜などを考慮し1.0fぴったりではなく)
            if (contact.normal.y > 0.9f)
            {
                currentJumps = 0; // ジャンプ回数をリセット
                Debug.Log("Landed! Jumps Reset.");
                return;
            }
        }
    }

}