using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.LowLevel;

public class Conect : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform _startPos;
    [SerializeField] private GameObject _uiGrid;
    [SerializeField] private GameObject _uiPrefPlayerPanel;
    public UnityEvent StartEvent;
    public UnityEvent ConectEvent;

    private void Start()
    {
        StartEvent?.Invoke();
        PhotonNetwork.ConnectUsingSettings(); // запускаем подключение
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Успешное подключение к Photon");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Вошли в лобби");
        RoomOptions options = new RoomOptions()
        {
            MaxPlayers = 5,
            IsVisible = false
        };
        PhotonNetwork.JoinOrCreateRoom("TestRoom", options, Photon.Realtime.TypedLobby.Default);
        // Мы создаем либо присоединяемся к уже созданному лоби 
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Вошли в комнату");
       var a = PhotonNetwork.Instantiate("Player", _startPos.position, Quaternion.identity); // заспавнить игрока
        Debug.Log($"Кол-во игроков {PhotonNetwork.CurrentRoom.PlayerCount}");
        ConectEvent?.Invoke();
        var b =a.GetComponentInChildren<ICharacterData>();
        if (b != null)
        {
            var g = Instantiate(_uiPrefPlayerPanel, _uiGrid.transform);
            var u = g.GetComponentInChildren<UiStats>();
            if (u == null) return;
            b.SetUi(u);
            u.SetName("Player "+ PhotonNetwork.CurrentRoom.PlayerCount);
            u.SetScore(0);
        }
    }
}