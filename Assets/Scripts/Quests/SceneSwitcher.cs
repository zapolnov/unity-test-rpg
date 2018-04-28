
using UnityEngine;

namespace Game
{
    public class SceneSwitcher : AbstractInteractable
    {
        public string sceneName;

        public override void Interact()
        {
            GameController.Instance.SwitchToScene(sceneName);
        }
    }
}
