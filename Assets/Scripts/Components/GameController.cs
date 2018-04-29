
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

namespace Game
{
    public class GameController : MonoBehaviour
    {
        public enum State
        {
            MainMenu,
            InGameMenu,
            Gameplay,
            PlayerDead,
            DialogMessage,
            DialogChoice,
            QuestsPanel,
        }

        [Serializable]
        public struct DialogChoiceButton
        {
            public Button button;
            public Text text;
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
        public GameObject dialogChoicePanel;
        public DialogChoiceButton[] dialogChoiceButtons;
        public NotificationsPanel notificationsPanel;
        public GameObject deathPanel;
        public GameObject inGameMenu;
        public QuestsPanel questsPanel;
        public Dictionary<string, QuestGiver.SavedState> questGivers = new Dictionary<string, QuestGiver.SavedState>();
        public Dictionary<string, AbstractEnemy.SavedState> enemies = new Dictionary<string, AbstractEnemy.SavedState>();
        public Dictionary<string, Collectible.SavedState> items = new Dictionary<string, Collectible.SavedState>();
        public Dictionary<CollectibleDefinition, int> inventory = new Dictionary<CollectibleDefinition, int>();
        public HashSet<Quest> activeQuests = new HashSet<Quest>();
        public HashSet<Quest> completedQuests = new HashSet<Quest>();

        private AbstractQuestElement mNextQuestElement;

