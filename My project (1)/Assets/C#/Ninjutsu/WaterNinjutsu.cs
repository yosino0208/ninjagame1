using UnityEngine;

[CreateAssetMenu(fileName = "WaterNinjutsu", menuName = "Ninjutsu/Water Ninjutsu")]
public class WaterNinjutsu : BaseNinjutsu
{
    // 抽象メソッド Activate をオーバーライド
    public override void Activate(Vector3 originPosition, Vector3 direction, Transform caster)
    {
        // 水遁の具体的な発動ロジックをここに記述する
        Debug.Log("Water Ninjutsu Activated (Empty)");
    }
}