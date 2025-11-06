// KunaiMovement.cs (最終版のOnCollisionEnter2D)
using UnityEngine;

// [RequireComponent(typeof(Rigidbody2D))] // 既にアタッチされている前提
public class KunaiMovement : MonoBehaviour
{
    // KunaiStatusの参照はAwakeなどで取得されている前提
    private KunaiStatus status;
    private float currentLifetime;

    void Awake()
    {
        // クナイのデータスクリプトを参照
        status = GetComponent<KunaiStatus>();
    }

    public void InitializeKunai()
    {
        if (status != null)
        {
            currentLifetime = status.lifetime;
        }
    }

    void Update()
    {
        // 寿命のカウントダウン処理（省略）
        if (currentLifetime > 0)
        {
            currentLifetime -= Time.deltaTime;
            if (currentLifetime <= 0)
            {
                ReturnToPool();
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        GameObject hitObject = other.gameObject;

        // 【BaseEnemyの取得】衝突相手からBaseEnemyコンポーネントを探す
        BaseEnemy baseEnemy = hitObject.GetComponent<BaseEnemy>();

        // プレイヤーのゲームオブジェクトに "Player" タグが設定されていることを前提とします。
        if (hitObject.CompareTag("Player"))
        {
            // プレイヤーに当たった場合は何もしない（ダメージも与えず、プールにも戻さない）
            // これにより、クナイはプレイヤーをすり抜けて飛び続けることが可能になります。
            // または、発射直後に衝突しないように、クナイのレイヤーを調整することもできます。
            return;
        }


        // 衝突相手が敵であるか確認 (BaseEnemyを継承している)
        if (baseEnemy != null)
        {
            // 敵へのダメージ処理
            if (status != null)
            {
                // BaseEnemyのTakeDamageを呼び出す
                // ダメージ値はKunaiStatusから取得
                baseEnemy.TakeDamage(status.damage);
            }

            // クナイは役割を果たしたのでプールに戻る
            ReturnToPool();
        }
        if (hitObject.CompareTag("Destructible"))
        {
            // 衝突したオブジェクトを削除
            // ?? 注意：オブジェクトプールを採用しているため、削除コストを抑えるには、
            //      このオブジェクト側にもプールへの返却処理を実装するのが理想です。
            Destroy(hitObject);
            ReturnToPool();

        }

        // それ以外のオブジェクト（壁、床など）に衝突した場合
        else
        {
            // 役目を終えたのでプールに戻る
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        // KunaiPool.Instance.ReleaseKunai(gameObject) を呼び出す処理（省略）
        if (KunaiPool.Instance != null)
        {
            KunaiPool.Instance.ReleaseKunai(gameObject);
        }
    }
}