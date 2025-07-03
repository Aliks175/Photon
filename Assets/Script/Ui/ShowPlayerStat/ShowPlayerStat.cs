using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowPlayerStat : MonoBehaviour
{

    [SerializeField] private Slider sliderRangeExpDoNextLevel;
    [SerializeField] private TextMeshProUGUI _level;
    [SerializeField] private TextMeshProUGUI _expNow;
    [SerializeField] private TextMeshProUGUI _expNextLevel;

    private ICharacterData characterData;

    public void SetSub(GameObject player)
    {
        if (player == null) { Debug.LogError("Not Found CharacterData "); return; }
        characterData = player.GetComponent<ICharacterData>();
        if (characterData == null) { Debug.LogError("Not Found CharacterData "); return; }
        characterData.SystemLevel.OnChangeExp += ChangeExp;
        characterData.SystemLevel.OnLevelUp += ChangeLevel;
        characterData.SystemLevel.OnSetUp += SetUp;
        characterData.SystemLevel.Initialization();
    }

    private void OnDisable()
    {
        characterData.SystemLevel.OnChangeExp -= ChangeExp;
        characterData.SystemLevel.OnLevelUp -= ChangeLevel;
        characterData.SystemLevel.OnSetUp -= SetUp;
    }

    private void ChangeExp(SystemLevelData expData)// Событие по изменению колво опыта 
    {
        UpdateUi(expData);// обновляем Ui
    }

    private void ChangeLevel()// отработало сабытие получение уровня 
    {
        Debug.Log("Level UP");
    }

    private void UpdateUi(SystemLevelData expData)// обновляем Ui заполняем поля 
    {
        _expNow.SetText($"EXP : {expData._nowExp.ToString()}");

        _level.SetText($"Level : {expData._level.ToString()}");

        _expNextLevel.SetText($"Lost EXP : {expData._expDoNextLevel.ToString()}");

        sliderRangeExpDoNextLevel.value = expData._rangeExpDoNextLevel;
    }

    private void SetUp(SystemLevelData expData) // предустановка обновить поля 
    {
        UpdateUi(expData);
    }

}
