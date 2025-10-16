using UnityEngine;

namespace NinjaGame
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class WindShield : MonoBehaviour
    {
        [SerializeField] private float radius = 2f;
        [SerializeField] private float damagePerSecond = 0.5f;
        [SerializeField] private float lifetime = 8f;
        [SerializeField] private float rotationSpeed = 90f;

        public Transform Owner { get; set; }
        public float Radius { get => radius; set => radius = value; }

        private void Awake()
        {
            CircleCollider2D collider = GetComponent<CircleCollider2D>();
            collider.isTrigger = true;
        }

        private void Start()
        {
            if (Owner != null)
            {
                transform.SetParent(Owner);
                transform.localPosition = Vector3.zero;
            }
            transform.localScale = Vector3.one * radius;
            Destroy(gameObject, lifetime);
        }

        private void Update()
        {
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
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
