using UnityEngine;
using UI;
using Unity.Cinemachine;
namespace Manager.SceneManagers
{
    public class Level1Manager : SceneManager<Level1Manager>
    {
        [SerializeField] private Transform playerSpawnPoint;
        private CharacterSelector characterSelector;

        private void Awake()
        {
            InitParams();
            SpwanPlayer();
        }

        private void InitParams()
        {
            characterSelector = FindAnyObjectByType<CharacterSelector>(FindObjectsInactive.Include);
        }

        private void SpwanPlayer()
        {
            Transform player = Instantiate(characterSelector.CurrentCharacter.Prefab).transform;
            FindAnyObjectByType<CinemachineCamera>().Target.TrackingTarget = player;
        }
    }
}