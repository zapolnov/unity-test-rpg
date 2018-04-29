
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Game
{
    [CreateAssetMenu]
    public class QuestDialogChoice : AbstractQuestElement
    {
        [Serializable]
        public struct Option
        {
            public string choice;
            public AbstractQuestElement nextQuestElement;
        }

        [Multiline] public string message;
        public List<Option> options;

        public override void Run()
        {
            GameController.Instance.DisplayDialogChoice(message, options.AsReadOnly());
        }
    }
}
