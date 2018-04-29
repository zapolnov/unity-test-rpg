
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Game
{
    [RequireComponent(typeof(UniqueId))]
    public class QuestGiver : AbstractInteractable
    {
        [Serializable]
        public struct SavedState
        {
            public bool allFinished;
        }

        public GameObject exclamation;
        public Quest quest;
        public AbstractQuestElement startQuestElement;
        public AbstractQuestElement endQuestElement;

        private UniqueId mUniqueId;

        void Start()
        {
            mUniqueId = GetComponent<UniqueId>();

            Dictionary<string, QuestGiver.SavedState> questGivers = GameController.Instance.questGivers;
            if (questGivers.ContainsKey(mUniqueId.guid)) {
                SavedState state = questGivers[mUniqueId.guid];
                if (state.allFinished)
                    endQuestElement = null;
            }
        }

        void Update()
        {
            exclamation.SetActive(CanInteract());
        }

        public override bool CanInteract()
        {
            if (quest == null)
                return false;

            var gameController = GameController.Instance;
            if (gameController.activeQuests.Contains(quest))
                return false;

            if (gameController.completedQuests.Contains(quest))
                return endQuestElement != null;
            else
                return startQuestElement != null;
        }

        public override void Interact()
        {
            var gameController = GameController.Instance;
            if (gameController.activeQuests.Contains(quest))
                return;

            if (!gameController.completedQuests.Contains(quest))
                startQuestElement.Run();
            else {
                endQuestElement.Run();
                endQuestElement = null;

                SavedState s = new SavedState();
                s.allFinished = true;
                GameController.Instance.questGivers[mUniqueId.guid] = s;
            }
        }
    }
}
