// RangedAttack_Water.cs (最終版 - 弾丸連携ロジック含む)
using UnityEngine;

// 水鉄砲による遠距離攻撃
public class RangedAttack_Water : AttackComponent
{
  

    [Header("攻撃速度と範囲")]
    [SerializeField] private float fireRate = 2.0f;
    // 索敵範囲は EnemyData の detectionRange を使用

    private float nextFireTime;
    private int playerLayer;
    private Transform playerTarget;

    protected override void Awake()
    {
        base.Awake();
        // Unityのレイヤー名に合わせるため大文字の "PLAYER" を使用
        playerLayer = LayerMask.GetMask("PLAYER");
        if (playerLayer == 0)
        {
            Debug.LogError("UnityのLayer設定で 'PLAYER' レイヤーが見つかりません。");
        }
    }

    public override void Initialize(BaseEnemy enemy)
    {
        base.Initialize(enemy);
        // PlayerStatusを持つオブジェクトを検索してターゲットを取得
        PlayerStatus playerStatus = FindObjectOfType<PlayerStatus>();
        if (playerStatus != null)
        {
            playerTarget = playerStatus.transform;
        }
    }

    public override void Act()
    {
        if (playerTarget == null) return;

        // 1. 検出範囲チェック: EnemyDataのdetectionRangeを使用
        Collider2D playerCollider = Physics2D.OverlapCircle(
            owner.transform.position,
            owner.Data.detectionRange, // 索敵/射程範囲として使用
            playerLayer
        );

        if (playerCollider == null) return; // プレイヤーが範囲外
        if (Time.time < nextFireTime) return; // クールダウン中

        // 2. 発射
        FireBullet();
        nextFireTime = Time.time + fireRate;
    }

    private void FireBullet()
    {
        // ★ 変更点: データアセットからプレハブを取得する
        GameObject waterBulletPrefab = owner.Data.projectilePrefab;

        if (waterBulletPrefab == null)
        {
            // エラーメッセージもデータアセット向けに変更
            Debug.LogError("PetBottle DataアセットにWater Bullet Prefabが設定されていません。", this);
            return;
        }

        Vector3 startPos = owner.transform.position;

        // 1. 弾を生成
        GameObject bullet = Instantiate(waterBulletPrefab, startPos, Quaternion.identity);

        // ... (弾丸の初期化ロジックは変更なし) ...
        BulletScript bulletScript = bullet.GetComponent<BulletScript>();

        if (bulletScript != null)
        {
            Vector3 direction = (playerTarget.position - startPos).normalized;
            bulletScript.SetDirection(direction);
            bulletScript.SetDamage(owner.Data.attackDamage);
        }
        else
        {
            Debug.LogError("WaterBulletPrefabにBulletScriptがアタッチされていません。");
        }
    }
}