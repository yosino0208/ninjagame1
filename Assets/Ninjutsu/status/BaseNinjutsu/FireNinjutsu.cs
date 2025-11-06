// FireNinjutsu.cs

using UnityEngine;

[CreateAssetMenu(fileName = "FireNinjutsu", menuName = "Ninjutsu/Fire Ninjutsu")]
public class FireNinjutsu : BaseNinjutsu
{
    public override void Activate(Vector3 originPosition, Vector3 direction, Transform caster)
    {
        // ... 既存の個体差計算ロジック（ダメージ、持続時間など） ...

        // ------------------------------------------------
        // 1. 【スピードの個体差計算】
        // ------------------------------------------------

        // 速度のランダムな補正値を計算 (例: -3.0f から +3.0f の間)
        float randomSpeedDelta = Random.Range(-speedVariance, speedVariance);

        // 最終的な速度を決定 (基本速度 + 補正値)
        float finalSpeed = launchSpeed + randomSpeedDelta;

        // 速度がゼロやマイナスにならないようにクランプ（念のため）
        if (finalSpeed < 1.0f) finalSpeed = 1.0f;

        // Debug.Log($"[Ninjutsu Speed] Base Speed: {launchSpeed} -> Final Speed: {finalSpeed:F2}");


        // ------------------------------------------------
        // 2. 忍術オブジェクトの生成と移動
        // ------------------------------------------------

        if (projectilePrefab == null)
        {
            Debug.LogError($"[Ninjutsu] {ninjutsuName} の projectilePrefab が設定されていません！");
            return;
        }

        Vector3 launchPosition = originPosition + direction.normalized * 0.7f;
        int playerAttackLayer = LayerMask.NameToLayer("PLAYER_ATTACK");

        GameObject flame = Instantiate(projectilePrefab, launchPosition, Quaternion.identity);
        Rigidbody2D rb = flame.GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError($"[Ninjutsu] {ninjutsuName} の projectilePrefab に Rigidbody2D がありません。移動できません！");
            // 飛ばせない場合はDurationで消滅させるための既存ロジックを再利用（ここでは省略）
        }
        else
        {
            // 重力の影響を無効にする
            rb.gravityScale = 0f;

            // 個体差が適用された finalSpeed を使って速度を設定
            Vector2 moveDirection = direction.normalized;
            rb.velocity = moveDirection * finalSpeed;
        }


        // 3. その他設定（レイヤー、回転、消滅時間）...

        // (レイヤー設定)
        if (playerAttackLayer != -1)
        {
            flame.layer = playerAttackLayer;
        }

        // (回転設定)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        flame.transform.rotation = Quaternion.Euler(0, 0, angle);

        // (持続時間。これも個体差が適用されていると仮定)
        float finalDuration = 2.0f; // ここでdurationの個体差計算を行うのが望ましい
        Destroy(flame, finalDuration);
    }
}