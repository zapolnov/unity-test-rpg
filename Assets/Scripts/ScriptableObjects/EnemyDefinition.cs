
using UnityEngine;

namespace Game
{
    [CreateAssetMenu]
    public class EnemyDefinition : ScriptableObject
    {
        public float health;
        public float armor;
        public float damage;
        public float givesExp;
    }
}
