// PlayerInput.cs
using UnityEngine;
using System.Collections.Generic;
using NinjaGame;

public class PlayerInput : MonoBehaviour
{
    // 【修正】PlayerControllerへの単一参照
    private PlayerController controller;

    void Start()
    {
        //  PlayerControllerの参照のみを取得
        controller = GetComponent<PlayerController>();

        if (controller == null)
        {
            Debug.LogError("PlayerControllerが見つかりません。同じGameObjectにアタッチされていることを確認してください。", this);
        }
    }

    void Update()
    {
        if (controller == null) return;

        float horizontalInput = 0f;

        // --- 移動の入力 ---
        if (Input.GetKey(KeyCode.D))
        {
            horizontalInput = 1f;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            horizontalInput = -1f;
        }

        //  PlayerControllerに移動処理を委譲
        controller.HandleMoveInput(horizontalInput);

        // --- ジャンプの入力 ---
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // ? PlayerControllerにジャンプ処理を委譲
            controller.HandleJumpInput();
        }

        // --- 攻撃の入力 (Qキー) ---
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("【入力検知】Qキーが押されました。手裏剣を投擲します。");
            //  PlayerControllerに攻撃処理を委譲
            controller.HandleAttackInput();
        }

        // --- 忍術発動の入力 (Eキー) ---
        if (Input.GetKeyDown(KeyCode.E))
        {
            // ? PlayerControllerに忍術処理を委譲
            controller.HandleUseNinjutsuComboOrSingleInput();
        }

        // --- 忍術一斉発動の入力 (Wキー) ---
        if (Input.GetKeyDown(KeyCode.W))
        {
            // ? PlayerControllerに忍術処理を委譲
            controller.HandleUseAllRemainingNinjutsuInput();
        }

        // --- 忍術セットのリセット (Rキー) ---
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("【入力検知】Rキーが押されました。巻物セットをリセットします。");
            // PlayerControllerに忍術処理を委譲
            controller.HandleGenerateAndSetRandomNinjutsuInput();
        }
    }
}