
using UnityEngine;
using System;

namespace Game
{
    [Serializable]
    public class PlayerState
    {
        public int level;
        public int experience;
        public float health;

        public void Init(PlayerDefinition playerDefinition)
        {
            level = 1;
            experience = 0;
            health = playerDefinition.MaxHealth(level);
        }
    }
}
