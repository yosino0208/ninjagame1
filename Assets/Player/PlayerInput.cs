using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    // 行動を処理するスクリプトへの参照
    private PlayerMovement movement;
    private PlayerAttacker attacker; // 【追加】攻撃スクリプトの参照


    void Start()
    {
        // 同じゲームオブジェクトにアタッチされている他のスクリプトを取得
        movement = GetComponent<PlayerMovement>();
        attacker = GetComponent<PlayerAttacker>(); // 【追加】PlayerAttackerの参照を取得
       

       
    }

    void Update()
    {
        // 移動の方向を格納する変数
        float horizontalInput = 0f;

        // --- 移動の入力 (GetKeyを使用) ---
        // Dキーが押されている場合（右移動）
        if (Input.GetKey(KeyCode.D))
        {
            horizontalInput = 1f;
        }
        // Aキーが押されている場合（左移動）
        else if (Input.GetKey(KeyCode.A))
        {
            horizontalInput = -1f;
        }

        // 実際の移動処理をPlayerMovementに任せる
        movement.Move(horizontalInput);

        // --- ジャンプの入力 (GetKeyDownを使用) ---

        // Spaceキーが押された瞬間
        if (Input.GetKeyDown(KeyCode.Space))
        {
            movement.Jump();
        }

        // --- 攻撃の入力 (GetKeyDownを使用) ---
        // 例えば、左Shiftキーで攻撃
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (attacker != null)
            {
                attacker.ThrowAttack(); // 【修正】PlayerAttackerを呼び出す
                Debug.Log("hit");
            }
        }

    }

}

