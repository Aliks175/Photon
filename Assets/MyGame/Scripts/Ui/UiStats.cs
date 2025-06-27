using TMPro;
using UnityEngine;

public class UiStats : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _namePlayer;
    [SerializeField] private TextMeshProUGUI _scorePlayer;

    public void SetName(string name)
    {
        _namePlayer.SetText(name);
    }

    public void SetScore(int score)
    {
        _scorePlayer.SetText($"Score : {score.ToString()}");
    }
}
