// PlayerController.cs

using UnityEngine;

[RequireComponent(typeof(PlayerInput))]

[RequireComponent(typeof(PlayerMovement))]

[RequireComponent(typeof(PlayerAttacker))]

[RequireComponent(typeof(PlayerStatus))]

[RequireComponent(typeof(NinjutsuHandler))]

[RequireComponent(typeof(NinjutsuCombiner))]

[RequireComponent(typeof(Animator))]

[RequireComponent(typeof(BoxCollider2D))]

public class PlayerController : MonoBehaviour, IDamageable

{



    [Header("コンポーネント参照 (設定漏れはGetComponentで補完)")]

    [SerializeField] private PlayerInput playerInput;

    [SerializeField] private PlayerMovement movement;

    [SerializeField] private PlayerAttacker attacker;

    [SerializeField] private PlayerStatus status;

    [SerializeField] private NinjutsuHandler ninjutsuHandler;

    [SerializeField] private NinjutsuCombiner ninjutsuCombiner;

    [SerializeField] private Animator animator;


    public PlayerInput PlayerInput => playerInput;

    public PlayerMovement Movement => movement;

    public PlayerAttacker Attacker => attacker;

    public PlayerStatus Status => status;

    public NinjutsuHandler NinjutsuHandler => ninjutsuHandler;

    public Animator Animator => animator;

    public NinjutsuCombiner NinjutsuCombiner => ninjutsuCombiner;

    void Awake()

    {


        if (playerInput == null)
        {
            playerInput = GetComponent<PlayerInput>();
            if (playerInput == null) Debug.LogError("PlayerInputが見つかりません。", this);
            else Debug.LogWarning("PlayerInputがInspectorで未設定だったため、GetComponentで自動取得しました。", this);
        }


        // PlayerMovementの参照取得

        if (movement == null)

        {

            movement = GetComponent<PlayerMovement>();

            if (movement == null) Debug.LogError("PlayerMovementが見つかりません。", this);

            else Debug.LogWarning("PlayerMovementがInspectorで未設定だったため、GetComponentで自動取得しました。", this);

        }



        // PlayerAttackerの参照取得

        if (attacker == null)

        {

            attacker = GetComponent<PlayerAttacker>();

            if (attacker == null) Debug.LogError("PlayerAttackerが見つかりません。", this);

            else Debug.LogWarning("PlayerAttackerがInspectorで未設定だったため、GetComponentで自動取得しました。", this);

        }



        // PlayerStatusの参照取得

        if (status == null)

        {

            status = GetComponent<PlayerStatus>();

            if (status == null) Debug.LogError("PlayerStatusが見つかりません。", this);

            else Debug.LogWarning("PlayerStatusがInspectorで未設定だったため、GetComponentで自動取得しました。", this);

        }



        // NinjutsuHandlerの参照取得

        if (ninjutsuHandler == null)

        {

            ninjutsuHandler = GetComponent<NinjutsuHandler>();

            if (ninjutsuHandler == null) Debug.LogError("NinjutsuHandlerが見つかりません。", this);

            else Debug.LogWarning("NinjutsuHandlerがInspectorで未設定だったため、GetComponentで自動取得しました。", this);

        }

        //ninjutsuCombinerの参照取得
        if (ninjutsuCombiner == null)
        {
            ninjutsuCombiner = GetComponent<NinjutsuCombiner>();
            // 変数名を 'ninjutsuCombiner' に統一
            if (ninjutsuCombiner == null) Debug.LogError("NinjutsuCombinerが見つかりません。", this);
            else Debug.LogWarning("NinjutsuCombinerがInspectorで未設定だったため、GetComponentで自動取得しました。", this);
        }

        // Animatorの参照取得
        if (animator == null)
        {
            animator = GetComponent<Animator>();
            if (animator == null) Debug.LogError("Animatorが見つかりません。", this);
            else Debug.LogWarning("AnimatorがInspectorで未設定だったため、GetComponentで自動取得しました。", this);
        }




        // 依存するコンポーネントへ参照を注入（DI）

        movement?.SetStatusReference(status);

        attacker?.SetStatusReference(status);


        // PlayerMovementとPlayerAttackerにController自身を注入

        movement?.SetControllerReference(this);

        attacker?.SetControllerReference(this);

        ninjutsuHandler?.SetControllerReference(this);

    }



    // --- IDamageableインターフェースの実装（敵からの窓口） ---

    public void TakeDamage(int damageAmount)

    {

        Status?.TakeDamage(damageAmount);

    }

    //アニメーション制御メソッドをPlayerControllerに追加

    // 動きに応じて「IsRunning」Boolを切り替える
    public void SetMovementAnimation(float direction)
    {
        if (animator == null) return;

        // directionが0でなければ走り
        bool isMoving = direction != 0;
        animator.SetBool("run", isMoving);
    }

    // 忍術のアニメーション制御 (攻撃アニメーションを流用)
    public void TriggerNinjutsuAttackAnimation()
    {
        if (animator == null) return;

        animator.SetTrigger("attack");
    }

    // --- PlayerInputからの動作要求の窓口（委譲） ---

    public void HandleMoveInput(float direction) => movement?.Move(direction);

    public void HandleJumpInput() => movement?.Jump();

    public void HandleAttackInput() => attacker?.ThrowAttack();

    public void HandleUseNinjutsuComboOrSingleInput() => ninjutsuHandler?.UseNinjutsuComboOrSingle();

    public void HandleUseAllRemainingNinjutsuInput() => ninjutsuHandler?.UseAllRemainingNinjutsu();

    public void HandleGenerateAndSetRandomNinjutsuInput() => ninjutsuHandler?.GenerateAndSetRandomNinjutsu();

}