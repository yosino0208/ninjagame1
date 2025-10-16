using UnityEngine;

[CreateAssetMenu(fileName = "WindNinjutsu", menuName = "Ninjutsu/Wind Ninjutsu")]
public class WindNinjutsu : BaseNinjutsu
{
    // 抽象メソッド Activate をオーバーライド
    public override void Activate(Vector3 originPosition, Vector3 direction, Transform caster)
    {
        // 風遁の具体的な発動ロジックをここに記述する
        Debug.Log("Wind Ninjutsu Activated (Empty)");
    }
}