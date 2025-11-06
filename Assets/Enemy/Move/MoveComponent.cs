using UnityEngine;

// 動きコンポーネントの基盤
public abstract class MoveComponent : MonoBehaviour
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

    // 毎フレームの動きを定義する抽象メソッド
    public abstract void Act();
}