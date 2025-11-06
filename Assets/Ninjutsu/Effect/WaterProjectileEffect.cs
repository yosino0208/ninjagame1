using UnityEngine;

// MonoBehaviourを直接継承
public class WaterProjectileEffect : MonoBehaviour
{
    // ★NinjutsuProjectileCoreから呼び出されるメソッド★
    // ノックバックなどのエフェクトは、ここに追加します。
    public void HandleHitEffect(GameObject enemy, float damageAmount)
    {
        // 暫定処理: 水弾は当たったら消滅
        Debug.Log("Water Hit: 投射物消滅を子コンポーネントで処理。");

        // 【注意】ノックバック処理を追加する際、ここで Rigidbody2D を使って処理します。

        Destroy(gameObject);
    }
}