using System;
using System.Collections.Generic;
using UnityEngine;

namespace NinjaGame
{
    [DisallowMultipleComponent]
    public class NinjutsuManager : MonoBehaviour
    {
        [Serializable]
        public class AbilityPrefab
        {
            public NinjutsuAbilityId abilityId;
            public GameObject prefab;
        }

        private readonly struct ElementCount
        {
            public readonly NinjutsuElement element;
            public readonly int count;

            public ElementCount(NinjutsuElement element, int count)
            {
                this.element = element;
                this.count = count;
            }
        }

        [Header("Scroll Slots")]
        [SerializeField] private int slotCount = 3;
        [SerializeField] private NinjutsuSlotUI slotUI;

        [Header("Ability Prefabs")]
        [SerializeField] private List<AbilityPrefab> abilityPrefabs = new();

        [Header("Ability Parameters")]
        [SerializeField] private float fireFireRadius = 5f;
        [SerializeField] private int fireFireHits = 3;
        [SerializeField] private float fireSingleRadius = 7f;
        [SerializeField] private float fireballSpeed = 18f;
        [SerializeField] private float tornadoDuration = 20f;
        [SerializeField] private float tornadoWidth = 3f;
        [SerializeField] private float windShieldRadius = 2f;
        [SerializeField] private float waterArrowSpeed = 10f;
        [SerializeField] private float waterArrowSpread = 12f;

        private readonly List<NinjutsuElement> slots = new();
        private readonly Dictionary<NinjutsuAbilityId, GameObject> prefabLookup = new();
        private readonly Dictionary<NinjutsuElement, int> generationBalance = new();

        public event Action<IReadOnlyList<NinjutsuElement>> OnSlotsChanged;

        private void Awake()
        {
            slots.Capacity = Mathf.Max(1, slotCount);
            foreach (NinjutsuElement element in Enum.GetValues(typeof(NinjutsuElement)))
            {
                generationBalance[element] = 0;
            }

            foreach (AbilityPrefab ability in abilityPrefabs)
            {
                if (ability.prefab != null)
                {
                    prefabLookup[ability.abilityId] = ability.prefab;
                }
            }
        }

        private void Start()
        {
            RefreshUI();
        }

        public void ReloadScroll()
        {
            while (slots.Count < slotCount)
            {
                NinjutsuElement element = GetBalancedRandomElement();
                slots.Add(element);
            }
            RefreshUI();
        }

        public bool TryActivate(Vector3 origin, bool facingRight)
        {
            if (slots.Count == 0)
            {
                return false;
            }

            if (TryActivateCombo(origin, facingRight))
            {
                return true;
            }

            return TryActivateSingle(origin, facingRight);
        }

        private bool TryActivateCombo(Vector3 origin, bool facingRight)
        {
            if (slots.Count < 2)
            {
                return false;
            }

            NinjutsuElement first = slots[0];
            NinjutsuElement second = slots[1];

            NinjutsuAbilityId abilityId;

            if (IsPair(first, second, NinjutsuElement.Fire, NinjutsuElement.Fire))
            {
                abilityId = NinjutsuAbilityId.FireFireBarrier;
            }
            else if (IsPair(first, second, NinjutsuElement.Water, NinjutsuElement.Wind))
            {
                abilityId = NinjutsuAbilityId.WaterWindTornado;
            }
            else if (IsPair(first, second, NinjutsuElement.Wind, NinjutsuElement.Fire))
            {
                abilityId = NinjutsuAbilityId.WindFireFireball;
            }
            else
            {
                return false;
            }

            slots.RemoveAt(0);
            slots.RemoveAt(0);
            ConsumeElement(first);
            ConsumeElement(second);
            ActivateAbility(abilityId, origin, facingRight);
            RefreshUI();
            return true;
        }

        private bool TryActivateSingle(Vector3 origin, bool facingRight)
        {
            NinjutsuElement element = slots[0];
            NinjutsuAbilityId abilityId;
            switch (element)
            {
                case NinjutsuElement.Fire:
                    abilityId = NinjutsuAbilityId.FireExplosion;
                    break;
                case NinjutsuElement.Water:
                    abilityId = NinjutsuAbilityId.WaterArrows;
                    break;
                case NinjutsuElement.Wind:
                    abilityId = NinjutsuAbilityId.WindShield;
                    break;
                default:
                    return false;
            }

            slots.RemoveAt(0);
            ConsumeElement(element);
            ActivateAbility(abilityId, origin, facingRight);
            RefreshUI();
            return true;
        }

        private void ActivateAbility(NinjutsuAbilityId abilityId, Vector3 origin, bool facingRight)
        {
            if (!prefabLookup.TryGetValue(abilityId, out GameObject prefab) || prefab == null)
            {
                SpawnRuntimeAbility(abilityId, origin, facingRight);
                return;
            }

            Quaternion rotation = facingRight ? Quaternion.identity : Quaternion.Euler(0f, 180f, 0f);
            GameObject instance = Instantiate(prefab, origin, rotation);
            ConfigureInstance(instance, abilityId, facingRight);
        }

        private void SpawnRuntimeAbility(NinjutsuAbilityId abilityId, Vector3 origin, bool facingRight)
        {
            switch (abilityId)
            {
                case NinjutsuAbilityId.FireFireBarrier:
                    CreateAreaPulse(origin, fireFireRadius, fireFireHits, 0.35f, 1f, 1f);
                    break;
                case NinjutsuAbilityId.WaterWindTornado:
                    CreateTornado(origin, facingRight);
                    break;
                case NinjutsuAbilityId.WindFireFireball:
                    CreateFireball(origin, facingRight, fireballSpeed * (facingRight ? 1f : -1f));
                    break;
                case NinjutsuAbilityId.FireExplosion:
                    CreateAreaPulse(origin, fireSingleRadius, 1, 0f, 1.5f, 2.5f);
                    break;
                case NinjutsuAbilityId.WaterArrows:
                    LaunchWaterArrows(origin, facingRight);
                    break;
                case NinjutsuAbilityId.WindShield:
                    CreateWindShield(origin);
                    break;
            }
        }

