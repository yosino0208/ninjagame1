// NinjutsuUtility.cs (ïœçXÇ»Çµ)

using UnityEngine;

public static class NinjutsuUtility
{
    public static GameObject LaunchProjectile(BaseNinjutsu data, Vector3 originPosition, Vector3 direction)
    {
        if (data.projectilePrefab == null)
        {
            Debug.LogError($"[Ninjutsu Utility] {data.ninjutsuName} ÇÃ projectilePrefab Ç™ê›íËÇ≥ÇÍÇƒÇ¢Ç‹ÇπÇÒÅI");
            return null;
        }

        GameObject projectile = GameObject.Instantiate(data.projectilePrefab, originPosition, Quaternion.identity);

        NinjutsuProjectileCore projectileScript = projectile.GetComponent<NinjutsuProjectileCore>();
        if (projectileScript != null)
        {
            projectileScript.damageAmount = data.damage;
            projectileScript.duration = data.duration;
            projectileScript.ninjutsuType = data.type;
        }

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.Euler(0, 0, angle);

        Rigidbody2D rb2d = projectile.GetComponent<Rigidbody2D>();
        if (rb2d != null)
        {
            rb2d.velocity = direction.normalized * data.launchSpeed;
        }

        return projectile;
    }
}