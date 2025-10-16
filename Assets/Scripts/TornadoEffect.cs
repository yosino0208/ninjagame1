using UnityEngine;

namespace NinjaGame
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class TornadoEffect : MonoBehaviour
    {
        [SerializeField] private float duration = 20f;
        [SerializeField] private float damagePerSecond = 0.35f;
        [SerializeField] private float width = 3f;
        [SerializeField] private float moveSpeed = 2f;

        private float timeAlive;
        private BoxCollider2D boxCollider;

        public Transform Owner { get; set; }
        public float Duration { get => duration; set => duration = value; }
        public float Width { get => width; set => width = value; }
        public Vector2 Direction { get; set; } = Vector2.right;

        private void Awake()
        {
            boxCollider = GetComponent<BoxCollider2D>();
            boxCollider.isTrigger = true;
        }

        private void OnEnable()
        {
            transform.localScale = new Vector3(width, transform.localScale.y, transform.localScale.z);
        }

        private void Update()
        {
            timeAlive += Time.deltaTime;
            if (timeAlive >= duration)
            {
                Destroy(gameObject);
                return;
            }

            Vector3 displacement = (Vector3)Direction.normalized * moveSpeed * Time.deltaTime;
            transform.Translate(displacement, Space.World);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (Owner != null && other.transform == Owner)
            {
                return;
            }

            if (!other.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                return;
            }

            damageable.TakeDamage(damagePerSecond * Time.deltaTime);
        }
    }
}