        private void ConfigureInstance(GameObject instance, NinjutsuAbilityId abilityId, bool facingRight)
        {
            AbilityEffect effect = instance.GetComponent<AbilityEffect>();
            if (effect != null)
            {
                AbilityContext context = new AbilityContext
                {
                    Origin = instance.transform.position,
                    FacingRight = facingRight,
                    FireFireRadius = fireFireRadius,
                    FireFireHits = fireFireHits,
                    FireballSpeed = fireballSpeed,
                    FireSingleRadius = fireSingleRadius,
                    TornadoDuration = tornadoDuration,
                    TornadoWidth = tornadoWidth,
                    WindShieldRadius = windShieldRadius,
                    WaterArrowSpeed = waterArrowSpeed,
                    WaterArrowSpread = waterArrowSpread
                };
                effect.Configure(abilityId, context);
            }
        }

        private void CreateAreaPulse(Vector3 origin, float radius, int pulses, float delayBetweenPulses, float lifetime, float damagePerPulse)
        {
            GameObject obj = new("NinjutsuAreaPulse");
            obj.transform.position = origin;
            var area = obj.AddComponent<AreaDamageEmitter>();
            area.Radius = radius;
            area.PulseCount = pulses;
            area.DelayBetweenPulses = Mathf.Max(0.01f, delayBetweenPulses);
            area.Lifetime = Mathf.Max(lifetime, pulses * delayBetweenPulses);
            area.Owner = transform;
            area.DamagePerPulse = damagePerPulse;
        }

        private void CreateTornado(Vector3 origin, bool facingRight)
        {
            GameObject obj = new("NinjutsuTornado");
            obj.transform.position = origin + Vector3.right * (facingRight ? 1.5f : -1.5f);
            var tornado = obj.AddComponent<TornadoEffect>();
            tornado.Duration = tornadoDuration;
            tornado.Width = tornadoWidth;
            tornado.Owner = transform;
            tornado.Direction = facingRight ? Vector2.right : Vector2.left;
        }

        private void CreateFireball(Vector3 origin, bool facingRight, float velocityX)
        {
            GameObject obj = new("NinjutsuFireball");
            obj.transform.position = origin;
            var projectile = obj.AddComponent<Projectile>();
            projectile.Speed = Mathf.Abs(velocityX);
            projectile.Direction = facingRight ? Vector2.right : Vector2.left;
            projectile.Owner = transform;
            projectile.Lifetime = 4f;
            projectile.Damage = 1f;
        }

        private void LaunchWaterArrows(Vector3 origin, bool facingRight)
        {
            float directionMultiplier = facingRight ? 1f : -1f;
            float[] angles = { 0f, waterArrowSpread, -waterArrowSpread };
            foreach (float angle in angles)
            {
                GameObject obj = new("NinjutsuWaterArrow");
                obj.transform.position = origin;
                var projectile = obj.AddComponent<Projectile>();
                projectile.Speed = waterArrowSpeed;
                Vector2 dir = Quaternion.Euler(0f, 0f, angle) * Vector2.right * directionMultiplier;
                projectile.Direction = dir.normalized;
                projectile.Owner = transform;
                projectile.Lifetime = 3f;
                projectile.Damage = 0.5f;
                projectile.IsHoming = true;
            }
        }

        private void CreateWindShield(Vector3 origin)
        {
            GameObject obj = new("NinjutsuWindShield");
            obj.transform.position = origin;
            var shield = obj.AddComponent<WindShield>();
            shield.Radius = windShieldRadius;
            shield.Owner = transform;
        }

        private bool IsPair(NinjutsuElement first, NinjutsuElement second, NinjutsuElement expectedA, NinjutsuElement expectedB)
        {
            return (first == expectedA && second == expectedB) || (first == expectedB && second == expectedA);
        }

        private NinjutsuElement GetBalancedRandomElement()
        {
            ElementCount[] counts =
            {
                new ElementCount(NinjutsuElement.Fire, generationBalance[NinjutsuElement.Fire]),
                new ElementCount(NinjutsuElement.Water, generationBalance[NinjutsuElement.Water]),
                new ElementCount(NinjutsuElement.Wind, generationBalance[NinjutsuElement.Wind])
            };

            int min = int.MaxValue;
            foreach (ElementCount entry in counts)
            {
                min = Mathf.Min(min, entry.count);
            }

            List<NinjutsuElement> candidates = new();
            foreach (ElementCount entry in counts)
            {
                if (entry.count == min)
                {
                    candidates.Add(entry.element);
                }
            }

            NinjutsuElement selected = candidates[UnityEngine.Random.Range(0, candidates.Count)];
            generationBalance[selected]++;
            return selected;
        }

        private void RefreshUI()
        {
            OnSlotsChanged?.Invoke(slots);
            if (slotUI != null)
            {
                slotUI.UpdateSlots(slots, slotCount);
            }
        }

        private void ConsumeElement(NinjutsuElement element)
        {
            if (!generationBalance.ContainsKey(element))
            {
                return;
            }

            generationBalance[element] = Mathf.Max(0, generationBalance[element] - 1);
        }
    }
}
