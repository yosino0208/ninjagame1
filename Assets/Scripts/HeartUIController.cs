using System.Collections.Generic;
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

    public class NinjutsuSlotUI : MonoBehaviour
    {
        [SerializeField] private Image[] slotImages;
        [SerializeField] private Sprite fireSprite;
        [SerializeField] private Sprite waterSprite;
        [SerializeField] private Sprite windSprite;
        [SerializeField] private Color emptyColor = new(1f, 1f, 1f, 0.25f);

        private readonly Dictionary<NinjutsuElement, Sprite> spriteLookup = new();

        private void Awake()
        {
            spriteLookup[NinjutsuElement.Fire] = fireSprite;
            spriteLookup[NinjutsuElement.Water] = waterSprite;
            spriteLookup[NinjutsuElement.Wind] = windSprite;
        }

        public void UpdateSlots(IReadOnlyList<NinjutsuElement> elements, int maxSlots)
        {
            EnsureImages(maxSlots);

            for (int i = 0; i < slotImages.Length; i++)
            {
                Image image = slotImages[i];
                if (image == null)
                {
                    continue;
                }

                if (i < elements.Count)
                {
                    NinjutsuElement element = elements[i];
                    if (spriteLookup.TryGetValue(element, out Sprite sprite))
                    {
                        image.sprite = sprite;
                        image.color = Color.white;
                    }
                }
                else
                {
                    image.sprite = null;
                    image.color = emptyColor;
                }
            }
        }

        private void EnsureImages(int maxSlots)
        {
            if (slotImages == null || slotImages.Length == 0)
            {
                slotImages = GetComponentsInChildren<Image>();
            }

            if (slotImages.Length < maxSlots)
            {
                System.Array.Resize(ref slotImages, maxSlots);
            }
        }
    }
}

