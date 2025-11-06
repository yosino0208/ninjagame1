using UnityEngine;

// MonoBehaviourを直接継承
public class FireProjectileEffect : MonoBehaviour
{
    // ★NinjutsuProjectileCoreから呼び出されるメソッド★
    public void HandleHitEffect(GameObject enemy, float damageAmount)
    {
        // 暫定処理: 火遁は持続エフェクトのため、ここでは投射物を消滅させない
        Debug.Log("Fire Hit: 持続エフェクトのため、子コンポーネントでは消滅させません。");

        // 【注意】炎上（Damage over Time: DoT）などの処理を追加する際、ここに追加します。
    }
}