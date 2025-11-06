using UnityEngine;

// メインカメラにアタッチするスクリプト
public class CameraFollow : MonoBehaviour
{
    [Header("ターゲット参照")]
    // InspectorでPlayerオブジェクトのTransformを直接割り当ててください
    public Transform target;

    [Header("追尾設定")]
    public float smoothSpeed = 0.125f; // 追尾の滑らかさ (数値が小さいほど滑らか)
    public Vector3 offset;             // ターゲットとカメラ間のオフセット距離 (Z値は必須)

    private Vector3 velocity = Vector3.zero; // Dampening用の変数

    void Awake()
    {
        // ターゲットが設定されていない場合、PlayerStatusを持つオブジェクトを自動検索 (フォールバック)
        if (target == null)
        {
            PlayerStatus player = FindObjectOfType<PlayerStatus>();
            if (player != null)
            {
                target = player.transform;
            }
            else
            {
                Debug.LogError("CameraFollow: PlayerStatusを持つターゲットが見つかりません。InspectorでTransformを割り当ててください。");
            }
        }
    }

    // Update()ではなくLateUpdate()を使うことで、
    // Playerの移動が完了した後にカメラを動かし、カクつきを防ぎます。
    void LateUpdate()
    {
        if (target == null) return;

        // ターゲット位置を計算 (ターゲットの位置 + オフセット)
        Vector3 desiredPosition = target.position + offset;

        // Z座標はカメラの位置を固定するために保持します
        desiredPosition.z = transform.position.z;

        // Lerp（線形補間）または SmoothDamp を使って滑らかに追尾
        // SmoothDampを使うと、急な移動の停止時にカメラが滑らかに減速します
        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPosition,
            ref velocity,
            smoothSpeed
        );

        // ※ Lerpを使う場合:
        // transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }
}