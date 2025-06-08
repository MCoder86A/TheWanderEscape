using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = nameof(CharacterSO), menuName = "ScriptableObjects/Character")]
    public class CharacterSO : ScriptableObject
    {
        [field: SerializeField] public GameObject Prefab {  get; private set; }
        [field: SerializeField] public string Cname { get; private set; }
    }
}