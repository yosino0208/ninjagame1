// FollowMove.cs (追尾開始・停止ロジック組み込み済み)
using UnityEngine;

// プレイヤーを追跡する動き
public class FollowMove : MoveComponent
{
    // コンポーネント固有の設定値
    [Header("追尾と停止")]
    [Tooltip("追尾を停止する最小距離（緑の円）")]
    [SerializeField] private float stopDistance = 1.5f; // 追尾を停止する距離

    private Transform playerTarget;
    // 追尾開始距離 (黄色い円) は owner.Data.detectionRange で参照します

    public override void Initialize(BaseEnemy enemy)
    {
        base.Initialize(enemy);

        // ターゲットの取得ロジック（変更なし）
        PlayerStatus playerStatus = FindObjectOfType<PlayerStatus>();
        if (playerStatus != null)
        {
            playerTarget = playerStatus.transform;
            Debug.Log($"{owner.Data.enemyName} のターゲット (Player) を取得しました。");
        }
        else
        {
            Debug.LogWarning("PlayerStatusを持つオブジェクトが見つかりません。FollowMoveは機能しません。");
            this.enabled = false;
        }
    }

    // 毎フレームの動きを定義
    public override void Act()
    {
        if (playerTarget == null) return;

        float distance = Vector3.Distance(owner.transform.position, playerTarget.position);

        // --- 追尾実行の判定 ---

        // 1. 追尾開始判定 (Detection Range / 黄色の円)
        // 検出範囲外の場合、移動を停止し、何もせずに Act() を終了する
        if (distance > owner.Data.detectionRange)
        {
            return;
        }

        // 2. 停止判定 (Stop Distance / 緑の円)
        // 停止距離まで近づいたら移動を停止し、何もせずに Act() を終了する
        if (distance <= stopDistance)
        {
            return;
        }

        // --- 移動実行 (検出範囲内で、かつ停止距離外の場合のみ移動) ---

        Vector3 targetPosition = playerTarget.position;

        owner.transform.position = Vector3.MoveTowards(
            owner.transform.position,
            targetPosition,
            owner.Data.moveSpeed * Time.deltaTime
        );

        OnDrawGizmos();

        // ... (スプライト反転ロジックなど)
    }

    // 描画関数 (変更なし)
    private void OnDrawGizmos()
    {
        if (owner == null) return;

        Vector3 center = owner.transform.position;

        // 緑色の円 (停止距離)
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(center, stopDistance);

        // 黄色の円 (追尾開始距離/Detection Range)
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(center, owner.Data.detectionRange);
    }
}