using UnityEngine;
using System.Collections.Generic;

public class PlayerActionInput : MonoBehaviour // ƒNƒ‰ƒX–¼‚ğ•ÏX
{
    // s“®‚ğˆ—‚·‚éƒXƒNƒŠƒvƒg‚Ö‚ÌQÆ
    private PlayerMovement movement;
<<<<<<<< HEAD:Assets/Player/controller/PlayerActionInput.cs
    private PlayerAttacker attacker; // UŒ‚ƒXƒNƒŠƒvƒg‚ÌQÆ
    private NinjutsuHandler ninjutsuHandler;
========
    private PlayerAttacker attacker; // y’Ç‰ÁzUŒ‚ƒXƒNƒŠƒvƒg‚ÌQÆ

>>>>>>>> 0b19ef9 (ãƒ—ãƒ­ãƒˆã‚¿ã‚¤ãƒ—):Assets/Player/PlayerInput.cs


    void Start()
    {
        // “¯‚¶ƒQ[ƒ€ƒIƒuƒWƒFƒNƒg‚ÉƒAƒ^ƒbƒ`‚³‚ê‚Ä‚¢‚é‘¼‚ÌƒXƒNƒŠƒvƒg‚ğæ“¾
        movement = GetComponent<PlayerMovement>();
<<<<<<<< HEAD:Assets/Player/controller/PlayerActionInput.cs
        attacker = GetComponent<PlayerAttacker>();
        ninjutsuHandler = GetComponent<NinjutsuHandler>();
========
        attacker = GetComponent<PlayerAttacker>(); // y’Ç‰ÁzPlayerAttacker‚ÌQÆ‚ğæ“¾
       

       
>>>>>>>> 0b19ef9 (ãƒ—ãƒ­ãƒˆã‚¿ã‚¤ãƒ—):Assets/Player/PlayerInput.cs
    }

    void Update()
    {
        // ˆÚ“®‚Ì•ûŒü‚ğŠi”[‚·‚é•Ï”
        float horizontalInput = 0f;

        // --- ˆÚ“®‚Ì“ü—Í (GetKey‚ğg—p) ---
        // DƒL[‚ª‰Ÿ‚³‚ê‚Ä‚¢‚éê‡i‰EˆÚ“®j
        if (Input.GetKey(KeyCode.D))
        {
            horizontalInput = 1f;
        }
        // AƒL[‚ª‰Ÿ‚³‚ê‚Ä‚¢‚éê‡i¶ˆÚ“®j
        else if (Input.GetKey(KeyCode.A))
        {
            horizontalInput = -1f;
        }

        // ÀÛ‚ÌˆÚ“®ˆ—‚ğPlayerMovement‚É”C‚¹‚é
        movement.Move(horizontalInput);

        // --- ƒWƒƒƒ“ƒv‚Ì“ü—Í (GetKeyDown‚ğg—p) ---

        // SpaceƒL[‚ª‰Ÿ‚³‚ê‚½uŠÔ
        if (Input.GetKeyDown(KeyCode.Space))
        {
            movement.Jump();
        }

        // --- UŒ‚‚Ì“ü—Í (GetKeyDown‚ğg—p) ---
        // QƒL[‚ÅUŒ‚
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("y“ü—ÍŒŸ’mzQƒL[‚ª‰Ÿ‚³‚ê‚Ü‚µ‚½BŠª•¨ƒZƒbƒg‚ğƒŠƒZƒbƒg‚µ‚Ü‚·B");
            if (attacker != null)
            {
                Debug.Log("o‚Ä‚é");
                attacker.ThrowAttack();
            }
        }

<<<<<<<< HEAD:Assets/Player/controller/PlayerActionInput.cs
        // --- ”Ep”­“®‚Ì“ü—Í (EƒL[‚ğg—p) ---
        if (ninjutsuHandler != null)
        {
            // EƒL[‚ÅƒŠƒXƒg‚Ìæ“ª‚©‚ç‡”Ô‚É”Ep‚ğ”­“®A‚¨‚æ‚Ñc‚è‚Ì”Ep‚ğ‚·‚×‚Ä”­“®
            if (Input.GetKeyDown(KeyCode.E))
            { 
                // yWƒL[‚©‚çˆÚs‚µ‚½‹@”\z: c‚è‚Ì”Ep‚ğ‘S‚Ä”­“®
                ninjutsuHandler.UseAllRemainingNinjutsu();
            }
        }

        // WƒL[‚Ìˆ—‚Ííœ‚³‚ê‚Ü‚µ‚½

        if (Input.GetKeyDown(KeyCode.R))
        {
            // yV‹KzƒfƒoƒbƒOƒƒO‚Ì’Ç‰Á
            Debug.Log("y“ü—ÍŒŸ’mzRƒL[‚ª‰Ÿ‚³‚ê‚Ü‚µ‚½BŠª•¨ƒZƒbƒg‚ğƒŠƒZƒbƒg‚µ‚Ü‚·B");
            ninjutsuHandler.GenerateAndSetRandomNinjutsu();
        }

    }
}
========
    }

}

>>>>>>>> 0b19ef9 (ãƒ—ãƒ­ãƒˆã‚¿ã‚¤ãƒ—):Assets/Player/PlayerInput.cs
