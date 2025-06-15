using UnityEngine;
using Interface;
using UnityEngine.UI;
using ScriptableObjects;

namespace UI
{
    public class CharacterSelector : MonoBehaviour, IEventReceiver
    {
        [SerializeField] private Transform displayTransform;
        [SerializeField] private Button nextBtn;
        [SerializeField] private Button prevBtn;
        private int current;

        public CharacterSO CurrentCharacter
        {
            get
            {
                displayTransform.GetChild(current).TryGetComponent(out CharacterCard characterCard);
                return characterCard.CharacterSO;
            }
        }

        private void Awake()
        {
            InitParams();
            RegisterEvents();
        }

        public void RegisterEvents()
        {
            nextBtn.onClick.AddListener(OnNext);
            prevBtn.onClick.AddListener(OnPrevious);
        }

        public void UnRegisterEvents()
        {
            nextBtn.onClick.RemoveListener(OnNext);
            prevBtn.onClick.RemoveListener(OnPrevious);
        }

        private void InitParams()
        {
            current = Mathf.Clamp(0, 0, displayTransform.childCount-1);
        }

        private void OnNext()
        {
            int childCnt = displayTransform.childCount;
            Switch(current = ++current%childCnt);
        }

        private void OnPrevious()
        {
            int childCnt = displayTransform.childCount;
            Switch(current = --current%childCnt);
        }

        private void Switch(int pointer)
        {
            int childCnt = displayTransform.childCount;
            if (pointer == -1) pointer = childCnt - 1;
            for(int i=0; i<childCnt; i++)
            {
                displayTransform.GetChild(i).gameObject.SetActive(pointer == i);
            }
        }

        private void OnDestroy()
        {
            UnRegisterEvents();
        }
    }
}