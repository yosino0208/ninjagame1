using UnityEngine;

// プレイヤーに近づいた後、自爆する攻撃
public class SuicideAttack : AttackComponent
{
    // コンポーネント固有の設定値
    [SerializeField] private float triggerRange = 2.0f;    // 自爆のカウントダウンを開始する距離
    [SerializeField] private float explosionRange = 3.0f;  // 爆発のダメージ範囲
    [SerializeField] private float countdownTime = 1.5f;   // 自爆までの時間

    private float explosionTimer = 0f;
    private bool isCountingDown = false;
    private int playerLayer;
    private Transform playerTarget;

    protected override void Awake()
    {
        base.Awake();
        playerLayer = LayerMask.GetMask("Player");
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

        // 1. カウントダウンの開始判定
        if (!isCountingDown)
        {
            // プレイヤーとの距離をチェック
            if (Vector3.Distance(owner.transform.position, playerTarget.position) <= triggerRange)
            {
                isCountingDown = true;
                explosionTimer = countdownTime;
                Debug.Log($"{owner.Data.enemyName}: 自爆カウントダウン開始！残り {countdownTime}秒");
                // 【TODO】警告のエフェクトやSEを再生
            }
        }

        // 2. カウントダウンの進行
        if (isCountingDown)
        {
            explosionTimer -= Time.deltaTime;

            if (explosionTimer <= 0)
            {
                Explode();
            }
        }
    }

    private void Explode()
    {
        // 爆発のエフェクトを生成（例: Instantiate(explosionPrefab, ...)）

        // 爆発範囲内のPlayerを検出
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(
            owner.transform.position,
            explosionRange,
            playerLayer
        );

        foreach (var hit in hitObjects)
        {
            // PlayerStatusを持っているかチェック
            if (hit.TryGetComponent<PlayerStatus>(out PlayerStatus playerStatus))
            {
                // BaseEnemyDataのattackDamageを使ってダメージ処理
                playerStatus.TakeDamage(owner.Data.attackDamage);
            }
        }

        Debug.Log($"{owner.Data.enemyName} が自爆し、{hitObjects.Length}個のオブジェクトにダメージを与えた。");

        // 最後に自身のオブジェクトを破壊する（BaseEnemyの死亡処理を呼び出す）
        // 現在のHP以上のダメージを与えて確実にDie()を呼ぶ
        owner.TakeDamage(owner.currentHP + 1);
    }
}