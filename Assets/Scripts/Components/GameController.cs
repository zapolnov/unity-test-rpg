
using UnityEngine;
using System;

namespace Game
{
    public class GameController : MonoBehaviour
    {
        public PlayerState playerState = new PlayerState();
        public PlayerDefinition playerDefinition;
        public ParticleManager particleManager = new ParticleManager();
        public PlayerController playerController;

        private static GameController mInstance;
        public static GameController Instance {
                get {
                    if (mInstance == null)
                        throw new InvalidOperationException("GameController is null.");
                    return mInstance;
                }
            }

        private void Awake()
        {
            if (mInstance != null) {
                DestroyImmediate(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
            mInstance = this;

            playerState.Init(playerDefinition);
        }

        private void OnDestroy()
        {
            particleManager.Clear();

            if (mInstance == this)
                mInstance = null;
        }

        private void Update()
        {
            particleManager.Update();
        }
    }
}
