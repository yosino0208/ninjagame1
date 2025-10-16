using System;
using UnityEngine;

namespace NinjaGame
{
    public class Health : MonoBehaviour, IDamageable
    {
        [SerializeField] private int maxHearts = 5;
        [SerializeField] private int currentHearts;
        [SerializeField] private bool autoInitialize = true;

        public event Action<int, int> OnHealthChanged;
        public event Action OnDeath;

        public int MaxHearts => maxHearts;
        public int CurrentHearts => currentHearts;

        private void Awake()
        {
            if (autoInitialize)
            {
                currentHearts = Mathf.Clamp(currentHearts, 0, maxHearts);
                if (currentHearts == 0)
                {
                    currentHearts = maxHearts;
                }
            }
        }

        public void TakeDamage(float amount)
        {
            int damage = Mathf.CeilToInt(amount);
            ApplyDamage(damage);
        }

        public void ApplyDamage(int hearts)
        {
            if (hearts <= 0 || currentHearts <= 0)
            {
                return;
            }

            currentHearts = Mathf.Max(0, currentHearts - hearts);
            OnHealthChanged?.Invoke(currentHearts, maxHearts);
            if (currentHearts <= 0)
            {
                OnDeath?.Invoke();
            }
        }

        public void Heal(int hearts)
        {
            if (hearts <= 0)
            {
                return;
            }

            currentHearts = Mathf.Clamp(currentHearts + hearts, 0, maxHearts);
            OnHealthChanged?.Invoke(currentHearts, maxHearts);
        }

        public void SetMaxHearts(int hearts, bool refill = true)
        {
            maxHearts = Mathf.Max(1, hearts);
            if (refill)
            {
                currentHearts = maxHearts;
            }
            else
            {
                currentHearts = Mathf.Clamp(currentHearts, 0, maxHearts);
            }

            OnHealthChanged?.Invoke(currentHearts, maxHearts);
        }
    }
}
