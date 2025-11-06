using UnityEngine;

// すべての複合技データが継承する基底クラス
public abstract class CompositeNinjutsuData : ScriptableObject
{
    [Header("投射物パラメータ")]
    public GameObject projectilePrefab;

    [Header("共通 複合技データ")]
    public string ninjutsuName = "複合技";
    // 【★追加★】複合技の属性タイプ
    public NinjutsuElement type;

    public float baseDamage = 1.0f;
    public float duration = 0.5f;    // 持続時間
    public float range = 1.0f;       // 範囲 (爆発など)
    public float speed = 1.0f;       // 速度 (巨大波など)

    // 投射物の発射位置のオフセット距離
    [Header("発射位置")]
    public float launchOffset = 1.5f; // プレイヤーの体から少し離れた距離を設定

    // 複合技の具体的な効果を定義する抽象メソッド
    public abstract void Activate(Vector3 originPosition, Vector3 direction, Transform caster);
}