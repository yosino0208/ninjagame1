// PlayerAttacker.cs
using NinjaGame;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    private PlayerStatus status;
    private PlayerController controller; // PlayerControllerへの参照

    // 投擲するクナイのPrefab
    [Header("クナイ設定")]
    [SerializeField] private GameObject kunaiPrefab;

    [Header("発射設定")]
    public float kunaiHorizontalOffset = 0.5f; // 発射オフセット
    public float throwSpeed = 15.0f;           // クナイの投てき速度

    public void SetStatusReference(PlayerStatus statusReference)
    {
        status = statusReference;
    }

    // PlayerControllerからの参照を受け取るメソッド (DI用)
    public void SetControllerReference(PlayerController controllerReference)
    {
        controller = controllerReference;
    }

    void Start()
    {
        if (kunaiPrefab == null)
        {
            Debug.LogError("PlayerAttacker: kunaiPrefabが設定されていません。クナイを投げるにはPrefabを設定してください。", this);
        }
    }

    public void ThrowAttack()
    {
        // ⭐ 【変更点】Prefabが設定されていない場合は処理を中止
        if (kunaiPrefab == null) return;

        // プレイヤーの向きを取得
        float directionX = transform.localScale.x > 0 ? 1f : -1f;

        // 1. 発射位置と方向の計算 
        Vector3 spawnPosition = transform.position + new Vector3(directionX * kunaiHorizontalOffset, 0, 0);
        Vector2 throwDirection = new Vector2(directionX, 0);


        GameObject newKunai = Instantiate(kunaiPrefab, spawnPosition, Quaternion.identity);

        if (newKunai != null)
        {
            // 2. 生成したクナイに速度と方向を設定
            Rigidbody2D kunaiRb = newKunai.GetComponent<Rigidbody2D>();
            if (kunaiRb != null)
            {
                // 進行方向に合わせて回転させる (任意)
                newKunai.transform.right = throwDirection;

                // 速度を設定
                kunaiRb.velocity = throwDirection * throwSpeed;
            }
            else
            {
                Debug.LogWarning($"生成されたクナイ ({kunaiPrefab.name}) に Rigidbody2D が見つかりません。", newKunai);
            }
        }
    }
}