using UnityEngine;

// クナイの情報（データ）を保持するスクリプト
public class KunaiStatus : MonoBehaviour
{
    [Header("クナイ データ")]
    public float throwSpeed = 15.0f;           // 投てき物の速さ (現在はPlayerControllerから上書きされる)
    public Sprite kunaiSprite;                 // クナイの画像
    public Vector2 kunaiColliderSize = new Vector2(0.5f, 0.1f); // 衝突判定に必要なサイズ

    [Header("戦闘パラメータ")]
    public int damage = 1;                     // 与えるダメージ
    public float lifetime = 2.0f;              // 自動消滅までの時間

    // クナイの動作処理を行うスクリプトがここから情報を引っ張ります
}