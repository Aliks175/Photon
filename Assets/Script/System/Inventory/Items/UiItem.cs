using System.Collections.Generic;
using UnityEngine;

public class UiItem : MonoBehaviour, IUiItem, ICrafteble
{
    public string NameId => _name;
    [SerializeField] private List<AbilityItem> _listAbility;
    [SerializeField] private bool isDestroyForOver = false;
    [SerializeField] private string _name = "None";
    private ICharacterData _characterData;

    public void Initialization(ICharacterData characterData)
    {
        _characterData = characterData;
        if (_listAbility != null)
        {
            foreach (var ability in _listAbility)
            {
                if (ability == null) continue;

                ability.Initialization(characterData);
            }
        }
    }

    public void UseItem()
    {
        if (_listAbility != null)
        {
            foreach (var ability in _listAbility)
            {
                if (ability == null) continue;
                if (_characterData != null)
                {
                    ability.UseAbility(_characterData);
                }
            }
        }
        if (isDestroyForOver) { Destroy(gameObject); }
    }
}


