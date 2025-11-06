using UnityEngine;

// 右クリックメニューからアセットを作成するための設定
[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Enemy/Enemy Data")]
public class EnemyData : ScriptableObject
{
    // 敵の基本的なパラメータ（静的なデータ）
    [Header("共通 敵データ")]
    public string enemyName = "Unknown Enemy";
    public int maxHP = 100;
    public int attackDamage = 10;
    public float moveSpeed = 3.0f;
    public float detectionRange = 5.0f; // プレイヤーの発見範囲

    [Header("AIコンポーネント")]

    // リストの中身はクラス名を表すstring
    [Tooltip("この敵が使用する動きのクラス名リスト (例: PatrolMove)")]
    public string[] moveComponentNames;

    // リストの中身はクラス名を表すstring
    [Tooltip("この敵が使用する攻撃のクラス名リスト (例: MeleeAttack)")]
    public string[] attackComponentNames;

    [Header("攻撃データ")]
    [Tooltip("遠距離攻撃で使用する弾丸のプレハブ")]
    public GameObject projectilePrefab; // RangedAttack_Waterがこれを使う
}