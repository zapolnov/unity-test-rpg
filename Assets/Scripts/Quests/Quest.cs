
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Game
{
    [CreateAssetMenu]
    public class Quest : ScriptableObject
    {
        public string goalDescription;
        public int givesExp;
        public List<CollectibleDefinition> itemsToCollect = new List<CollectibleDefinition>();
        public Dictionary<string, AbstractEnemy> enemiesToKill = new Dictionary<string, AbstractEnemy>();

        public bool GoalsAchieved()
        {
            foreach (var enemy in enemiesToKill) {
                if (enemy.Value == null || !enemy.Value.IsDead())
                    return false;
            }

            var inventory = GameController.Instance.inventory;
            foreach (var item in itemsToCollect) {
                if (!inventory.ContainsKey(item))
                    return false;
            }

            return true;
        }
    }
}
