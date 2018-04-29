
using UnityEngine;

namespace Game
{
    public class MainMenuController : MonoBehaviour
    {
        private bool mInitialized;

        void Awake()
        {
            GameController.EnsureInitialized();
        }

        void Update()
        {
            if (!mInitialized && GameController.IsInitialized()) {
                mInitialized = true;
                GameController.Instance.SetMenuState();
            }
        }

        public void OnPlayButtonClicked()
        {
            GameController.Instance.SwitchToScene("Village");
        }
    }
}
