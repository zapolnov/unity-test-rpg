﻿
using UnityEngine;

namespace Game
{
    [CreateAssetMenu]
    public class PlayerDefinition : ScriptableObject
    {
        public float strafeSpeed;
        public float forwardSpeed;

        public float baseHealth;
        public float baseArmor;
        public float baseDamage;
        public float healthMultiplier;
        public float armorMultiplier;
        public float damageMultiplier;

        public float MaxHealth(int level) { return baseHealth + healthMultiplier * (level - 1); }
        public float MaxArmor(int level) { return baseArmor + armorMultiplier * (level - 1); }
        public float MaxDamage(int level) { return baseDamage + damageMultiplier * (level - 1); }
    }
}
