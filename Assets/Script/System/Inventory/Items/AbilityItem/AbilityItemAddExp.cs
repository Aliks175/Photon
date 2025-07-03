using UnityEngine;

public class AbilityItemAddExp : AbilityItem
{
    [SerializeField] private int _exp;
    public override void UseAbility(ICharacterData characterData)
    {
        Debug.LogError($"ICharacterData Null ? - AbilityItemAddExp -{characterData == null} ");
        characterData.SystemLevel.SetEx(_exp);
    }
}
