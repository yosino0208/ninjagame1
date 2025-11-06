// WindNinjutsu.cs

using UnityEngine;

[CreateAssetMenu(fileName = "WindNinjutsu", menuName = "Ninjutsu/Wind Ninjutsu")]
public class WindNinjutsu : BaseNinjutsu
{
    public override void Activate(Vector3 originPosition, Vector3 direction, Transform caster)
    {
        Debug.Log("Wind Ninjutsu Activated: 葉っぱを高速で3枚ちだす。");

        Vector3 launchPosition = originPosition + direction.normalized * 0.5f + Vector3.up * 0.5f;
        int playerAttackLayer = LayerMask.NameToLayer("PLAYER_ATTACK");
        const float VERTICAL_OFFSET = 0.3f;

        for (int i = 0; i < 3; i++)
        {
            Vector3 offset = new Vector3(0, (i - 1) * VERTICAL_OFFSET, 0);

            // BaseNinjutsuの共通機能を使って投射物を発射
            GameObject projectile = LaunchProjectile(launchPosition + offset, direction);

            // 生成された葉っぱのレイヤーを設定
            if (projectile != null && playerAttackLayer != -1)
            {
                projectile.layer = playerAttackLayer;
            }
        }
    }
}