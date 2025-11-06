using UnityEngine;

// NinjutsuElement enum は BaseNinjutsu.cs にある前提
// public enum NinjutsuElement { None, Fire, Water, Wind } 

public class NinjutsuProjectileCore : MonoBehaviour
{
    // ★BaseNinjutsuから渡されるデータ★
    [HideInInspector] public NinjutsuElement ninjutsuType;
    [HideInInspector] public float damageAmount;
    [HideInInspector] public float duration;
    [HideInInspector] public bool isExplosion; // 持続エフェクト判定

    // 【追加】投射物のヒットポイント（耐久回数） (以前の会話から追加を推奨したフィールド)
    [HideInInspector] public int hitPoints = 1;

    void Start()
    {
        if (duration > 0.01f && !isExplosion && GetComponent<ProjectileSplitController>() == null)
        {
            Destroy(gameObject, duration);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        BaseEnemy enemy = other.GetComponent<BaseEnemy>();
        if (enemy != null)
        {
            HandleHit(other.gameObject);
        }
        else if (other.CompareTag("Destructible"))
        {
            HandleObstacleHit(other.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        BaseEnemy enemy = collision.gameObject.GetComponent<BaseEnemy>();
        if (enemy != null)
        {
            HandleHit(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Destructible"))
        {
            HandleObstacleHit(collision.gameObject);
        }
    }

    private void HandleObstacleHit(GameObject obstacle)
    {
        Debug.Log($"障害物 ({obstacle.name}) を破壊しました。");
        Destroy(obstacle);

        // 障害物に当たったら必ず自身も消滅させる
        Destroy(gameObject);
    }

    private void HandleHit(GameObject enemy)
    {
        Debug.Log($"[DEBUG: CORE HIT] Collided with {enemy.name}. Current HP: {hitPoints}. Raw Damage: {damageAmount}");
        // 1. 共通処理: ダメージ適用を必ず最初に実行
        ApplyDamage(enemy, damageAmount);
        // 【ヒットポイント処理を追加】
        hitPoints--;

        // 2. 属性別処理: エフェクトと投射物の消滅を判断
        switch (ninjutsuType)
        {
            case NinjutsuElement.Fire:
                // 分裂ロジックがある場合、衝突時に自身を破壊し、分裂処理をOnDestroyに委譲
                ProjectileSplitController splitController = GetComponent<ProjectileSplitController>();

                if (splitController != null)
                {
                    Destroy(gameObject);
                }
                else
                {
                    // 通常の炎（分裂しない）は敵に当たったら消滅
                    Destroy(gameObject);
                }
                break;

            case NinjutsuElement.Water:
                // ★【削除】★ ApplyKnockback(enemy); と Destroy(gameObject) を削除
                Destroy(gameObject); // 水弾は消滅させる処理のみ残す
                break;

            case NinjutsuElement.Wind:
                // 風弾は敵に当たったら消滅
                Destroy(gameObject);
                break;

            default:
                Destroy(gameObject);
                break;
        }
    }

    // --- ヘルパーメソッド ---

    private void ApplyDamage(GameObject enemy, float damage)
    {
        Debug.Log($"[DEBUG: APPLY] Applying damage. Check: {(damage > 0)}. Final Damage: {Mathf.CeilToInt(damage)}");
        if (damage > 0)
        {
            BaseEnemy baseEnemy = enemy.GetComponent<BaseEnemy>();
            if (baseEnemy != null)
            {
                int intDamage = Mathf.CeilToInt(damage);
                baseEnemy.TakeDamage(intDamage);
                Debug.Log($"敵 ({enemy.name}) に {intDamage} ダメージを与えました。");
            }
        }
    }

    // 【★削除★】private void ApplyKnockback(GameObject enemy) メソッドを削除
}