using ScriptableObjects;
using UnityEngine;

namespace UI
{
    public class CharacterCard : MonoBehaviour
    {
        [field: SerializeField] public CharacterSO CharacterSO { get; private set; }
    }
}