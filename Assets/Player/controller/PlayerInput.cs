using UnityEngine;
using System.Collections.Generic;

public class PlayerInput : MonoBehaviour
{
    // 行動を処理するスクリプトへの参照
    private PlayerMovement movement;
    private PlayerAttacker attacker; // 【追加】攻撃スクリプトの参照
    private NinjutsuHandler ninjutsuHandler;


    void Start()
    {
        // 同じゲームオブジェクトにアタッチされている他のスクリプトを取得
        movement = GetComponent<PlayerMovement>();
        attacker = GetComponent<PlayerAttacker>(); // 【追加】PlayerAttackerの参照を取得
        ninjutsuHandler = GetComponent<NinjutsuHandler>();
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
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("【入力検知】Qキーが押されました。巻物セットをリセットします。");
            if (attacker != null)
            {
                Debug.Log("出てる");
                attacker.ThrowAttack();
            }
        }

        // --- 忍術発動の入力 (Eキーを使用) ---
        if (ninjutsuHandler != null)
        {
            // Eキーでリストの先頭から順番に忍術を発動
            if (Input.GetKeyDown(KeyCode.E))
            {
                ninjutsuHandler.UseNinjutsuComboOrSingle();
            }
        }

        // --- 忍術発動の入力 (Eキーを使用) ---
        if (ninjutsuHandler != null)
        {
            // Eキーでリストの先頭から順番に忍術を発動
            if (Input.GetKeyDown(KeyCode.W))
            {
                ninjutsuHandler.UseAllRemainingNinjutsu();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            // 【新規】デバッグログの追加
            Debug.Log("【入力検知】Rキーが押されました。巻物セットをリセットします。");
            ninjutsuHandler.GenerateAndSetRandomNinjutsu();
        }

    }
}