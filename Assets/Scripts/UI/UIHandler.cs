using Interface;
using Utility;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using Manager;

namespace UI
{
    public class UIHandler : MonoBehaviour, IEventReceiver
    {
        public static UIHandler Instance;
        [SerializeField] private Button playBtn;
        [SerializeField] private GameObject characterSelMenu;

        [SerializeField] private GameObject m_gameOverMenu;
        [SerializeField] private Button gameoverBtn;
        
        private CharacterSelector characterSelector;

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitParams();
            RegisterEvents();
        }

        public void RegisterEvents()
        {
            playBtn.onClick.AddListener(Play);
            gameoverBtn.onClick.AddListener(Restart);
            EventManager.OnObjectiveUpdate += EventManager_OnObjectiveUpdate;
        }

        private void EventManager_OnObjectiveUpdate(Levels.Objective objective)
        {
            if(objective.CompletionState == Levels.Objective.COMPLETION_STATE.FAIL) { m_gameOverMenu.SetActive(true); }
        }

        private void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            m_gameOverMenu.SetActive(false);
        }

        public void UnRegisterEvents()
        {
            playBtn.onClick.RemoveListener(Play);
            gameoverBtn.onClick.RemoveListener(Restart);
            EventManager.OnObjectiveUpdate -= EventManager_OnObjectiveUpdate;
        }

        private void Play()
        {
            if (characterSelector.CurrentCharacter != null) SceneManager.LoadScene(Constants.SceneName.Level1);
            characterSelMenu.SetActive(characterSelector.CurrentCharacter == null);
        }

        private void InitParams()
        {
            characterSelector = FindAnyObjectByType<CharacterSelector>();
        }

        private void OnDestroy()
        {
            UnRegisterEvents();
        }
    }
}