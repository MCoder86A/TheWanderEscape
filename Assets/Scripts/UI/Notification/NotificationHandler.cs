using NaughtyAttributes;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Notification
{
    public class NotificationHandler : MonoBehaviour
    {
        public static NotificationHandler Instance { get; private set; }
        private Queue<ResultMsg> m_resultMsgQ = new();

        [SerializeField] private Color WarningColor = Color.yellow;
        [SerializeField] private Color DangerColor = Color.red;
        [SerializeField] private TMP_Text m_text;
        [SerializeField] private Image m_border;

        [SerializeField] private Animator m_resultAnimator;
        [SerializeField, AnimatorParam(nameof(m_resultAnimator))] private int m_expandAnimParam;

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            ProccessQueue();
        }

        private void ProccessQueue()
        {
            print($"{m_resultAnimator.playableGraph.IsPlaying()} -- " +
                $"{m_resultAnimator.playableGraph.IsDone()}");
            if(m_resultMsgQ.Count > 0 && !m_resultAnimator.playableGraph.IsPlaying())
            {
                ResultMsg _result = m_resultMsgQ.Dequeue();
                m_text.text = _result.TextMsg;
                m_border.color = _result.ColorMsg;
                m_resultAnimator.SetTrigger(m_expandAnimParam);
                print("PQ");
            }
        }

        [ContextMenu(nameof(Notify))]
        private void Notify()
        {
            PushResultMsg("VICTORY!!", PRIORITY.WARNING);
            PushResultMsg("LOSE!!", PRIORITY.DANGER);
        }

        public void PushResultMsg(string p_text, PRIORITY p_priority)
        {
            Color color = p_priority switch
            {
                PRIORITY.DANGER => WarningColor,
                PRIORITY.WARNING => DangerColor,
                _ => throw new NotImplementedException(),
            };
            
            m_resultMsgQ.Enqueue(new ResultMsg(p_text, color));
        }
         
        public enum PRIORITY : byte { DANGER, WARNING }

        [Serializable]
        public class ResultMsg
        {
            public string TextMsg;
            public Color ColorMsg;

            public ResultMsg(string textMsg, Color colorMsg)
            {
                TextMsg = textMsg;
                ColorMsg = colorMsg;
            }
        }
    }
}