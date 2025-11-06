using UnityEngine;

// ゆっくりと浮遊する動き
public class FloatMove : MoveComponent
{
    // コンポーネント固有の設定値は、ここで [SerializeField] で保持
    [SerializeField] private float floatHeight = 0.5f; // 浮遊する高さの振幅
    [SerializeField] private float floatSpeed = 1.0f;  // 浮遊する速さ

    private Vector3 initialPosition;
    private Rigidbody2D rb;

    public override void Initialize(BaseEnemy enemy)
    {
        base.Initialize(enemy);
        initialPosition = owner.transform.position;

        // BaseEnemyがアタッチされているGameObjectからRigidbody2Dを取得
        rb = owner.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            // 飛行中は重力を無効化し、現在の速度をリセット
            rb.gravityScale = 0f;
            rb.velocity = Vector2.zero;
        }
    }

    public override void Act()
    {
        // ターゲットに依存しない、基本の浮遊アニメーション

        // 垂直方向の浮遊アニメーション (サイン波を利用)
        float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        Vector3 targetPos = initialPosition + new Vector3(0, yOffset, 0);

        // 基本の移動速度（owner.Data.moveSpeed）を使って浮遊位置に移動
        owner.transform.position = Vector3.Lerp(
            owner.transform.position,
            targetPos,
            Time.deltaTime * owner.Data.moveSpeed * 0.5f
        );
    }
}