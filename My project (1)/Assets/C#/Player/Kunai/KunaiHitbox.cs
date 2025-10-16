// KunaiHitbox.cs (再掲)
using UnityEngine;

public class KunaiHitbox : MonoBehaviour
{
    public int damageAmount = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        BaseEnemy enemy = other.GetComponent<BaseEnemy>();

        if (enemy != null)
        {
            // 敵の共通関数を呼び出しダメージを与える
            enemy.TakeDamage(damageAmount);

            // クナイは消滅
            Destroy(gameObject);
        }

        //障害物削除
        if (other.CompareTag("Destructible"))
        {
            Debug.Log($"障害物 {other.gameObject.name} をクナイで破壊しました！");

            // 障害物オブジェクトを削除
            Destroy(other.gameObject);

            // クナイも消滅させる
            Destroy(gameObject);
            return; // 処理を終了
        }

        // 壁などに当たったときも消すロジック（オプション）
        if (other.CompareTag("Ground Check"))
        {
            Destroy(gameObject);
        }
    }
}