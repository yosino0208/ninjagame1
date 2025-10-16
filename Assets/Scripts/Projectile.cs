using UnityEngine;

namespace NinjaGame
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed = 10f;
        [SerializeField] private Vector2 direction = Vector2.right;
        [SerializeField] private float damage = 1f;
        [SerializeField] private float lifetime = 4f;
        [SerializeField] private bool destroyOnImpact = true;
        [SerializeField] private bool isHoming;
        [SerializeField] private float homingRotationSpeed = 360f;
        [SerializeField] private float homingSearchRadius = 12f;

        private float timeAlive;
        public Transform Owner { get; set; }
        public float Speed { get => speed; set => speed = value; }
        public Vector2 Direction { get => direction; set => direction = value.normalized; }
        public float Damage { get => damage; set => damage = value; }
        public float Lifetime { get => lifetime; set => lifetime = value; }
        public bool IsHoming { get => isHoming; set => isHoming = value; }

        private void Awake()
        {
            Rigidbody2D body = gameObject.AddComponent<Rigidbody2D>();
            body.isKinematic = true;
            CircleCollider2D collider = GetComponent<CircleCollider2D>();
            collider.isTrigger = true;
            collider.radius = 0.25f;
        }

        private void Update()
        {
            timeAlive += Time.deltaTime;
            if (timeAlive >= lifetime)
            {
                Destroy(gameObject);
                return;
            }

            if (isHoming)
            {
                UpdateHomingDirection();
            }

            Vector3 displacement = (Vector3)direction.normalized * speed * Time.deltaTime;
            transform.Translate(displacement, Space.World);
        }

        private void UpdateHomingDirection()
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, homingSearchRadius);
            Transform bestTarget = null;
            float bestScore = float.MaxValue;
            foreach (Collider2D hit in hits)
            {
                if (hit.transform == Owner)
                {
                    continue;
                }

                if (!hit.TryGetComponent<IDamageable>(out _))
                {
                    continue;
                }

                float score = Vector2.Distance(hit.transform.position, transform.position);
                if (score < bestScore)
                {
                    bestScore = score;
                    bestTarget = hit.transform;
                }
            }

            if (bestTarget == null)
            {
                return;
            }

            Vector2 toTarget = (bestTarget.position - transform.position).normalized;
            float angle = Vector3.SignedAngle(direction, toTarget, Vector3.forward);
            float rotationStep = Mathf.Sign(angle) * Mathf.Min(Mathf.Abs(angle), homingRotationSpeed * Time.deltaTime);
            direction = Quaternion.Euler(0f, 0f, rotationStep) * direction;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (Owner != null && other.transform == Owner)
            {
                return;
            }

            if (!other.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                return;
            }

            damageable.TakeDamage(damage);

            if (destroyOnImpact)
            {
                Destroy(gameObject);
            }
        }
    }
}
