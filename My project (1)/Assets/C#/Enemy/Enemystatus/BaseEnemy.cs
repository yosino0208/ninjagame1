using UnityEngine;

// MonoBehaviourを継承した全てのゴミモンスターの基底クラス
public class BaseEnemy : MonoBehaviour
{
    // --- コア ステータス ---
    protected int health = 3;
    // Rigidbody2Dは子クラスでも使用するため、ここで参照を持つ
    protected Rigidbody2D rb;

    void Start()
    {
        // Rigidbodyの参照を取得
        rb = GetComponent<Rigidbody2D>();

        // Inspectorで割り振られているため、ここではGetComponentは不要

        // エラーチェック（Inspectorで設定が必須の場合）
        if (rb == null) Debug.LogError("BaseEnemy: Rigidbody2Dがアタッチされていません!");
    }


    // ダメージを受ける処理（全モンスター共通）
    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log(gameObject.name + " hit! Health: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    // 回収（消滅）処理
    // 'virtual' をつけることで、子クラスで個別の処理に上書き（オーバーライド）可能になる
    protected virtual void Die()
    {
        Debug.Log(gameObject.name + " Recycled!");

        // 【共通の回収ロジック】: 最終的にGameObjectを削除
        Destroy(gameObject);
    }
}