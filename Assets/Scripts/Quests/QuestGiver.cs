
using UnityEngine;

namespace Game
{
    public class QuestGiver : AbstractInteractable
    {
        public AbstractQuestElement startQuestElement;

        public override void Interact()
        {
            startQuestElement.Run();
        }
    }
}
