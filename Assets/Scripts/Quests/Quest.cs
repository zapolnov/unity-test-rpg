
using UnityEngine;
using System.Collections.Generic;

namespace Game
{
    [CreateAssetMenu]
    public class Quest : ScriptableObject
    {
        public string goalDescription;
        public int givesExp;
        public List<CollectibleDefinition> itemsToCollect = new List<CollectibleDefinition>();
        public HashSet<AbstractEnemy> enemiesToKill = new HashSet<AbstractEnemy>();

        private bool mCompleted;

        public bool GoalsAchieved()
        {
            if (mCompleted)
                return true;

            foreach (var enemy in enemiesToKill) {
                if (enemy != null && !enemy.IsDead())
                    return false;
            }

            var inventory = GameController.Instance.inventory;
            foreach (var item in itemsToCollect) {
                if (!inventory.ContainsKey(item))
                    return false;
            }

            mCompleted = true;
            return true;
        }
    }
}
