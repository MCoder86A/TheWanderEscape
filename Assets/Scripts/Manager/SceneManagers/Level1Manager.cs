using UnityEngine;
using UI;
using Unity.Cinemachine;
namespace Manager.SceneManagers
{
    public class Level1Manager : SceneManager<Level1Manager>
    {
        [SerializeField] private Transform playerSpawnPoint;
        [SerializeField] private CinemachineCamera playerVcam;
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
            playerVcam.Target.TrackingTarget = player;
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.SetPositionAndRotation(playerSpawnPoint.position, playerSpawnPoint.rotation);
            player.GetComponent<CharacterController>().enabled = true;
        }
    }
}