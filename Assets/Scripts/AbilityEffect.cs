using UnityEngine;

namespace NinjaGame
{
    public abstract class AbilityEffect : MonoBehaviour
    {
        public abstract void Configure(NinjutsuAbilityId abilityId, AbilityContext context);
    }
}
