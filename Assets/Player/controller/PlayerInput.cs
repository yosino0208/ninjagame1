using UnityEngine;
using System.Collections.Generic;

public class PlayerInput : MonoBehaviour
{
    // s“®‚ğˆ—‚·‚éƒXƒNƒŠƒvƒg‚Ö‚ÌQÆ
    private PlayerMovement movement;
    private PlayerAttacker attacker; // y’Ç‰ÁzUŒ‚ƒXƒNƒŠƒvƒg‚ÌQÆ
<<<<<<< HEAD
<<<<<<< HEAD

<<<<<<<< HEAD:Assets/Player/controller/PlayerInput.cs

========
>>>>>>>> 0b19ef9 (ãƒ—ãƒ­ãƒˆã‚¿ã‚¤ãƒ—):Assets/Player/PlayerInput.cs
=======
    private NinjutsuHandler ninjutsuHandler;

>>>>>>> 0b19ef9 (ãƒ—ãƒ­ãƒˆã‚¿ã‚¤ãƒ—)
=======
    private NinjutsuHandler ninjutsuHandler;

>>>>>>> 0b19ef9 (ãƒ—ãƒ­ãƒˆã‚¿ã‚¤ãƒ—)

    void Start()
    {
        // “¯‚¶ƒQ[ƒ€ƒIƒuƒWƒFƒNƒg‚ÉƒAƒ^ƒbƒ`‚³‚ê‚Ä‚¢‚é‘¼‚ÌƒXƒNƒŠƒvƒg‚ğæ“¾
        movement = GetComponent<PlayerMovement>();
        attacker = GetComponent<PlayerAttacker>(); // y’Ç‰ÁzPlayerAttacker‚ÌQÆ‚ğæ“¾
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<<< HEAD:Assets/Player/controller/PlayerInput.cs
<<<<<<<< HEAD:Assets/Player/controller/PlayerInput.cs
        ninjutsuHandler = GetComponent<NinjutsuHandler>();
========
========
>>>>>>>> 0b19ef9 (ãƒ—ãƒ­ãƒˆã‚¿ã‚¤ãƒ—):Assets/Player/PlayerInput.cs
       

       
>>>>>>>> 0b19ef9 (ãƒ—ãƒ­ãƒˆã‚¿ã‚¤ãƒ—):Assets/Player/PlayerInput.cs
=======
        ninjutsuHandler = GetComponent<NinjutsuHandler>();
>>>>>>> 0b19ef9 (ãƒ—ãƒ­ãƒˆã‚¿ã‚¤ãƒ—)
=======
        ninjutsuHandler = GetComponent<NinjutsuHandler>();
>>>>>>> 0b19ef9 (ãƒ—ãƒ­ãƒˆã‚¿ã‚¤ãƒ—)
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
        // —á‚¦‚ÎA¶ShiftƒL[‚ÅUŒ‚
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("y“ü—ÍŒŸ’mzQƒL[‚ª‰Ÿ‚³‚ê‚Ü‚µ‚½BŠª•¨ƒZƒbƒg‚ğƒŠƒZƒbƒg‚µ‚Ü‚·B");
            if (attacker != null)
            {
                Debug.Log("o‚Ä‚é");
                attacker.ThrowAttack();
            }
        }

<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<<< HEAD:Assets/Player/controller/PlayerInput.cs
=======
>>>>>>> 0b19ef9 (ãƒ—ãƒ­ãƒˆã‚¿ã‚¤ãƒ—)
=======
>>>>>>> 0b19ef9 (ãƒ—ãƒ­ãƒˆã‚¿ã‚¤ãƒ—)
        // --- ”Ep”­“®‚Ì“ü—Í (EƒL[‚ğg—p) ---
        if (ninjutsuHandler != null)
        {
            // EƒL[‚ÅƒŠƒXƒg‚Ìæ“ª‚©‚ç‡”Ô‚É”Ep‚ğ”­“®
            if (Input.GetKeyDown(KeyCode.E))
            {
                ninjutsuHandler.UseNinjutsuComboOrSingle();
            }
        }

        // --- ”Ep”­“®‚Ì“ü—Í (EƒL[‚ğg—p) ---
        if (ninjutsuHandler != null)
        {
            // EƒL[‚ÅƒŠƒXƒg‚Ìæ“ª‚©‚ç‡”Ô‚É”Ep‚ğ”­“®
            if (Input.GetKeyDown(KeyCode.W))
            {
                ninjutsuHandler.UseAllRemainingNinjutsu();
            }
        }

<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<<< HEAD:Assets/Player/controller/PlayerInput.cs
=======
>>>>>>> 0b19ef9 (ãƒ—ãƒ­ãƒˆã‚¿ã‚¤ãƒ—)
=======
>>>>>>> 0b19ef9 (ãƒ—ãƒ­ãƒˆã‚¿ã‚¤ãƒ—)
        if (Input.GetKeyDown(KeyCode.R))
        {
            // yV‹KzƒfƒoƒbƒOƒƒO‚Ì’Ç‰Á
            Debug.Log("y“ü—ÍŒŸ’mzRƒL[‚ª‰Ÿ‚³‚ê‚Ü‚µ‚½BŠª•¨ƒZƒbƒg‚ğƒŠƒZƒbƒg‚µ‚Ü‚·B");
            ninjutsuHandler.GenerateAndSetRandomNinjutsu();
        }

    }
<<<<<<< HEAD
<<<<<<< HEAD
}
========
    }
========
    }
>>>>>>>> 0b19ef9 (ãƒ—ãƒ­ãƒˆã‚¿ã‚¤ãƒ—):Assets/Player/PlayerInput.cs

}

>>>>>>>> 0b19ef9 (ãƒ—ãƒ­ãƒˆã‚¿ã‚¤ãƒ—):Assets/Player/PlayerInput.cs
=======
}
>>>>>>> 0b19ef9 (ãƒ—ãƒ­ãƒˆã‚¿ã‚¤ãƒ—)
=======
}
>>>>>>> 0b19ef9 (ãƒ—ãƒ­ãƒˆã‚¿ã‚¤ãƒ—)
