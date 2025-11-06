using UnityEngine;

// MonoBehaviourを直接継承
public class WindProjectileEffect : MonoBehaviour
{
    // ★NinjutsuProjectileCoreから呼び出されるメソッド★
    public void HandleHitEffect(GameObject enemy, float damageAmount)
    {
        // 暫定処理: 風弾は当たったら消滅
        Debug.Log("Wind Hit: 投射物消滅を子コンポーネントで処理。");

        // 【注意】風遁固有のデバフや処理を追加する際、ここに追加します。

        Destroy(gameObject);
    }
}