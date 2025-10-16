using UnityEngine;

// BaseEnemyを継承する
public class CanEnemy : BaseEnemy
{
    void Start()
    {
        // 缶モンスターはHPを高めに設定（基底クラスのprotected変数にアクセス）
        health = 5;
        gameObject.name = "CanEnemy";
    }

    // 基底クラスのDie()を 'override' して、缶独自の回収処理を記述
    protected override void Die()
    {
        // 缶をプレスするような特殊なエフェクト（Debugで代用）
        Debug.Log("Can specific: *CRUNCH!* " + gameObject.name + " is perfectly recycled.");

        // 最後に基底クラスのDestroy処理を呼び出す
        base.Die();
    }
}