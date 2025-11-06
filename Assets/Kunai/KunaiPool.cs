// KunaiPool.cs
using UnityEngine;
using System.Collections.Generic;

public class KunaiPool : MonoBehaviour
{
    // シングルトンインスタンス
    public static KunaiPool Instance { get; private set; }

    [Header("プールの設定")]
    public GameObject kunaiPrefab;      // プールするクナイのプレハブ
    public int poolSize = 15;           // 最大生成数 (制限)

    private Queue<GameObject> availableKunais = new Queue<GameObject>();

    void Awake()
    {
        // シングルトン設定
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        InitializePool();
    }

    private void InitializePool()
    {
        // プールの初期生成と待機状態への設定
        for (int i = 0; i < poolSize; i++)
        {
            GameObject kunai = Instantiate(kunaiPrefab, transform);
            kunai.SetActive(false); // 非アクティブにして待機
            availableKunais.Enqueue(kunai);
        }
        Debug.Log($"KunaiPool: 初期クナイ {poolSize} 個を生成しました。");
    }

    /// <summary>
    /// プールからクナイを借りる（発射時に呼び出される）
    /// </summary>
    public GameObject SpawnKunai(Vector3 position, Quaternion rotation)
    {
        if (availableKunais.Count == 0)
        {
            Debug.LogWarning("クナイプールが空です。生成制限によりクナイは発射できませんでした。");
            return null;
        }

        // プールから取り出し、位置を設定してアクティブ化
        GameObject kunai = availableKunais.Dequeue();
        kunai.transform.position = position;
        kunai.transform.rotation = rotation;
        kunai.SetActive(true);

        // クナイの動作スクリプトを初期化（あれば）
        KunaiMovement movement = kunai.GetComponent<KunaiMovement>();
        if (movement != null)
        {
            movement.InitializeKunai();
        }

        return kunai;
    }

    /// <summary>
    /// 使用後のクナイをプールに返却する
    /// </summary>
    public void ReleaseKunai(GameObject kunai)
    {
        if (kunai == null) return;

        kunai.SetActive(false);
        // プールの親オブジェクトに戻す
        kunai.transform.SetParent(transform);
        availableKunais.Enqueue(kunai);
    }
}