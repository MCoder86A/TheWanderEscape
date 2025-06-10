using Game.UI.Dialog;
using Manager;
using System;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Managers
{
    public class DialogHandler : MonoBehaviour
    {
        public static DialogHandler Instance;
        public event Action<int> OnOptionSelected;
        public event Action OnNext;
        [SerializeField] private List<Option> m_OptionBtns;
        [SerializeField] private GameObject m_prompt;
        [SerializeField]
        private TMP_Text
            m_AgentText,
            m_playerText,
            m_nextBtnText;
        [SerializeField]
        private GameObject m_nextBtn;
        [SerializeField, Tooltip("Character reveal delay in millisecond. Ex: 1000 means to reveal 1 character per second"), Min(0)]
        private float
        m_agentTextRevealDelay,
        m_playerTextRevealDelay;
        private CancellationTokenSource m_CancellationTokenSource;
        private bool m_IsDone = true;

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(this);
            RegisterEvents();
        }

        private void DeRegisterEvents()
        {
            for(int i=0; i<m_OptionBtns.Count; i++) { m_OptionBtns[i].Button.onClick.RemoveAllListeners(); }
        }

        private void DisableAllChoices()
        {
            for(int i = 0; i < m_OptionBtns.Count; i++)
            {
                m_OptionBtns[i].gameObject.SetActive(false);
            }
        }

        private void RegisterEvents()
        {
            for (int i = 0; i < m_OptionBtns.Count; i++)
            {
                int _optionIndex = i;
                m_OptionBtns[i].Button.onClick.AddListener(() =>
                {
                    m_playerText.text = m_OptionBtns[_optionIndex].Text.text;
                    m_CancellationTokenSource = new ();
                    DisableAllChoices();
                    m_playerText.enabled = true;
                    m_prompt.SetActive(true);
                    OnOptionSelected?.Invoke(_optionIndex + 1);
                    //if (await WriteTextAsync(m_playerText, "", $"{m_OptionBtns[_optionIndex].Text.text}", m_playerTextRevealDelay ,m_CancellationTokenSource.Token))
                    //{
                    //    try { await Awaitable.WaitForSecondsAsync(1, m_CancellationTokenSource.Token); }
                    //    catch { return; }
                    //}
                });
            }
            RegisterForNextButton();
        }

        private void RegisterForNextButton()
        {
            m_nextBtn.GetComponent<Button>().onClick.AddListener(() =>
            {
                OnNext?.Invoke();
            });
        }

        public bool WaitForNext(string p_msg)
        {
            m_nextBtn.SetActive(true);
            return true;
        }

        public Awaitable<bool> WaitForNextAsync(string p_msg)
        {
            AwaitableCompletionSource<bool> awaitableCompletionSource = new();
            WaitForNext(p_msg);
            void _a()
            {
                awaitableCompletionSource.SetResult(true);
                OnNext -= _a;
            }
            OnNext += _a;
            return awaitableCompletionSource.Awaitable;
        }

        /// <summary></summary>
        /// <param name="p_textContainer"></param>
        /// <param name="p_fixedText"></param>
        /// <param name="p_text"></param>
        /// <param name="p_delay">delay in millisecond</param>
        /// <param name="p_cancellationToken"></param>
        /// <returns></returns>
        private async Awaitable<bool> WriteTextAsync(TMP_Text p_textContainer, string p_fixedText, string p_text, float p_delay, CancellationToken p_cancellationToken)
        {
            p_textContainer.maxVisibleCharacters = 0;
            p_textContainer.text = p_fixedText;
            p_textContainer.ForceMeshUpdate();
            int _maxVisible = p_textContainer.textInfo.characterCount;
            p_textContainer.maxVisibleCharacters = _maxVisible;
            p_textContainer.text = $"{p_fixedText}{p_text}";
            float _time = Time.time;
            float _delayInSecond = p_delay/1000;
            if (_delayInSecond == 0) { p_textContainer.maxVisibleCharacters = 9999; return true; }
            for (int i = 0; i < p_text.Length;)
            {
                float _currTime = Time.time;
                int _c = (int)((_currTime - _time)/ _delayInSecond);
                _time = _currTime;
                _maxVisible += _c;
                i += _c;
                p_textContainer.maxVisibleCharacters = _maxVisible;
                try { await Awaitable.WaitForSecondsAsync(_delayInSecond, p_cancellationToken); }
                catch { return false; }
            }
            return true;
        }

        public async Awaitable PushPromptText(string p_fixedString, string p_prompt)
        {
            if (m_IsDone)
            {
                EventManager.Broadcast_OnDialogStarted();
                m_IsDone = false;
            }
            m_CancellationTokenSource = new();
            m_playerText.enabled = false;
            m_AgentText.enabled = true;
            m_prompt.SetActive(true);
            m_nextBtn.SetActive(false);
            await WriteTextAsync(m_AgentText, p_fixedString, p_prompt, m_agentTextRevealDelay, m_CancellationTokenSource.Token);
        }

        public void PushOptions(params string[] p_options)
        {
            for(int i=0; i< p_options.Length; i++)
            {
                m_OptionBtns[i].Text.text = p_options[i];
                m_OptionBtns[i].gameObject.SetActive(true);
            }
            m_nextBtn.SetActive(false);
            m_prompt.SetActive(true);
        }

        public Awaitable<int> PushOptionsAsync(params string[] p_options)
        {
            AwaitableCompletionSource<int> awaitableCompletionSource = new();
            PushOptions(p_options);
            void _a(int i)
            {
                awaitableCompletionSource.SetResult(i);
                OnOptionSelected -= _a;
            }
            OnOptionSelected += _a;
            return awaitableCompletionSource.Awaitable;
        }

        public void ClearDialog()
        {
            m_CancellationTokenSource?.Cancel();
            m_AgentText.enabled = false;
            m_playerText.enabled = false;
            for (int i = 0; i < m_OptionBtns.Count; i++) m_OptionBtns[i].gameObject.SetActive(false);
            m_nextBtn.SetActive(false);
            m_IsDone = true;
            EventManager.Broadcast_OnDialogReachedEnd();
            m_prompt.SetActive(false);
        }

        private void OnDestroy()
        {
            DeRegisterEvents();
            m_CancellationTokenSource?.Cancel();
        }
    }
}