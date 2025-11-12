// IDamageable.cs
using UnityEngine;


public interface IDamageable
{
    // ダメージ量を受け取り、HPを減らす機能
    void TakeDamage(int damageAmount);
}
