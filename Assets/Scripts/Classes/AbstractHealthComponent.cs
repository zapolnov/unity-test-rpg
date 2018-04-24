
using UnityEngine;

namespace Game
{
    public abstract class AbstractHealthComponent : MonoBehaviour
    {
        public abstract float MaxHealth();
        public abstract float CurrentHealth();

        public abstract void ApplyDamage(float damage);
    }
}
