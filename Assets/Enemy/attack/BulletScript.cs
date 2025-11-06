// BulletScript.cs
using UnityEngine;

// 敵が発射する弾丸のロジック
// WaterBullet Prefabにアタッチして使用
public class BulletScript : MonoBehaviour
{
    [Header("弾丸の設定")]
    [SerializeField] private float speed = 10f; // 弾速
    [SerializeField] private float lifeTime = 3f; // 寿命（画面外などで）

    private Vector3 moveDirection;
    private int damageAmount =1; // RangedAttack_Waterから渡されるダメージ量

    // --- 初期化 ---

    // RangedAttack_Waterから呼ばれ、移動方向を設定する
    public void SetDirection(Vector3 direction)
    {
        moveDirection = direction.normalized;
        // 弾丸の向きを進行方向に合わせる（任意）
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    // RangedAttack_Waterから呼ばれ、ダメージ量を設定する
    public void SetDamage(int newDamage)
    {
        damageAmount = newDamage;
    }

    // --- ライフサイクル ---

    private void Start()
    {
        // 寿命タイマーを開始
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        // 弾丸を前進させる
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
    }

    // --- 衝突判定 ---

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 衝突したオブジェクトからPlayerStatusを取得
        if (other.TryGetComponent<PlayerStatus>(out PlayerStatus playerStatus))
        {
            // PlayerStatusを持っているオブジェクト（プレイヤー）にダメージを与える
            playerStatus.TakeDamage(damageAmount);
            Debug.Log($"水鉄砲がPlayerに命中！{damageAmount}ダメージを与えた。");

            // 弾丸を破壊
            Destroy(gameObject);
        }


    }
}