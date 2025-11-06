using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    // --- ステータス ---
    [Header("ステータス")] // UnityのInspectorで見やすくするための設定
    public int currentHP = 4; // 現在のHP (ハート4つを想定 )
    public float moveSpeed = 5.0f; // 移動速度
    public float jumpPower = 10.0f; // ジャンプ力


    // HPが減る処理（他のスクリプトから呼ばれる）
    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        // 死亡判定など
        if (currentHP <= 0)
        {
            Debug.Log("クリー忍者は倒れてしまった！");
            // Game Over処理を呼び出す
        }
    }
}