
using UnityEngine;

namespace Game
{
    public class QuestGiver : AbstractInteractable
    {
        public GameObject exclamation;
        public Quest quest;
        public AbstractQuestElement startQuestElement;
        public AbstractQuestElement endQuestElement;

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
            }
        }
    }
}
