// WaterNinjutsu.cs (修正)

using UnityEngine;

[CreateAssetMenu(fileName = "WaterNinjutsu", menuName = "Ninjutsu/Water Ninjutsu")]
public class WaterNinjutsu : BaseNinjutsu
{
    [Header("水遁パラメータ")]
    public int maxBounces = 3; // ★追加★ 最大跳ね返り回数

    public override void Activate(Vector3 originPosition, Vector3 direction, Transform caster)
    {
        Debug.Log("Water Ninjutsu Activated: Shot a water bullet!");

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

            // 【★重要★】投射物コアに跳ね返り回数を設定
            WaterProjectileBounce bounceScript = waterBullet.GetComponent<WaterProjectileBounce>();
            if (bounceScript != null)
            {
                bounceScript.InitializeBounce(maxBounces);
            }
        }
    }
}