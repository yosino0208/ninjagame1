// TripleWaterNinjutsu.cs

using UnityEngine;

[CreateAssetMenu(fileName = "TripleWaterNinjutsu", menuName = "Ninjutsu/Water/Triple Water Combo")]
public class TripleWaterNinjutsu : CompositeNinjutsuData
{
    [Header("雨パラメータ")]
    public int totalDrops = 20;           // 生成する雨粒の総数
    public float dropInterval = 0.1f;     // 雨粒を生成する間隔（秒）
    public float rainHeight = 5.0f;       // 雨粒が生成される高さ

    public override void Activate(Vector3 originPosition, Vector3 direction, Transform caster)
    {
        Debug.Log($"【複合技発動】: {ninjutsuName} - プレイヤーの頭上広範囲に水滴を降らせる！"); //

        if (projectilePrefab != null) //
        {
            // 【★追加★】プレイヤー攻撃レイヤーを取得
            int playerAttackLayer = LayerMask.NameToLayer("PLAYER_ATTACK"); //
            if (playerAttackLayer == -1) //
            {
                Debug.LogError("[Triple Water] 'PLAYER_ATTACK' レイヤーが見つかりません。プロジェクト設定を確認してください。"); //
            }

            GameObject executor = new GameObject("TripleWaterExecutor"); //
            RainExecutor rainScript = executor.AddComponent<RainExecutor>(); //

            // 【★修正追加箇所★】CompositeNinjutsuData全体とレイヤー情報をExecutorに渡す
            // Executor内部で baseDamage, type, duration を利用できるようにする
            rainScript.StartRain( //
                this, // 複合技データ全体を渡す (baseDamage, type, duration を含む)
                originPosition, //
                rainHeight, //
                playerAttackLayer // レイヤー情報を追加
            );

            GameObject.Destroy(executor, duration + 0.5f); //
        }
        else
        {
            Debug.LogError($"[Triple Water] プレハブが設定されていません。"); //
        }
    }
}