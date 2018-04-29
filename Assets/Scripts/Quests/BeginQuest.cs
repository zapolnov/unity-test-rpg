
using UnityEngine;

namespace Game
{
    [CreateAssetMenu]
    public class BeginQuest : AbstractQuestElement
    {
        public AbstractQuestElement nextQuestElement;
        public Quest quest;

        public override void Run()
        {
            GameController.Instance.BeginQuest(quest);
            if (nextQuestElement != null)
                nextQuestElement.Run();
        }
    }
}
