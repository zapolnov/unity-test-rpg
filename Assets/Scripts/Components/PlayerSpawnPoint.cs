
using UnityEngine;
using UnityEngine.AI;

namespace Game
{
    public class PlayerSpawnPoint : MonoBehaviour
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

                GameController.Instance.SetGameplayState();

                var playerController = GameController.Instance.playerController;
                var navMeshAgent = playerController.GetComponent<NavMeshAgent>();
                navMeshAgent.Warp(transform.position);

                var playerTransform = playerController.transform;
                playerTransform.position = transform.position;
                playerTransform.rotation = transform.rotation;

                Destroy(gameObject);

                playerController.gameObject.SetActive(true);
            }
        }
    }
}
