
using UnityEngine;
using System.Collections.Generic;

namespace Game
{
    [CreateAssetMenu]
    public class Quest : ScriptableObject
    {
        public string goalDescription;
        public int givesExp;
        public HashSet<AbstractEnemy> enemiesToKill = new HashSet<AbstractEnemy>();

        public bool GoalsAchieved()
        {
            foreach (var enemy in enemiesToKill) {
                if (enemy != null && !enemy.IsDead())
                    return false;
            }

            return true;
        }
    }
}
