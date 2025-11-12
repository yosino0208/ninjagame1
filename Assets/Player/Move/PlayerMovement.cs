// PlayerMovement.cs
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    private IPlayerMoverStatus status;

    private PlayerController controller; // PlayerControllerへの参照を追加

    [Header("Jump Settings")]
    [SerializeField] private int maxJumps = 2;
    private int currentJumps = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }


    public void SetStatusReference(IPlayerMoverStatus statusReference)
    {
        status = statusReference;
    }

    //PlayerControllerからの参照を受け取る
    public void SetControllerReference(PlayerController controllerReference)
    {
        controller = controllerReference;
    }

    public void Move(float direction)
    {
        if (status == null || rb == null) return;

        // インターフェース経由でMoveSpeedにアクセス
        rb.velocity = new Vector2(direction * status.MoveSpeed, rb.velocity.y);

        float absX = Mathf.Abs(transform.localScale.x);
        float absY = Mathf.Abs(transform.localScale.y);
        float currentZ = transform.localScale.z;

        if (direction > 0)
        {
            transform.localScale = new Vector3(absX, absY, currentZ);
        }
        else if (direction < 0)
        {
            transform.localScale = new Vector3(-absX, absY, currentZ);
        }

        //アニメーション制御をPlayerController経由で呼び出す
        controller.SetMovementAnimation(direction);

    }

    public void Jump()
    {
        if (status == null || rb == null) return;

        if (currentJumps < maxJumps)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);

            // インターフェース経由でJumpPowerにアクセス
            rb.AddForce(Vector2.up * status.JumpPower, ForceMode2D.Impulse);

            currentJumps++;
            Debug.Log($"Jump! Current Jumps: {currentJumps}");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.9f)
            {
                currentJumps = 0;
                Debug.Log("Landed! Jumps Reset.");
                return;
            }
        }
    }
}