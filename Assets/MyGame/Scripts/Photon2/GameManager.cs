using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


namespace Photon2
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        #region Public Field
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private GameObject _prefPlayer;
        [SerializeField] private GameObject _inventaryPool;
        [SerializeField] private Camera _camera;

        public UnityEvent<GameObject> OnEnterPlayer;



        private GameObject _player;
        private PlayerCharecter playerCharecter;
        #endregion


        #region Photon Callbacks

        public override void OnPlayerEnteredRoom(Player other)
        {
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log($"Player - {other.NickName} Enter");
            }
        }

        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log($"Player - {other.NickName} Leave");
            }
        }

        #endregion

        #region Unity Callbacks

        private IEnumerator Start()
        {
            yield return new WaitUntil(() => PhotonNetwork.InRoom);
            Debug.Log("Start на сцене вызван");
            if (_prefPlayer == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
            }
            else
            {
                Debug.LogFormat($" Instantiate character", SceneManager.GetActiveScene().name);
                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                _player = PhotonNetwork.Instantiate(_prefPlayer.name, _spawnPoint.position, Quaternion.identity, 0);
                string name = PhotonNetwork.NickName + PhotonNetwork.CurrentRoom.PlayerCount;
                _player.name = name;
                playerCharecter = _player.GetComponent<PlayerCharecter>();
                if (playerCharecter != null)
                {
                    playerCharecter.SetSub();
                    playerCharecter.CharacterData.Inventory.SetSub(_inventaryPool);
                    playerCharecter.PlayerMoved.SetCamera(_camera);
                    OnEnterPlayer?.Invoke(_player);
                }
            }

        }
        #endregion

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }

        public void LeavedRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        #region Private Methods

        //private void LoadArena()
        //{
        //    if (!PhotonNetwork.IsMasterClient)
        //    {
        //        Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
        //        return;
        //    }
        //    Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
        //    PhotonNetwork.LoadLevel("Playground" + PhotonNetwork.CurrentRoom.PlayerCount);
        //}
        #endregion
    }
}