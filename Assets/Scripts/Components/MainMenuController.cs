
using UnityEngine;
using System;

namespace Game
{
    public class MainMenuController : MonoBehaviour
    {
        [NonSerialized] private bool mInitialized;

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

        public void OnQuitButtonClicked()
        {
            Application.Quit();
        }
    }
}
