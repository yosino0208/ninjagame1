using UnityEngine;

// 攻撃コンポーネントの基盤
public abstract class AttackComponent : MonoBehaviour
{
    protected BaseEnemy owner;

    protected virtual void Awake()
    {
        // 共通Awake処理
    }

    public virtual void Initialize(BaseEnemy enemy)
    {
        owner = enemy;
    }

    // 毎フレームの攻撃判定を定義する抽象メソッド
    public abstract void Act();
}