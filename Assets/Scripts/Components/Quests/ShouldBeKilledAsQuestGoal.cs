
using UnityEngine;
using System;

namespace Game
{
    public class ShouldBeKilledAsQuestGoal : MonoBehaviour
    {
        public Quest quest;

        void Start()
        {
            AbstractEnemy enemy = GetComponent<AbstractEnemy>();
            if (enemy == null)
                throw new InvalidOperationException("Not an enemy.");
            quest.enemiesToKill[enemy.UniqueId] = enemy;
        }
    }
}
