// TripleFireNinjutsu.cs

using UnityEngine;

[CreateAssetMenu(fileName = "TripleFireNinjutsu", menuName = "Ninjutsu/Fire/Triple Fire Combo")]
public class TripleFireNinjutsu : CompositeNinjutsuData
{
    // 複合技特有のデータフィールドを追加

    [Header("炎の分裂パラメータ")]
    public GameObject splitProjectilePrefab; // 分裂後に生成する小さな火球のプレハブ
    public int initialSplitCount = 3;        // 巨大火球が最初に分裂する数 (例: 3)

    public float splitBaseSpeed = 8.0f; // 分裂後の火球が持つ基本速度

    public override void Activate(Vector3 originPosition, Vector3 direction, Transform caster)
    {
        Debug.Log($"【複合技発動】: {ninjutsuName} - 巨大火球を発射、{initialSplitCount}個に分裂する！");

        if (projectilePrefab != null)
        {
            int playerAttackLayer = LayerMask.NameToLayer("PLAYER_ATTACK");

            // 1. プレイヤーの少し前方に火球を生成
            Vector3 launchPosition = originPosition + direction.normalized * launchOffset;
            GameObject fireBall = GameObject.Instantiate(projectilePrefab, launchPosition, Quaternion.identity);

            // レイヤー設定
            if (playerAttackLayer != -1)
            {
                fireBall.layer = playerAttackLayer;
            }

            // 【修正箇所】Coreにダメージと属性を渡す
            NinjutsuProjectileCore core = fireBall.GetComponent<NinjutsuProjectileCore>();
            if (core != null)
            {
                core.damageAmount = baseDamage; // 伝達
                core.ninjutsuType = type;
                core.duration = duration;
            }

            // 3. 【分裂処理の委譲】投射物側のスクリプトに、分裂データを渡す
            ProjectileSplitController splitController = fireBall.GetComponent<ProjectileSplitController>();
            if (splitController != null)
            {
                splitController.SetSplitData(
                    splitProjectilePrefab,
                    initialSplitCount,
                    splitBaseSpeed,
                    duration
                );
            }
            else
            {
                Debug.LogError("[Triple Fire] プレハブに ProjectileSplitController がありません！分裂できません。");
                GameObject.Destroy(fireBall, duration);
            }
        }
    }
}