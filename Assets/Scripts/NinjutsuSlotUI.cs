using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NinjaGame
{
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
