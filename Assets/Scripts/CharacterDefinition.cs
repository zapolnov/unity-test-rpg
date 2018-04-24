
using UnityEngine;

namespace Game
{
    [CreateAssetMenu]
    public class CharacterDefinition : ScriptableObject
    {
        public int level;
        public float health;
        public float armor;
        public float damage;
        public float givesExp;
    }
}
