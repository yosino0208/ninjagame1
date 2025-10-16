using UnityEngine;
using UnityEngine.UI;

namespace NinjaGame
{
    public class HeartUIController : MonoBehaviour
    {
        [SerializeField] private Image[] heartImages;
        [SerializeField] private Sprite fullHeart;
        [SerializeField] private Sprite emptyHeart;
        [SerializeField] private Health health;

        private void Awake()
        {
            if (health == null)
            {
                health = FindObjectOfType<Health>();
            }

            if (health != null)
            {
                health.OnHealthChanged += HandleHealthChanged;
                HandleHealthChanged(health.CurrentHearts, health.MaxHearts);
            }
        }

        private void OnDestroy()
        {
            if (health != null)
            {
                health.OnHealthChanged -= HandleHealthChanged;
            }
        }

        private void HandleHealthChanged(int current, int max)
        {
            EnsureHearts(max);
            for (int i = 0; i < heartImages.Length; i++)
            {
                Image image = heartImages[i];
                if (image == null)
                {
                    continue;
                }

                image.sprite = i < current ? fullHeart : emptyHeart;
                image.enabled = i < max;
            }
        }

        private void EnsureHearts(int max)
        {
            if (heartImages != null && heartImages.Length >= max)
            {
                return;
            }

            heartImages = GetComponentsInChildren<Image>();
        }
    }
}
