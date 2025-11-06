using UnityEngine;

// 巨大火球と分裂後の小火球の両方にアタッチすることを想定
public class ProjectileSplitController : MonoBehaviour
{
    private GameObject _splitPrefab;
    private int _count;
    private float _baseSpeed;     // 分裂後の投射物が持つ固定の初速
    private float _lifetime = 0.5f;

    // TripleFireNinjutsuからデータを受け取るメソッド
    public void SetSplitData(GameObject prefab, int count, float baseSpeed, float lifetime)
    {
        _splitPrefab = prefab;
        _count = count;
        _baseSpeed = baseSpeed;
        _lifetime = lifetime;

        // 寿命タイマーを開始
        Destroy(gameObject, lifetime);
    }

    // 投射物が寿命で破壊される直前、または衝突時に実行される
    private void OnDestroy()
    {
        // シーンのロード/アンロードによる破壊は無視
        if (!gameObject.scene.isLoaded) return;

        // 分裂処理が定義されているか、かつ分裂数があるかチェック
        if (_splitPrefab == null || _count <= 0) return;

        PerformSplit();
    }

    private void PerformSplit()
    {
        float angleStep = 360f / _count;

        // 親火球の現在のRigidbodyとProjectileCoreを取得
        Rigidbody2D parentRb2d = GetComponent<Rigidbody2D>();
        Vector2 parentVelocity = (parentRb2d != null) ? parentRb2d.velocity : Vector2.zero;

        // 【★修正点★】親火球のコアからダメージとタイプを読み取る
        NinjutsuProjectileCore parentCore = GetComponent<NinjutsuProjectileCore>();
        float parentDamage = (parentCore != null) ? parentCore.damageAmount : 0f;
        NinjutsuElement parentType = (parentCore != null) ? parentCore.ninjutsuType : NinjutsuElement.None;

        // 分裂後のダメージを計算 (例: 親の50%に減衰)
        float splitDamage = parentDamage * 0.5f;

        for (int i = 0; i < _count; i++)
        {
            // 1. 発射角度を計算 (円周上に均等に)
            float angle = i * angleStep;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Vector3 direction = rotation * Vector3.right; // 分裂方向ベクトル

            // 2. 新しい投射物（分裂後の小火球）を生成
            GameObject newProjectile = Instantiate(_splitPrefab, transform.position, rotation);

            // 3. 速度を設定
            Rigidbody2D rb2d = newProjectile.GetComponent<Rigidbody2D>();
            if (rb2d != null)
            {
                // 分裂のエネルギー (全方位への_baseSpeed) に、親火球の進行方向の速度を加算
                Vector2 splitVelocity = (Vector2)direction.normalized * _baseSpeed;
                rb2d.velocity = parentVelocity + splitVelocity;
            }

            // 【★修正点★】子火球のNinjutsuProjectileCoreにダメージとタイプを設定
            NinjutsuProjectileCore childCore = newProjectile.GetComponent<NinjutsuProjectileCore>();
            if (childCore != null)
            {
                childCore.damageAmount = splitDamage;
                childCore.ninjutsuType = parentType;
            }

            // 4. 【1→3→5の連鎖】次の分裂段階のデータを設定
            if (_count == 3)
            {
                // 3個に分裂した後、次に5個に分裂するように設定
                ProjectileSplitController nextSplit = newProjectile.GetComponent<ProjectileSplitController>();
                if (nextSplit != null)
                {
                    // 次の分裂は速度をさらに落とし、寿命を短くして再設定
                    nextSplit.SetSplitData(_splitPrefab, 5, _baseSpeed * 0.8f, _lifetime * 0.5f);
                }
            }
            // 5個に分裂した後は処理をしないため、分裂はここで終了する
        }
    }

    // 衝突で分裂させたい場合は、OnCollisionEnter2Dなどを実装してください。（NinjutsuProjectileCoreがHandleHitでDestroyを呼んでいるため、OnDestroyで分裂処理が走ります）
}