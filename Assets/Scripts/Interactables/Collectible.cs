
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Game
{
    [RequireComponent(typeof(UniqueId))]
    public class Collectible : AbstractInteractable
    {
        [Serializable]
        public struct SavedState
        {
            public bool hasBeenCollected;
        }

        public CollectibleDefinition definition;

        private UniqueId mUniqueId;

        void Start()
        {
            mUniqueId = GetComponent<UniqueId>();

            Dictionary<string, Collectible.SavedState> items = GameController.Instance.items;
            if (items.ContainsKey(mUniqueId.guid)) {
                SavedState state = items[mUniqueId.guid];
                if (state.hasBeenCollected)
                    Destroy(gameObject);
            }
        }

        public override void Interact()
        {
            GameController.Instance.AddToInventory(definition);
            Destroy(gameObject);

            SavedState s = new SavedState();
            s.hasBeenCollected = true;
            GameController.Instance.items[mUniqueId.guid] = s;
        }
    }
}
