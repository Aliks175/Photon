using UnityEngine;

public class GameLogicManager : Singleton<GameLogicManager>
{
    [SerializeField] private Pause _pause;
    [SerializeField] private ControlUi _controlUi;
    private PlayerCharecter _playerCharecter;

    public void SetSub(GameObject player)
    {
        if (player == null) { Debug.LogError("Not Found CharacterData "); return; }
        PlayerCharecter tempPlayerCharecter = player.GetComponent<PlayerCharecter>();
        if (tempPlayerCharecter == null) { Debug.LogError("Not Found CharacterData "); return; }
        _playerCharecter = tempPlayerCharecter;
    }

    public void Pause()
    {
        if (_playerCharecter == null) { Debug.LogError("Not Found CharacterData "); return; }
        _pause.OnPause(_playerCharecter);
        _controlUi.ControlFocusElement(_pause.Paused);
    }
}
