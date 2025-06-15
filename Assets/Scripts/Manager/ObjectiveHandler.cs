using Levels;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class ObjectiveHandler : MonoBehaviour
    {
        public static ObjectiveHandler Instance {  get; private set; }

        [SerializeField, Expandable] private List<Objective> m_objectives;
        private int m_current = -1;

        private void Awake()
        {
            Instance = this;
        }

        public void NextObjective()
        {
            m_objectives[++m_current].StartMission();
        }
    }
}