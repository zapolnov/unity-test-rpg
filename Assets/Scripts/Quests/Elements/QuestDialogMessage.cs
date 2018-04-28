
using UnityEngine;

namespace Game
{
    [CreateAssetMenu]
    public class QuestDialogMessage : AbstractQuestElement
    {
        public AbstractQuestElement nextQuestElement;
        [Multiline] public string message;

        public override void Run()
        {
            GameController.Instance.DisplayDialogMessage(message, nextQuestElement);
        }
    }
}
