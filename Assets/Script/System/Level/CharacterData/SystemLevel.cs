using System;
using UnityEngine;

public class SystemLevel
{
    private CharacterData _characterData;
    private LevelLine _levelLine;
    public int Level { get { return _level; } private set { _level = value; } }

    private int _�xperience = 0;
    private int _level = 0;
    private float _expDoNextLevel = 0;
    private float _procentExpDoNextLevel = 0;

    public event Action<SystemLevelData> OnSetUp;
    public event Action<SystemLevelData> OnChangeExp;
    public event Action OnLevelUp;

    public SystemLevel(LevelLine levelLine, CharacterData characterData)
    {
        _levelLine = levelLine;
        _characterData = characterData;
    }

    /// <summary>
    /// �������� ��������� �� ��������� �������
    /// </summary>
    private void CheckUpdateLevel()
    {
        int Leveltemp = Mathf.FloorToInt(_levelLine.CurveLevel.Evaluate(_�xperience));
        if (Leveltemp > Level)
        {
            while (Level < Leveltemp)
            {
                Level++;
                OnLevelUp?.Invoke();
            }
        }
    }

    /// <summary>
    /// �������� �xperience
    /// </summary>
    /// <param name="newExp"></param>
    public void SetEx(int newExp)
    {
        newExp = Mathf.Abs(newExp);
        _�xperience += newExp;
        CheckUpdateLevel();
        CalculateExp();

        OnChangeExp?.Invoke(new SystemLevelData()
        {
            _nowExp = _�xperience,
            _level = Level,
            _expDoNextLevel = _expDoNextLevel,
            _rangeExpDoNextLevel = _procentExpDoNextLevel
        });
    }

    /// <summary>
    /// ������� � ���������� ����� , ������� �������� ����� , � ���������� �����������
    /// </summary>
    private void CalculateExp()
    {
        var keysLevelUpLine = _levelLine.CurveLevel.keys;// �������� ��� ����� ������ 1 ����� ����� 1 ������
        float ExpNextLevel = 0; // ���������� ����� ���������� ������ 

        if (keysLevelUpLine.Length < Level + 1)
        {
            _expDoNextLevel = 0;
            _procentExpDoNextLevel = 1f;
            return;
        } // ���� ��������� ������� ������ ��� ���� � ������ , �� �� ������ �� ������ ����� ������ ������������� ������ 

        ExpNextLevel = keysLevelUpLine[Level].time;// ���������� ����� ���������� ������ 
        _expDoNextLevel = ExpNextLevel - _�xperience; // ���������� ����� �� ���������� ������ 

        float doNowLevelToNextLevel = (_�xperience - keysLevelUpLine[Level - 1].time);// ���������� ����� ���������� ����� ��������� �������� ������ 
        float rangeExpDoNowLevelToNextLevel = (ExpNextLevel - keysLevelUpLine[Level - 1].time);// ���������� ����� �� �������� ������ �� ���������� 

        float Procent = doNowLevelToNextLevel / rangeExpDoNowLevelToNextLevel;// ������� ����������� ����� �� ��������� ���������� ������ , ��� 100% ��� 1.0 ��� ��������� ���� ������

        _procentExpDoNextLevel = Mathf.Clamp01(Procent);
    }

    /// <summary>
    /// ��������� ������������� 
    /// </summary>
    public void Initialization()
    {
        Level = Mathf.FloorToInt(_levelLine.CurveLevel.Evaluate(_�xperience));
        CalculateExp();
        OnSetUp?.Invoke(new SystemLevelData()
        {
            _nowExp = _�xperience,
            _level = Level,
            _expDoNextLevel = _expDoNextLevel,
            _rangeExpDoNextLevel = _procentExpDoNextLevel
        });
    }
}
/// <summary>
/// ��������� ���������� ���������� �� ������
/// </summary>
public struct SystemLevelData
{
    public int _nowExp;
    public float _expDoNextLevel;
    public float _rangeExpDoNextLevel;
    public int _level;
}
