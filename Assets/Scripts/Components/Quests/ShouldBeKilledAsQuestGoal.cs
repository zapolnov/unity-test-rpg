
using UnityEngine;
using System;

namespace Game
{
    public class ShouldBeKilledAsQuestGoal : MonoBehaviour
    {
        public Quest quest;

        void Awake()
        {
            AbstractEnemy enemy = GetComponent<AbstractEnemy>();
            if (enemy == null)
                throw new InvalidOperationException("Not an enemy.");
            quest.enemiesToKill.Add(enemy);
        }
    }
}
