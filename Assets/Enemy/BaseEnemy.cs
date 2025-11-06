using UnityEngine;
using System.Collections.Generic;

public class BaseEnemy : MonoBehaviour
{
    // --- データアセット ---
    [Header("データアセット")]
    // BaseEnemyDataを継承したデータなら何でも受け取れる
    [SerializeField] private EnemyData enemyData;

    public int currentHP { get; private set; }
    public EnemyData Data => enemyData;

    // --- コンポーネント ---
    private List<MoveComponent> moveComponents = new List<MoveComponent>();
    private List<AttackComponent> attackComponents = new List<AttackComponent>();

    private void Awake()
    {
        if (enemyData != null)
        {
            currentHP = enemyData.maxHP;
        }
        else
        {
            Debug.LogError("BaseEnemyDataが設定されていません: " + gameObject.name);
            currentHP = 1;
        }
    }

    private void Start()
    {
        LoadAndInitializeComponents();
    }

    private void Update()
    {
        // 有効なコンポーネントのみAct()を実行
        foreach (var comp in moveComponents)
        {
            if (comp.enabled) comp.Act();
        }
        foreach (var comp in attackComponents)
        {
            if (comp.enabled) comp.Act();
        }
    }

    // --- ダメージ/死亡処理 ---
    public void TakeDamage(int damage)
    {
        if (currentHP <= 0) return;
        currentHP -= damage;
        if (currentHP <= 0) Die();
    }

    private void Die()
    {
        Debug.Log(enemyData.enemyName + " は倒された！");
        Destroy(gameObject);
    }

    // --- コンポーネント管理 ---

    private void LoadAndInitializeComponents()
    {
        if (enemyData == null) return;

        // 動きコンポーネントのロード (string[] をループ)
        foreach (string className in enemyData.moveComponentNames)
        {
            var comp = LoadComponent<MoveComponent>(className);
            if (comp != null)
            {
                comp.Initialize(this); // 引数なしで初期化
                moveComponents.Add(comp);
                comp.enabled = (moveComponents.Count == 1);
            }
        }

        // 攻撃コンポーネントのロード (string[] をループ)
        foreach (string className in enemyData.attackComponentNames)
        {
            var comp = LoadComponent<AttackComponent>(className);
            if (comp != null)
            {
                comp.Initialize(this); // 引数なしで初期化
                attackComponents.Add(comp);
                comp.enabled = (attackComponents.Count == 1);
            }
        }
    }

    // LoadComponentヘルパー関数: stringを使用して動的にコンポーネントをアタッチ
    private T LoadComponent<T>(string componentName) where T : MonoBehaviour
    {
        if (string.IsNullOrEmpty(componentName)) return null;

        System.Type type = System.Type.GetType(componentName);

        if (type != null && typeof(T).IsAssignableFrom(type))
        {
            return (T)gameObject.AddComponent(type);
        }
        else
        {
            Debug.LogError($"コンポーネントが見つからないか、型が不正です: {componentName}。適切なクラス名か確認してください。");
            return null;
        }
    }
}