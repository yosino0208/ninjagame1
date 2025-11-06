using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))] 
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerStatus status;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        status = GetComponent<PlayerStatus>();

    }


    public void Move(float direction)
    {


        rb.linearVelocity = new Vector2(direction * status.moveSpeed, rb.linearVelocity.y);

        // 現在のY, Zスケールを保持
        float currentY = transform.localScale.y;
        float currentZ = transform.localScale.z;

        if (direction > 0)
        {
            // 右向き: Xを現在のXの絶対値に戻す
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), currentY, currentZ);
        }
        else if (direction < 0)
        {
            // 左向き: Xを現在のXの絶対値のマイナスにする
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), currentY, currentZ);
        }
    }


        public void Jump()
    {

      
        rb.AddForce(Vector2.up * status.jumpPower, ForceMode2D.Impulse);
     
    }


}