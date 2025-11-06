// WaterNinjutsu.cs (修正：跳ね返り回数に個体差を導入)

using UnityEngine;

[CreateAssetMenu(fileName = "WaterNinjutsu", menuName = "Ninjutsu/Water Ninjutsu")]
public class WaterNinjutsu : BaseNinjutsu
{
    [Header("水遁パラメータ")]
    public int maxBounces = 3;       // 基本の最大跳ね返り回数
    public int bounceVariance = 1;   // ★追加★ 跳ね返り回数のランダムな振れ幅 (例: ±1回)

    public override void Activate(Vector3 originPosition, Vector3 direction, Transform caster)
    {
        Debug.Log("Water Ninjutsu Activated: Shot a water bullet with variance!");

        // ------------------------------------------------
        // 1. 【個体差の計算】: 跳ね返り回数の補正をランダムに決定
        // ------------------------------------------------

        // ランダムな補正値を計算 (例: -1 から +1 の間の整数)
        int randomBounceDelta = Random.Range(-bounceVariance, bounceVariance + 1); // +1で最大値を含む

        // 最終的な跳ね返り回数を決定
        int finalBounces = maxBounces + randomBounceDelta;

        // 0未満にならないようにクランプ
        if (finalBounces < 0) finalBounces = 0;

        Debug.Log($"跳ね返り回数 個体差適用: Base={maxBounces} -> Final={finalBounces}回");


        // ------------------------------------------------
        // 2. 忍術オブジェクトの生成と設定
        // ------------------------------------------------

        // レイヤーを取得
        int playerAttackLayer = LayerMask.NameToLayer("PLAYER_ATTACK");

        // BaseNinjutsuのLaunchProjectileメソッドを使用して投射物を発射
        GameObject waterBullet = LaunchProjectile(originPosition, direction);

        if (waterBullet != null)
        {
            // 生成された水弾のレイヤーを設定
            if (playerAttackLayer != -1)
            {
                waterBullet.layer = playerAttackLayer;
            }

            // 【★重要★】投射物コアに個体差が適用された回数を設定
            WaterProjectileBounce bounceScript = waterBullet.GetComponent<WaterProjectileBounce>();
            if (bounceScript != null)
            {
                // 個体差が適用された finalBounces を渡す
                bounceScript.InitializeBounce(finalBounces);
            }
        }

        // ※ LaunchProjectile()内で、飛ぶスピードの個体差計算と設定も行う必要があります。
    }
}