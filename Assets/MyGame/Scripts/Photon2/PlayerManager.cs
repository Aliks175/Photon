using Photon.Pun;
using UnityEngine;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    public static GameObject LocalPlayerInstance;

    private PhotonView _photonView;

    private void Awake()
    {
        if (_photonView == null)
        {
            _photonView = GetComponentInChildren<PhotonView>();
        }

        if (_photonView.IsMine)
        {
            LocalPlayerInstance = this.gameObject;
        }
        // #Critical
        // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
        DontDestroyOnLoad(gameObject);
    }
}
