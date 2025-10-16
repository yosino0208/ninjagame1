using UnityEngine;

// BaseEnemyを継承する
public class BottleEnemy : BaseEnemy
{
    void Start()
    {
        // ボトルモンスターはHPを低めに設定
        health = 5;
        gameObject.name = "BottleEnemy";
    }
    // 基底クラスのDie()を 'override' して、ボトル独自の回収処理を記述
    protected override void Die()
    {
        // ボトルをプレスするような特殊なエフェクト（Debugで代用）
        Debug.Log("Can specific: *CRUNCH!* " + gameObject.name + " is perfectly recycled.");

        // 最後に基底クラスのDestroy処理を呼び出す
        base.Die();
    }
}