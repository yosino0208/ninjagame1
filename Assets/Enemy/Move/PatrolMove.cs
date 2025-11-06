using UnityEngine;

// 左右に巡回する動き
public class PatrolMove : MoveComponent
{
    // コンポーネント固有の設定値
    [Header("巡回範囲")]
    [Tooltip("左右に移動する振幅（中心位置から片側への最大距離）")]

    private float patrolRange = 5.0f; // ★ [SerializeField] を追加し、Inspectorから設定可能に

    private Vector3 initialPosition;

    public override void Initialize(BaseEnemy enemy)
    {
        base.Initialize(enemy);
        initialPosition = owner.transform.position;
    }

    public override void Act()
    {
        // シンプルな左右往復移動のロジック

        // Sin波の振幅に patrolRange を使用
        float moveX = Mathf.Sin(Time.time * owner.Data.moveSpeed * 0.5f) * patrolRange;

        Vector3 newPos = initialPosition + new Vector3(moveX, 0, 0);

        owner.transform.position = Vector3.MoveTowards(
            owner.transform.position,
            newPos,
            owner.Data.moveSpeed * Time.deltaTime
        );
        //OnDrawGizmos();
    }
    // PatrolMove.cs に追加するコード

    private void OnDrawGizmos()
    {
        // 敵オブジェクトが存在し、Inspectorが有効な場合のみ描画
        if (owner == null) return;

        // 敵が現在シーンに配置されている位置を初期位置と仮定
        // ただし、Initializeが呼ばれていない場合 (Editor上) は現在の位置を使用
        Vector3 currentPosition = owner.transform.position;

        // 描画の中心位置を計算 (現在の位置を基準とする。初期位置 initialPosition は Editor 実行前には使えないため)
        Vector3 center = currentPosition;

        // 左右への最大移動位置を計算
        Vector3 leftLimit = center - new Vector3(patrolRange, 0, 0);
        Vector3 rightLimit = center + new Vector3(patrolRange, 0, 0);

        // 1. 巡回範囲を示す線を描画 (青)
        // 巡回範囲全体を線で結ぶ
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(leftLimit, rightLimit);

        // 2. 巡回の中心位置を示す球体を描画 (黄色)
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(center, 0.15f); // 半径0.15fの球

        // 3. 左右の端点を示す球体を描画 (水色)
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(leftLimit, 0.1f);
        Gizmos.DrawSphere(rightLimit, 0.1f);
    }
}
