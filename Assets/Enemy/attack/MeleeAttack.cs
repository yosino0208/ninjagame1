using UnityEngine;

// 近接攻撃コンポーネント
public class MeleeAttack : AttackComponent
{
    private float attackCooldown = 1.0f; // 攻撃間隔（秒）
    private float nextAttackTime;
    private int playerLayer;

    float attackRange = 1.5f;

    protected override void Awake()
    {
        base.Awake();
        // Playerレイヤーのマスクを事前に取得
        playerLayer = LayerMask.GetMask("PLAYER");
    }

    public override void Act()
    {


        // 攻撃クールダウンが終了しているかチェック
        if (Time.time < nextAttackTime) return;



        // Playerレイヤーに属するオブジェクトを検出
        Collider2D playerCollider = Physics2D.OverlapCircle(
            owner.transform.position,
            attackRange,
            playerLayer
        );

        if (playerCollider != null)
        {
            // ★ 追加: プレイヤーを検出したことを確認
            Debug.Log("MeleeAttack: プレイヤーを検出しました！攻撃を試みます。");
            AttemptAttack(playerCollider.gameObject);
        }
        else
        {
            // ★ 追加: プレイヤーを検出していないことを確認
            Debug.Log("MeleeAttack: プレイヤー検出範囲外です。");
        }

        //OnDrawGizmos();
    }
    private void AttemptAttack(GameObject targetObject)
    {
        // ターゲットからPlayerStatusコンポーネントを取得し、存在すれば攻撃
        // ★ 修正: dynamicの使用を止め、ジェネリックな GetComponent<PlayerStatus>() を使用
        PlayerStatus playerStatus = targetObject.GetComponent<PlayerStatus>();

        Debug.Log($"MeleeAttack: PlayerStatus取得結果: {(playerStatus != null ? "成功" : "失敗")}");

        if (playerStatus != null)
        {
            // TakeDamageを直接呼び出す
            playerStatus.TakeDamage(owner.Data.attackDamage);

            Debug.Log($"{owner.Data.enemyName}がPlayerに{owner.Data.attackDamage}ダメージを与えた！");

            // クールダウンをリセット
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    // MeleeAttack.cs の Act() の最後に一時的に追加
    private void OnDrawGizmos()
    {
        // 攻撃範囲を赤色の円で描画
        if (owner != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(owner.transform.position, attackRange);
        }
    }
}