
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

namespace Game
{
    public class QuestsPanel : MonoBehaviour
    {
        public ScrollRect scrollRect;
        public QuestsPanelElement itemPrefab;

        public void FillContents()
        {
            ClearContents();

            List<QuestsPanelElement> items = new List<QuestsPanelElement>();
            foreach (var quest in GameController.Instance.activeQuests) {
                var item = Instantiate(itemPrefab.gameObject).GetComponent<QuestsPanelElement>();
                item.Init(quest, false);
                items.Add(item);
            }
            foreach (var quest in GameController.Instance.completedQuests) {
                var item = Instantiate(itemPrefab.gameObject).GetComponent<QuestsPanelElement>();
                item.Init(quest, true);
                items.Add(item);
            }

            items.OrderBy(it => it.isCompleted).ThenBy(it => it.title.text);

            var contentTransform = scrollRect.content.GetComponent<RectTransform>();
            var contentSize = contentTransform.sizeDelta;

            float y = 0;
            foreach (var item in items) {
                var transform = item.GetComponent<RectTransform>();
                float height = transform.sizeDelta.y;

                contentSize.y = y + height + 5;
                contentTransform.sizeDelta = contentSize;

                item.transform.SetParent(scrollRect.content.transform, false);

                var pos = transform.anchoredPosition;
                pos.y = -y;
                transform.anchoredPosition = pos;

                y += height;
            }
        }

        public void ClearContents()
        {
            foreach (Transform child in scrollRect.content.transform)
                Destroy(child.gameObject);
        }
    }
}
