
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

namespace Game
{
    public class NotificationsPanel : MonoBehaviour
    {
        [Serializable]
        public class Message
        {
            public string text;
            public float timeLeft;
        }

        public Text[] notifications;
        public float messageDisappearTime;

        private List<Message> mMessages = new List<Message>();
        private bool mMessagesChanged;

        void Awake()
        {
            foreach (var notification in notifications)
                notification.gameObject.SetActive(false);
        }

        public void AddMessage(string text)
        {
            Message message = new Message();
            message.text = text;
            message.timeLeft = messageDisappearTime;
            mMessages.Add(message);
            mMessagesChanged = true;
        }

        void Update()
        {
            for (int i = 0; i < mMessages.Count && i < notifications.Length; ) {
                mMessages[i].timeLeft -= Time.deltaTime;
                if (mMessages[i].timeLeft > 0.0f)
                    ++i;
                else {
                    mMessages.RemoveAt(i);
                    mMessagesChanged = true;
                }
            }

            mMessagesChanged = true;
            if (mMessagesChanged) {
                int i = 0;

                for (; i < mMessages.Count && i < notifications.Length; i++) {
                    notifications[i].gameObject.SetActive(true);
                    notifications[i].text = mMessages[i].text;

                    var c = notifications[i].color;
                    c.a = mMessages[i].timeLeft / messageDisappearTime;
                    notifications[i].color = c;
                }

                for (; i < notifications.Length; i++)
                    notifications[i].gameObject.SetActive(false);

                mMessagesChanged = false;
            }
        }
    }
}
