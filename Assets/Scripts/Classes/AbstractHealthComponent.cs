
using UnityEngine;

namespace Game
{
    public abstract class AbstractHealthComponent : MonoBehaviour
    {
        public abstract float MaxHealth();
        public abstract float CurrentHealth();
    }
}
