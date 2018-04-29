
using UnityEngine;
using UnityEngine.AI;
using System;

namespace Game
{
    public class PlayerSpawnPoint : MonoBehaviour
    {
        public bool isMain;
        [NonSerialized] private bool mInitialized;

        void Awake()
        {
            GameController.EnsureInitialized();
        }

        void Update()
        {
            if (!mInitialized && GameController.IsInitialized()) {
                mInitialized = true;

                var gameController = GameController.Instance;

                var uniqueId = GetComponent<UniqueId>();
                if (uniqueId != null) {
                    if (gameController.returnSpawnPoint != null) {
                        if (uniqueId.guid != gameController.returnSpawnPoint) {
                            gameObject.SetActive(false);
                            return;
                        }
                    } else {
                        if (!isMain) {
                            gameObject.SetActive(false);
                            return;
                        }
                    }
                }

                gameController.SetGameplayState();

                var playerController = gameController.playerController;
                var navMeshAgent = playerController.GetComponent<NavMeshAgent>();
                navMeshAgent.Warp(transform.position);

                var playerTransform = playerController.transform;
                playerTransform.position = transform.position;
                playerTransform.rotation = transform.rotation;

                gameObject.SetActive(false);

                playerController.gameObject.SetActive(true);
            }
        }
    }
}
