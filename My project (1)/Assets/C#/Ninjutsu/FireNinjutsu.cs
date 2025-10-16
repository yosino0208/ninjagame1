using UnityEngine;

// UnityのInspectorでデータアセットとして作成可能にする
[CreateAssetMenu(fileName = "FireNinjutsu", menuName = "Ninjutsu/Fire Ninjutsu")]
public class FireNinjutsu : BaseNinjutsu
{
    // 抽象メソッド Activate をオーバーライド（継承のルール）
    public override void Activate(Vector3 originPosition, Vector3 direction, Transform caster)
    {
        // 火遁の具体的な発動ロジックをここに記述する
        Debug.Log("Fire Ninjutsu Activated (Empty)");

        // BaseNinjutsuで定義された共通の処理（例: エフェクト生成など）を呼び出す
        // SpawnEffect(...);
    }
}