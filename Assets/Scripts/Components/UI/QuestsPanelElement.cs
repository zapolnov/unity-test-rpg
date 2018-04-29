
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class QuestsPanelElement : MonoBehaviour
    {
        public Text title;
        public Text info;
        public bool isCompleted { get; set; }

        public void Init(Quest quest, bool completed)
        {
            isCompleted = completed;
            title.text = quest.goalDescription;
            if (completed) {
                info.text = "Completed!";
                info.color = Color.green;
            } else {
                info.text = string.Format("Reward: +{0} experience", quest.givesExp);
                info.color = Color.yellow;
            }
        }
    }
}