        private State mState = State.MainMenu;
        public State state {
                get { return mState; }
                private set {
                    if (value != mState) {
                        OnStateLeave();
                        mState = value;
                        OnStateEnter();
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
                SceneManager.LoadScene("UI", LoadSceneMode.Additive);
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

            hud.SetActive(false);
            questsPanel.gameObject.SetActive(false);
            inGameMenu.SetActive(false);
            deathPanel.SetActive(false);
            dialogMessagePanel.SetActive(false);
            dialogChoicePanel.SetActive(false);

            Restart();
        }

        private void OnDestroy()
        {
            particleManager.Clear();

            if (mInstance == this)
                mInstance = null;
        }

        private void OnStateEnter()
        {
            switch (mState) {
                case State.MainMenu:
                    hud.SetActive(false);
                    break;
                case State.InGameMenu:
                    inGameMenu.SetActive(true);
                    break;
                case State.Gameplay:
                    Time.timeScale = 1.0f;
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    break;
                case State.PlayerDead:
                    deathPanel.SetActive(true);
                    break;
                case State.DialogMessage:
                    dialogMessagePanel.SetActive(true);
                    break;
                case State.DialogChoice:
                    dialogMessagePanel.SetActive(true);
                    dialogChoicePanel.SetActive(true);
                    break;
                case State.QuestsPanel:
                    questsPanel.FillContents();
                    questsPanel.gameObject.SetActive(true);
                    break;
            }
        }

        private void OnStateLeave()
        {
            switch (mState) {
                case State.MainMenu:
                    hud.SetActive(true);
                    break;
                case State.InGameMenu:
                    inGameMenu.SetActive(false);
                    break;
                case State.Gameplay:
                    Time.timeScale = 0.0f;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    break;
                case State.PlayerDead:
                    deathPanel.SetActive(false);
                    break;
                case State.DialogMessage:
                    dialogMessagePanel.SetActive(false);
                    break;
                case State.DialogChoice:
                    dialogMessagePanel.SetActive(false);
                    dialogChoicePanel.SetActive(false);
                    break;
                case State.QuestsPanel:
                    questsPanel.ClearContents();
                    questsPanel.gameObject.SetActive(false);
                    break;
            }
        }

        private void Update()
        {
            List<Quest> pendingCompletedQuests = new List<Quest>();
            foreach (var quest in activeQuests) {
                if (quest.GoalsAchieved())
                    pendingCompletedQuests.Add(quest);
            }

            foreach (var quest in pendingCompletedQuests) {
                activeQuests.Remove(quest);
                completedQuests.Add(quest);

                foreach (var item in quest.itemsToCollect)
                    RemoveFromInventory(item);

                notificationsPanel.AddMessage("Quest completed!");
                GiveExpToPlayer(quest.givesExp);
            }

            switch (state) {
                case State.MainMenu:
                case State.PlayerDead:
                case State.DialogChoice:
                    break;

                case State.InGameMenu:
                    if (Input.GetButtonDown("Cancel"))
                        state = State.Gameplay;
                    break;

                case State.QuestsPanel:
                    if (Input.GetButtonDown("Quests") || Input.GetButtonDown("Cancel"))
                        state = State.Gameplay;
                    break;

                case State.Gameplay:
                    if (playerState.health <= 0.0f)
                        state = State.PlayerDead;
                    if (Input.GetButtonDown("Quests"))
                        state = State.QuestsPanel;
                    if (Input.GetButtonDown("Cancel"))
                        state = State.InGameMenu;
                    break;

                case State.DialogMessage:
                    if (Input.GetButtonDown("Submit") || Input.GetButtonDown("Cancel") || Input.GetButtonDown("Fire1")) {
                        state = State.Gameplay;
                        if (mNextQuestElement)
                            mNextQuestElement.Run();
                    }
                    break;
            }

            particleManager.Update();
        }

        public void BeginQuest(Quest quest)
        {
            if (!activeQuests.Contains(quest) && !completedQuests.Contains(quest)) {
                activeQuests.Add(quest);
                notificationsPanel.AddMessage("New quest!");
            }
        }

        public void AddToInventory(CollectibleDefinition item)
        {
            if (inventory.ContainsKey(item))
                inventory[item]++;
            else
                inventory[item] = 1;
            notificationsPanel.AddMessage(string.Format("Found {0}", item.description));
        }

        public void RemoveFromInventory(CollectibleDefinition item)
        {
            if (inventory.ContainsKey(item))
                inventory[item]--;
            else
                inventory[item] = -1;
        }

        public void GiveExpToPlayer(int amount)
        {
            playerState.experience += amount;
            notificationsPanel.AddMessage(string.Format("+{0} experience", amount));

            while (playerState.experience >= playerState.levelupThreshold) {
                notificationsPanel.AddMessage("LEVEL UP!!!");
                playerState.level++;
                playerState.levelupThreshold =
                    (int)(playerState.levelupThreshold * playerDefinition.levelupThresholdMultiplier);
                playerState.health = playerDefinition.MaxHealth(playerState.level);
            }
        }

        private void Restart()
        {
            particleManager.Clear();
            activeQuests.Clear();
            completedQuests.Clear();
            inventory.Clear();
            items.Clear();
            questGivers.Clear();
            enemies.Clear();
            playerState.Init(playerDefinition);
        }

        public void OnContinueButtonClicked()
        {
            state = State.Gameplay;
        }

        public void OnRestartButtonClicked()
        {
            state = State.Gameplay;
            Restart();
            SwitchToScene("Village");
        }

        public void OnQuitButtonClicked()
        {
            state = State.MainMenu;
            Restart();
            SwitchToScene("MainMenu");
        }

        public void OnCloseQuestsPanelButtonClicked()
        {
            state = State.Gameplay;
        }

        public void SwitchToScene(string name)
        {
            playerController.gameObject.SetActive(false);
            particleManager.Clear();
            SceneManager.LoadScene(name, LoadSceneMode.Single);
        }

        public void SetMenuState() { state = State.MainMenu; }
        public void SetGameplayState() { state = State.Gameplay; }

        public void DisplayDialogMessage(string message, AbstractQuestElement next)
        {
            state = State.DialogMessage;
            mNextQuestElement = next;
            dialogMessageText.text = message;
        }

        public void DisplayDialogChoice(string message, IList<QuestDialogChoice.Option> options)
        {
            state = State.DialogChoice;
            dialogMessageText.text = message;

            int i = 0;
            foreach (var option in options) {
                if (i >= dialogChoiceButtons.Length) {
                    Debug.LogError("Too many options for the quest!");
                    break;
                }

                dialogChoiceButtons[i].text.text = option.choice;
                dialogChoiceButtons[i].button.gameObject.SetActive(true);
                dialogChoiceButtons[i].button.onClick.RemoveAllListeners();
                dialogChoiceButtons[i].button.onClick.AddListener(() => {
                        state = State.Gameplay;
                        if (option.nextQuestElement != null)
                            option.nextQuestElement.Run();
                    });

                ++i;
            }

            while (i < dialogChoiceButtons.Length)
                dialogChoiceButtons[i++].button.gameObject.SetActive(false);
        }
    }
}
