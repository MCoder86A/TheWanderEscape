using Interface;
using Utility;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class UIHandler : MonoBehaviour, IEventReceiver
    {
        public static UIHandler Instance;
        [SerializeField] private Button playBtn;
        [SerializeField] private GameObject characterSelMenu;
        
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
        }

        public void UnRegisterEvents()
        {
            playBtn.onClick.RemoveListener(Play);
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