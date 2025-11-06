// WindProjectileOrbiter.cs (新規作成)

using UnityEngine;

public class WindProjectileOrbiter : MonoBehaviour
{
    private Transform _caster;
    private float _orbitalRadius;
    private float _orbitalSpeed;
    private float _launchDelay;
    private float _launchSpeed;
    private Vector3 _launchDirection;

    private float _currentAngle;
    private bool _isLaunched = false;

    // TripleWindNinjutsuから呼び出される初期化メソッド
    public void Initialize(
        Transform caster,
        float radius,
        float speed,
        float delay,
        float currentAngle,
        float launchSpeed) // 投射物自身の速度
    {
        _caster = caster;
        _orbitalRadius = radius;
        _orbitalSpeed = speed;
        _launchDelay = delay;
        _currentAngle = currentAngle;
        _launchSpeed = launchSpeed;

        // 発射タイマーを開始
        Invoke("PerformLaunch", _launchDelay);

    }

    void Update()
    {
        if (_isLaunched || _caster == null) return;

        // 軌道回転の計算
        _currentAngle += _orbitalSpeed * Time.deltaTime;

        // 回転位置を計算
        float rad = _currentAngle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * _orbitalRadius;

        // プレイヤーの位置を基準に移動
        transform.position = _caster.position + offset;
    }

    private void PerformLaunch()
    {
        _isLaunched = true;

        // 発射方向（プレイヤーの中心から投射物に向かうベクトル）
        _launchDirection = (transform.position - _caster.position).normalized;

        // Rigidbodyを有効化し、発射する
        Rigidbody2D rb2d = GetComponent<Rigidbody2D>();
        if (rb2d != null)
        {
            rb2d.simulated = true; // 物理演算を有効化
            rb2d.velocity = _launchDirection * _launchSpeed;
        }

        // 発射されたら、このスクリプトは役目を終える（NinjutsuProjectileCoreの寿命判定に委ねる）
        Destroy(this);
    }
}