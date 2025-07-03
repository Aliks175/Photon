using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Photon2
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        [Header("SettingsRoom")]
        [SerializeField] private int _maxPlayer = 5;

        [Header("EventState"), Space(20)]
        public UnityEvent StartEvent;
        public UnityEvent ConectLoadEvent;
        public UnityEvent DisConectEvent;

        private bool _isconnect = false;
        private bool _connectedToMaster = false;

        private Coroutine _coroutine;

        #region Private Serializable Fields

        #endregion

        #region Private Fields

        /// <summary>
        /// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
        /// </summary>
        string gameVersion = "1";

        #endregion

        #region MonoBehaviour CallBacks

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
        /// </summary>
        void Awake()
        {
            // #Critical
            // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        private void Start()
        {
            _isconnect = PhotonNetwork.ConnectUsingSettings();
            StartEvent?.Invoke();
        }

        #endregion

        #region MonoBehaviourPunCallbacks Callbacks

        public override void OnConnectedToMaster()
        {
            Debug.Log("ConnectedToMaster");
            _connectedToMaster = true;
        }

        public override void OnJoinedLobby()
        {
            if (!_isconnect)
            {
                _isconnect = PhotonNetwork.ConnectUsingSettings(); // запускаем подключение
                return;
            }

            if (PhotonNetwork.IsConnected)
            {

                Debug.Log("Вошли в лобби");
                RoomOptions options = new RoomOptions()
                {
                    MaxPlayers = _maxPlayer,
                    IsVisible = false
                };
                PhotonNetwork.JoinOrCreateRoom("TestRoom", options, Photon.Realtime.TypedLobby.Default);
                // Мы создаем либо присоединяемся к уже созданному лоби 
            }
            else
            {
                Debug.Log("Мы не в сети ");
            }

        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
            DisConectEvent?.Invoke();
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Подключение к комнате");
            Debug.Log("МЫ в комнате , загружаем уровень");

            // #Critical
            // Load the Room Level.
            
                PhotonNetwork.LoadLevel("GamePlayScene");
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Start the connection process.
        /// - If already connected, we attempt joining a random room
        /// - if not yet connected, Connect this application instance to Photon Cloud Network
        /// </summary>
        public void Connect()
        {
            if (_coroutine != null) return;
            _coroutine = StartCoroutine(Wait());
            // we check if we are connected or not, we join if we are , else we initiate the connection to the server.

        }


        private IEnumerator Wait()
        {
            ConectLoadEvent?.Invoke();
            yield return new WaitUntil(() => _connectedToMaster);
            if (PhotonNetwork.IsConnected && _isconnect)
            {
                // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
                PhotonNetwork.JoinLobby();
            }
            else
            {
                // #Critical, we must first and foremost connect to Photon Online Server.
                PhotonNetwork.GameVersion = gameVersion;
                _isconnect = PhotonNetwork.ConnectUsingSettings();
            }
            _coroutine = null;
        }
        #endregion



        //private void Awake()
        //{
        //    PhotonNetwork.AutomaticallySyncScene = true;
        //}

        //private void Start()
        //{
        //    _isconnect = PhotonNetwork.ConnectUsingSettings(); // запускаем подключение
        //}

        //#region MonoBehaviourPunCallbacks Callbacks

        //public override void OnConnectedToMaster()
        //{
        //    Debug.Log("Успешное подключение к Photon");

        //}

        //public override void OnJoinedLobby()
        //{
        //    if (!_isconnect)
        //    {
        //        _isconnect = PhotonNetwork.ConnectUsingSettings(); // запускаем подключение
        //        return;
        //    }
        //    if (PhotonNetwork.IsConnected)
        //    {
        //        StartEvent?.Invoke();
        //        Debug.Log("Вошли в лобби");
        //        RoomOptions options = new RoomOptions()
        //        {
        //            MaxPlayers = _maxPlayer,
        //            IsVisible = false
        //        };
        //        PhotonNetwork.JoinOrCreateRoom("TestRoom", options, Photon.Realtime.TypedLobby.Default);
        //        // Мы создаем либо присоединяемся к уже созданному лоби 
        //    }
        //    else
        //    {

        //        Debug.Log("Мы не в сети ");
        //    }
        //}

        //public override void OnDisconnected(DisconnectCause cause)
        //{
        //    _isconnect = false;
        //    Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        //    DisConectEvent?.Invoke();
        //}

        //public override void OnJoinedRoom()
        //{
        //    if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        //    {
        //        Debug.Log("We load the 'Room for 1' ");

        //        // #Critical
        //        // Load the Room Level.
        //        PhotonNetwork.LoadLevel("Playground1");
        //    }
        //    Debug.Log("Вошли в комнату");
        //    //var a = PhotonNetwork.Instantiate("Player", _startPos.position, Quaternion.identity); // заспавнить игрока
        //    Debug.Log($"Кол-во игроков {PhotonNetwork.CurrentRoom.PlayerCount}");
        //    ConectEvent?.Invoke();
        //    //var b = a.GetComponentInChildren<ICharacterData>();
        //    //if (b != null)
        //    //{
        //    //    var g = Instantiate(_uiPrefPlayerPanel, _uiGrid.transform);
        //    //    var u = g.GetComponentInChildren<UiStats>();
        //    //    if (u == null) return;
        //    //    b.SetUi(u);
        //    //    if (string.IsNullOrEmpty(PhotonNetwork.NickName))
        //    //    {
        //    //        PhotonNetwork.NickName = "Player_" + PhotonNetwork.CurrentRoom.PlayerCount;
        //    //    }
        //    //    u.SetName(PhotonNetwork.NickName);
        //    //    u.SetScore(0);
        //    //}
        //}
        //#endregion
    }
}