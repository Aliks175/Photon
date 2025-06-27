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
        PhotonNetwork.ConnectUsingSettings(); // ��������� �����������
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("�������� ����������� � Photon");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("����� � �����");
        RoomOptions options = new RoomOptions()
        {
            MaxPlayers = 5,
            IsVisible = false
        };
        PhotonNetwork.JoinOrCreateRoom("TestRoom", options, Photon.Realtime.TypedLobby.Default);
        // �� ������� ���� �������������� � ��� ���������� ���� 
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("����� � �������");
       var a = PhotonNetwork.Instantiate("Player", _startPos.position, Quaternion.identity); // ���������� ������
        Debug.Log($"���-�� ������� {PhotonNetwork.CurrentRoom.PlayerCount}");
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