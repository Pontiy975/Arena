using Arena.StatsSystem;
using System.Collections.Generic;
using UnityEngine;

namespace Arena.Player.Data
{
    [CreateAssetMenu(fileName = "CharacterModel", menuName = "ScriptableObjects/CharactersData/CharacterModel")]
    public class CharacterModel : ScriptableObject
    {
        [field: SerializeField] public List<Stat> Stats { get; private set; }

    }
}
