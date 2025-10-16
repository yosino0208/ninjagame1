using System.Collections;
using UnityEngine;

namespace NinjaGame
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class AreaDamageEmitter : MonoBehaviour
    {
        [SerializeField] private float radius = 5f;
        [SerializeField] private int pulseCount = 3;
        [SerializeField] private float delayBetweenPulses = 0.35f;
        [SerializeField] private float lifetime = 1f;
        [SerializeField] private float damagePerPulse = 1f;

        private CircleCollider2D areaCollider;

        public Transform Owner { get; set; }
        public float Radius { get => radius; set => radius = value; }
        public int PulseCount { get => pulseCount; set => pulseCount = Mathf.Max(1, value); }
        public float DelayBetweenPulses { get => delayBetweenPulses; set => delayBetweenPulses = value; }
        public float Lifetime { get => lifetime; set => lifetime = value; }
        public float DamagePerPulse { get => damagePerPulse; set => damagePerPulse = value; }

        private void Awake()
        {
            areaCollider = GetComponent<CircleCollider2D>();
            areaCollider.isTrigger = true;
        }

        private void OnEnable()
        {
            areaCollider.radius = radius;
            transform.localScale = Vector3.one * radius;
            StartCoroutine(PulseDamage());
            Destroy(gameObject, lifetime);
        }

        private IEnumerator PulseDamage()
        {
            for (int i = 0; i < pulseCount; i++)
            {
                Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);
                foreach (Collider2D hit in hits)
                {
                    if (Owner != null && hit.transform == Owner)
                    {
                        continue;
                    }

                    if (hit.TryGetComponent<IDamageable>(out IDamageable damageable))
                    {
                        damageable.TakeDamage(damagePerPulse);
                    }
                }

                if (delayBetweenPulses > 0.001f)
                {
                    yield return new WaitForSeconds(delayBetweenPulses);
                }
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1f, 0.4f, 0f, 0.3f);
            Gizmos.DrawSphere(transform.position, radius);
        }
#endif
    }
}
