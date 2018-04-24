
using UnityEngine;

namespace Game
{
    public class EnemyHealth : AbstractHealthComponent
    {
        public EnemyDefinition enemyDefinition;
        public float health;

        public void Awake()
        {
            health = enemyDefinition.health;
        }

        public override float MaxHealth()
        {
            return enemyDefinition.health;
        }

        public override float CurrentHealth()
        {
            return health;
        }

        public override void ApplyDamage(float damage)
        {
            health -= damage;
        }
    }
}
