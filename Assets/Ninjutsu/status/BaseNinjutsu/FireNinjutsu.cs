// FireNinjutsu.cs

using UnityEngine;

[CreateAssetMenu(fileName = "FireNinjutsu", menuName = "Ninjutsu/Fire Ninjutsu")]
public class FireNinjutsu : BaseNinjutsu
{
    public override void Activate(Vector3 originPosition, Vector3 direction, Transform caster)
    {
        if (projectilePrefab == null)
        {
            Debug.LogError($"[Ninjutsu] {ninjutsuName} の projectilePrefab が設定されていません！");
            return;
        }

        Vector3 launchPosition = originPosition + direction.normalized * 0.7f;
        int playerAttackLayer = LayerMask.NameToLayer("PLAYER_ATTACK");

        GameObject flame = Instantiate(projectilePrefab, launchPosition, Quaternion.identity);

        // 生成された炎のレイヤーを設定
        if (playerAttackLayer != -1)
        {
            flame.layer = playerAttackLayer;
        }

        // プレイヤーの向きに合わせて回転
        if (direction.x < 0)
        {
            flame.transform.localScale = new Vector3(-1 * Mathf.Abs(flame.transform.localScale.x), flame.transform.localScale.y, flame.transform.localScale.z);
        }

        // エフェクトを一定時間後に消滅させる
        Destroy(flame, duration);
    }
}