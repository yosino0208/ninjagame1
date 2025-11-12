// PlayerStatus.cs
using UnityEngine;

//インターフェースを実装
public class PlayerStatus : MonoBehaviour, IPlayerMoverStatus
{
    // --- ステータス ---
    [Header("ステータス")]
    public int currentHP = 4;
    public float moveSpeed = 5.0f;
    public float jumpPower = 10.0f;

    //インターフェースを満たすためのプロパティ
    public float MoveSpeed => moveSpeed;
    public float JumpPower => jumpPower;


    // HPが減る処理（他のスクリプトから呼ばれる）
    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        if (currentHP <= 0)
        {
            Debug.Log("クリー忍者は倒れてしまった！");
            Die();
        }
    }

    // 死亡時の処理を実行するメソッド
    private void Die()
    {
        gameObject.SetActive(false);
    }
}