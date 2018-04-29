
using UnityEngine;

namespace Game
{
    public class SceneSwitcher : AbstractInteractable
    {
        public string sceneName;
        public PlayerSpawnPoint returnPoint;

        public override void Interact()
        {
            GameController.Instance.SwitchToScene(sceneName, returnPoint);
        }
    }
}
