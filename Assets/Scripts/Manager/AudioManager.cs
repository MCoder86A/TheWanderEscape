using UnityEngine;

namespace Manager
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}