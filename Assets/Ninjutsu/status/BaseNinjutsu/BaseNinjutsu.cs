// BaseNinjutsu.cs

using UnityEngine;

// すべての忍術が継承する基礎クラス
public abstract class BaseNinjutsu : ScriptableObject
{
    [Header("共通 忍術データ")]
    public string ninjutsuName = "無名の忍術";
    public Sprite icon;
    public NinjutsuElement type;

    [Header("投射物パラメータ")]
    public GameObject projectilePrefab;
    public float launchSpeed = 10f;

    [Header("戦闘/効果パラメータ")]
    public float damage = 1.0f;
    public float duration = 2.0f;
    public float resourceCost = 1;

    [Header("Speed Variance")]
    public float speedVariance = 3.0f;    // 速度のランダムな振れ幅 (例: ±3.0f)

    // 具体的な動きを定義する抽象メソッド。実装は子クラスの Activate が行う。
    public abstract void Activate(Vector3 originPosition, Vector3 direction, Transform caster);

    // void から GameObject に戻り値を変更
    protected GameObject LaunchProjectile(Vector3 originPosition, Vector3 direction)
    {
        if (projectilePrefab == null)
        {
            Debug.LogError($"[Ninjutsu] {ninjutsuName} の projectilePrefab が設定されていません！");
            return null; // nullを返してエラーを回避
        }

        GameObject projectile = Instantiate(projectilePrefab, originPosition, Quaternion.identity);


        NinjutsuProjectileCore projectileScript = projectile.GetComponent<NinjutsuProjectileCore>();
        if (projectileScript != null)
        {
            projectileScript.damageAmount = damage;
            projectileScript.duration = duration;
            projectileScript.ninjutsuType = type;
        }

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.Euler(0, 0, angle);

        Rigidbody2D rb2d = projectile.GetComponent<Rigidbody2D>();
        if (rb2d != null)
        {
            rb2d.velocity = direction.normalized * launchSpeed;
        }

        return projectile; // 生成したオブジェクトを返す
    }

}

public enum NinjutsuElement
{
    None,
    Fire,
    Water,
    Wind
}