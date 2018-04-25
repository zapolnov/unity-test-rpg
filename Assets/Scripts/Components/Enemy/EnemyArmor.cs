
using UnityEngine;

namespace Game
{
    public class EnemyArmor : AbstractArmor
    {
        public EnemyDefinition enemyDefinition;

        public override float CurrentArmor()
        {
            return enemyDefinition.armor;
        }
    }
}
