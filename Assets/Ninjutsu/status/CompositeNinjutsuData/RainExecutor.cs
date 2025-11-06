// RainExecutor.cs (修正後)

using UnityEngine;
using System.Collections;

public class RainExecutor : MonoBehaviour
{
    private TripleWaterNinjutsu _data;
    private Vector3 _originPosition;
    private float _rainHeight;
    private int _playerAttackLayer;

    public void StartRain(TripleWaterNinjutsu data, Vector3 originPosition, float rainHeight, int playerAttackLayer)
    {
        _data = data;
        _originPosition = originPosition;
        _rainHeight = rainHeight;
        _playerAttackLayer = playerAttackLayer;

        StartCoroutine(GenerateRain());
    }

    private IEnumerator GenerateRain()
    {
        int dropsRemaining = _data.totalDrops;
        float endTime = Time.time + _data.duration;

        while (Time.time < endTime && dropsRemaining > 0)
        {
            float randomX = Random.Range(-_data.range, _data.range);
            float randomY = Random.Range(-_data.range, _data.range);

            Vector3 spawnPosition = _originPosition + new Vector3(randomX, _rainHeight + randomY, 0);

            // 雨粒を生成
            GameObject rainDrop = Instantiate(_data.projectilePrefab, spawnPosition, Quaternion.identity);

            // 【★修正追加箇所 1★】NinjutsuProjectileCoreにダメージと属性を渡す
            NinjutsuProjectileCore core = rainDrop.GetComponent<NinjutsuProjectileCore>();
            if (core != null)
            {
                core.damageAmount = _data.baseDamage; // 複合技データからダメージを取得
                core.ninjutsuType = _data.type;
                core.duration = _data.duration;
            }

            // 【★追加★】生成された雨粒のレイヤーを設定
            if (_playerAttackLayer != -1)
            {
                rainDrop.layer = _playerAttackLayer;
            }

            // 投射物の初期設定 (速度設定)
            Rigidbody2D rb2d = rainDrop.GetComponent<Rigidbody2D>();
            Vector3 dropDirection = Vector3.down;

            if (rb2d != null)
            {
                rb2d.velocity = dropDirection * _data.speed;
            }

            // WaterProjectileBounce の初期化 (水属性特有の処理)
            WaterProjectileBounce bounce = rainDrop.GetComponent<WaterProjectileBounce>();
            if (bounce != null)
            {
                // ここで跳ね返り回数を設定します
                // ただし、TripleWaterNinjutsu に maxBounces パラメータがないため、固定値か、または WaterNinjutsuから値を取得する必要があるかもしれません
            }

            Destroy(rainDrop, _rainHeight / _data.speed + 1f);

            dropsRemaining--;

            yield return new WaitForSeconds(_data.dropInterval);
        }

        Destroy(gameObject);
    }
}