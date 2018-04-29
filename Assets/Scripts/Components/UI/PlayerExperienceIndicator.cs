
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class PlayerExperienceIndicator : MonoBehaviour
    {
        private Text mText;

        void Awake()
        {
            mText = GetComponent<Text>();
        }

        void Update()
        {
            var playerState = GameController.Instance.playerState;
            mText.text = string.Format("Level {0}, exp: {1}/{2}", playerState.level, playerState.experience,
                GameController.Instance.playerState.levelupThreshold);
        }
    }
}
