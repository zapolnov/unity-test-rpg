
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

namespace Game
{
    public class GameController : MonoBehaviour
    {
        public enum State
        {
            Gameplay,
            DialogMessage,
        }

        public PlayerState playerState = new PlayerState();
        public PlayerDefinition playerDefinition;
        public ParticleManager particleManager = new ParticleManager();
        public PlayerController playerControllerPrefab;
        public PlayerController playerController;
        public GameObject hud;
        public GameObject eventSystem;
        public HealthIndicator playerHealthIndicator;
        public GameObject dialogMessagePanel;
        public Text dialogMessageText;

        private AbstractQuestElement mNextQuestElement;

        private State mState = State.Gameplay;
        public State state {
                get { return mState; }
                private set {
                    if (value == mState)
                        return;

                    switch (mState) {
                        case State.Gameplay:
                            Time.timeScale = 0.0f;
                            break;
                        case State.DialogMessage:
                            dialogMessagePanel.SetActive(false);
                            break;
                    }

                    mState = value;

                    switch (mState) {
                        case State.Gameplay:
                            Time.timeScale = 1.0f;
                            break;
                        case State.DialogMessage:
                            dialogMessagePanel.SetActive(true);
                            break;
                    }
                }
            }

        private static GameController mInstance;
        public static GameController Instance {
                get {
                    if (mInstance == null)
                        throw new InvalidOperationException("GameController is null.");
                    return mInstance;
                }
            }

        public static bool IsInitialized()
        {
            return mInstance != null;
        }

        public static void EnsureInitialized()
        {
            if (mInstance == null)
                SceneManager.LoadScene("Shared", LoadSceneMode.Additive);
        }

        private void Awake()
        {
            if (mInstance != null) {
                DestroyImmediate(gameObject);
                return;
            }

            mInstance = this;

            var player = Instantiate(playerControllerPrefab.gameObject);
            playerController = player.GetComponent<PlayerController>();
            playerHealthIndicator.healthComponent = player.GetComponent<AbstractHealthComponent>();

            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(playerController.gameObject);
            DontDestroyOnLoad(hud);
            DontDestroyOnLoad(eventSystem);

            dialogMessagePanel.SetActive(false);

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
            switch (state) {
                case State.Gameplay:
                    break;

                case State.DialogMessage:
                    if (Input.GetButtonDown("Submit") || Input.GetButtonDown("Cancel") || Input.GetButtonDown("Fire1")) {
                        if (mNextQuestElement)
                            mNextQuestElement.Run();
                        else
                            state = State.Gameplay;
                    }
                    break;
            }

            particleManager.Update();
        }

        public void SwitchToScene(string name)
        {
            state = State.Gameplay;
            playerController.gameObject.SetActive(false);
            particleManager.Clear();
            SceneManager.LoadScene(name, LoadSceneMode.Single);
        }

        public void DisplayDialogMessage(string message, AbstractQuestElement next)
        {
            state = State.DialogMessage;
            mNextQuestElement = next;
            dialogMessageText.text = message;
        }
    }
}
