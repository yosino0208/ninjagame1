// TripleWindNinjutsu.cs (修正版 - Orbiterスクリプトを利用)

using UnityEngine;

[CreateAssetMenu(fileName = "TripleWindNinjutsu", menuName = "Ninjutsu/Wind/Triple Wind Combo")]
public class TripleWindNinjutsu : CompositeNinjutsuData
{
    [Header("軌道・発射パラメータ")]
    public float orbitalRadius = 2.0f;          // プレイヤー周りの回転半径
    public float orbitalSpeed = 360.0f;         // 軌道回転の速度 (度/秒)
    public float launchDelay = 3.0f;            // 軌道回転から発射までの遅延時間 (秒)

    [Header("複数投射物設定")]
    public int projectileCount = 3;
    public float initialAngleOffset = 0f;

    public override void Activate(Vector3 originPosition, Vector3 direction, Transform caster)
    {
        Debug.Log($"【複合技発動】: {ninjutsuName} - 風の球 {projectileCount} 個がプレイヤーの周りを回転し、自動発射されます！");

        if (projectilePrefab == null)
        {
            Debug.LogError($"[Ninjutsu] {ninjutsuName} の projectilePrefab が設定されていません！");
            return;
        }

        int playerAttackLayer = LayerMask.NameToLayer("PLAYER_ATTACK");
        float angleStep = 360f / projectileCount;

        for (int i = 0; i < projectileCount; i++)
        {
            // 1. 初期位置と角度の計算
            float initialAngle = initialAngleOffset + i * angleStep;
            float rad = initialAngle * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * orbitalRadius;
            Vector3 spawnPosition = caster.position + offset;

            // 2. 投射物を生成
            GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);

            // レイヤーを設定
            if (playerAttackLayer != -1)
            {
                projectile.layer = playerAttackLayer;
            }

            // 3. 【重要】NinjutsuProjectileCore にダメージ、属性、持続時間を渡す
            NinjutsuProjectileCore core = projectile.GetComponent<NinjutsuProjectileCore>();
            if (core != null)
            {
                core.damageAmount = baseDamage;
                core.ninjutsuType = type;
                core.duration = duration;
            }

            // 4. 【重要】Orbiterスクリプトをアタッチし、軌道ロジックを初期化
            WindProjectileOrbiter orbiter = projectile.AddComponent<WindProjectileOrbiter>();
            orbiter.Initialize(
                caster,                 // プレイヤーのTransform
                orbitalRadius,          // 回転半径
                orbitalSpeed,           // 回転速度
                launchDelay,            // 発射遅延
                initialAngle,           // 初期角度
                speed                   // 発射速度 (CompositeNinjutsuDataのspeed)
            );
        }
    }
}