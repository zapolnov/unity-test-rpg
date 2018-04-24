
using UnityEngine;
using System;

namespace Game
{
    [Serializable]
    public class PlayerState
    {
        public int level;
        public float health;

        public void Init(PlayerDefinition playerDefinition)
        {
            level = 1;
            health = playerDefinition.MaxHealth(level);
        }
    }
}
