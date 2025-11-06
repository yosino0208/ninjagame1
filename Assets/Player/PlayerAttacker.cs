using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
   
    [Header("ｸﾅｲ")]
    public Sprite kunaiAssetSprite;
    public float kunaiThrowSpeed = 15.0f;
    public Vector2 kunaiColliderSize = new Vector2(0.5f, 0.1f);
    public float kunaiLifetime = 2.0f;

    private PlayerStatus status;
    private KunaiStatus kunaiData;

    void Start()
    {
       
        status = GetComponent<PlayerStatus>();

      
        GameObject kunaiDataManager = new GameObject("Kunai Data Manager");
        kunaiDataManager.transform.SetParent(this.transform);
        kunaiData = kunaiDataManager.AddComponent<KunaiStatus>();

      
        kunaiData.kunaiSprite = kunaiAssetSprite;
        kunaiData.throwSpeed = kunaiThrowSpeed;
        kunaiData.kunaiColliderSize = kunaiColliderSize;
        kunaiData.lifetime = kunaiLifetime;
    }

    public void ThrowAttack()
    {
        if (kunaiData.kunaiSprite == null)
        {
            Debug.LogError("クナイ画像が KunaiStatus に設定されていません！");
            return;
        }

        // 1. クナイオブジェクトの生成
        GameObject projectile = new GameObject("Kunai");

        // 2. 必須コンポーネントの追加と設定
        SpriteRenderer sr = projectile.AddComponent<SpriteRenderer>();
        sr.sprite = kunaiData.kunaiSprite;

        Rigidbody2D projRb = projectile.AddComponent<Rigidbody2D>();
        projRb.bodyType = RigidbodyType2D.Dynamic;
        projRb.gravityScale = 0;

        BoxCollider2D projCollider = projectile.AddComponent<BoxCollider2D>();
        projCollider.isTrigger = true;
        projCollider.size = kunaiData.kunaiColliderSize;

        KunaiHitbox hitbox = projectile.AddComponent<KunaiHitbox>();
        hitbox.damageAmount = kunaiData.damage;

        // 【修正】プレイヤーの向きを取得
        float directionX = transform.localScale.x;

        // 3. 発射位置の決定 (冗長な記述を削除)
        // プレイヤーの向きに応じて横に0.5fずらした位置
        Vector3 spawnPosition = transform.position + new Vector3(directionX * 0.5f, 0, 0);
        projectile.transform.position = spawnPosition;

        // 4. 発射方向と速度の設定 (冗長な記述を削除)
        Vector2 throwDirection = new Vector2(directionX, 0);

        // 【注意】linearVelocity は rb.velocity に変更することを推奨
        projRb.linearVelocity = throwDirection * kunaiData.throwSpeed;

        // 5. ライフタイムで削除
        Destroy(projectile, kunaiData.lifetime);
    }
}