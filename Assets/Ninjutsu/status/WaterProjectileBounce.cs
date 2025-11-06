using UnityEngine;

// 水弾のプレハブにアタッチするスクリプト
public class WaterProjectileBounce : MonoBehaviour
{
    private int remainingBounces;
    private Rigidbody2D rb2d;
    private const float BOUNCE_DAMPING = 0.8f; // 跳ね返り後の速度減衰係数

    // WaterNinjutsu.cs から呼び出され、初期化を行う
    public void InitializeBounce(int maxBounces)
    {
        remainingBounces = maxBounces;
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Unityの衝突検知メソッドを使用
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // "Floor"や"Ground"タグを持つオブジェクトとの衝突のみを判定
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (remainingBounces > 0)
            {
                // 1. 跳ね返り回数を減らす
                remainingBounces--;

                // 2. 跳ね返る処理を実行 (速度を反転し、減衰させる)
                if (rb2d != null)
                {
                    // 衝突面に対して反射するベクトルを計算
                    Vector2 normal = collision.contacts[0].normal;
                    Vector2 reflectDirection = Vector2.Reflect(rb2d.velocity, normal);

                    // 速度を減衰させて再設定
                    rb2d.velocity = reflectDirection * BOUNCE_DAMPING;

                    Debug.Log($"水弾が跳ねました。残り: {remainingBounces}回");
                }
            }
            else
            {
                // 3. 跳ね返り回数が0になったら投射物を消滅させる
                Destroy(gameObject);
            }
        }
    }
}