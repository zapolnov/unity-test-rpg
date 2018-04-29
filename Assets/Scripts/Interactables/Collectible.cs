
using UnityEngine;

namespace Game
{
    public class Collectible : AbstractInteractable
    {
        public CollectibleDefinition definition;

        public override void Interact()
        {
            GameController.Instance.AddToInventory(definition);
            Destroy(gameObject);
        }
    }
}
