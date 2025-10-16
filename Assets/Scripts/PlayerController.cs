using System.Collections.Generic;
using UnityEngine;

namespace NinjaGame
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Health))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 7f;
        [SerializeField] private float acceleration = 15f;
        [SerializeField] private float jumpForce = 13f;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private float groundCheckRadius = 0.1f;
        [SerializeField] private LayerMask groundLayer;

        [Header("Combat")]
        [SerializeField] private Transform projectileSpawnPoint;
        [SerializeField] private NinjutsuManager ninjutsuManager;

        private Rigidbody2D body;
        private Vector2 desiredVelocity;
        private bool jumpRequest;
        private bool isGrounded;
        private bool facingRight = true;

        private readonly List<Collider2D> groundContacts = new();

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            if (ninjutsuManager == null)
            {
                ninjutsuManager = GetComponent<NinjutsuManager>();
            }
        }

        private void Update()
        {
            ReadMovementInput();
            ReadCombatInput();
        }

        private void FixedUpdate()
        {
            UpdateGroundedState();
            ApplyMovement();
            TryJump();
        }

        private void ReadMovementInput()
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float targetVelocityX = horizontalInput * moveSpeed;
            desiredVelocity.x = Mathf.MoveTowards(body.velocity.x, targetVelocityX, acceleration * Time.deltaTime);
            desiredVelocity.y = body.velocity.y;

            if (horizontalInput > 0.01f)
            {
                facingRight = true;
            }
            else if (horizontalInput < -0.01f)
            {
                facingRight = false;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                jumpRequest = true;
            }
        }

        private void ReadCombatInput()
        {
            if (ninjutsuManager == null)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                ninjutsuManager.ReloadScroll();
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Vector3 origin = projectileSpawnPoint != null ? projectileSpawnPoint.position : transform.position;
                ninjutsuManager.TryActivate(origin, facingRight);
            }
        }

        private void ApplyMovement()
        {
            body.velocity = new Vector2(desiredVelocity.x, body.velocity.y);
        }

        private void TryJump()
        {
            if (!jumpRequest)
            {
                return;
            }

            jumpRequest = false;
            if (!isGrounded)
            {
                return;
            }

            body.velocity = new Vector2(body.velocity.x, 0f);
            body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        private void UpdateGroundedState()
        {
            if (groundCheck == null)
            {
                isGrounded = body.velocity.y == 0f;
                return;
            }

            Collider2D[] hits = Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRadius, groundLayer);
            groundContacts.Clear();
            groundContacts.AddRange(hits);
            isGrounded = groundContacts.Count > 0;
        }

        public void TeleportTo(Vector3 position)
        {
            body.position = position;
            body.velocity = Vector2.zero;
        }

        public bool IsFacingRight() => facingRight;
    }
}
