// PlayerAttacker.cs
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    [Header("発射設定")]
    public float kunaiHorizontalOffset = 0.5f; // 発射オフセット
    public float throwSpeed = 15.0f;           // クナイの投てき速度 (Poolから借りた後に設定)

    void Start()
    {
        // Poolがシーンにあるか確認 (FactoryチェックをPoolチェックに変更)
        if (KunaiPool.Instance == null)
        {
            Debug.LogError("KunaiPoolがシーンに見つかりません。ゲームオブジェクトにアタッチし、プレハブを設定してください。");
        }
    }

    public void ThrowAttack()
    {
        if (KunaiPool.Instance == null) return;

        // プレイヤーの向きを取得
        float directionX = transform.localScale.x > 0 ? 1f : -1f;

        // 1. 発射位置と方向の計算 
        Vector3 spawnPosition = transform.position + new Vector3(directionX * kunaiHorizontalOffset, 0, 0);
        Vector2 throwDirection = new Vector2(directionX, 0);

        // 2. 【変更点】Poolからオブジェクトを借りる
        GameObject newKunai = KunaiPool.Instance.SpawnKunai(spawnPosition, Quaternion.identity);

        if (newKunai != null)
        {
            // 3. 借りたクナイに速度と方向を設定
            Rigidbody2D kunaiRb = newKunai.GetComponent<Rigidbody2D>();
            if (kunaiRb != null)
            {
                // 進行方向に合わせて回転させる (任意)
                newKunai.transform.right = throwDirection;

                // 速度を設定
                kunaiRb.velocity = throwDirection * throwSpeed;
            }
        }
    }
}