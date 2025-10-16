using UnityEngine;

// すべての忍術が継承する基礎クラス
public abstract class BaseNinjutsu : ScriptableObject
{
    [Header("共通 忍術データ")]
    public string ninjutsuName = "無名の忍術";
    public Sprite icon; // 巻物UI表示用のアイコン
    public NinjutsuType type; // 忍術の種類（enum）

    [Header("戦闘/効果パラメータ")]
    public float damage = 1.0f;
    public float duration = 2.0f;
    public float resourceCost = 1; // 消費リソース（ゴミ/エネルギーなど）

    // 忍術の具体的な効果を定義する抽象メソッド
    // 継承したクラスは必ずこのメソッドを実装しなければならない
    public abstract void Activate(Vector3 originPosition, Vector3 direction, Transform caster);

    // 共通のユーティリティ機能（例: エフェクトの生成）
    protected void SpawnEffect(GameObject effectPrefab, Vector3 position)
    {
        if (effectPrefab != null)
        {
            // エフェクトを生成し、持続時間後に自動削除
            GameObject effect = Instantiate(effectPrefab, position, Quaternion.identity);
            Destroy(effect, duration);
        }
    }
}