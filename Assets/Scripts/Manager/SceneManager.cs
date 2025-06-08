using UnityEngine;

namespace Manager
{
    public class SceneManager<T> : MonoBehaviour where T : class
    {
        public static T instance;
        [field: SerializeField] public Camera MainCam {  get; private set; }

        private void Awake()
        {
            instance = this as T;
        }
    }
}