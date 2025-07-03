using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class GiveItem : MonoBehaviourPunCallbacks, IPunOwnershipCallbacks
{
    [SerializeField] private GameObject _uiItem;
    private bool _requestToDestroy = false;



    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
        if (targetView == photonView && _requestToDestroy)
        {
            photonView.RPC(nameof(RemoveItem), RpcTarget.AllBuffered);
            _requestToDestroy = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var playerData = other.gameObject.GetComponent<ICharacterData>();


        if (playerData != null && _uiItem.GetComponent<IUiItem>() != null)
        {
            playerData.Inventory.AddItem(_uiItem);

            if (photonView.IsMine)
            {
                photonView.RPC(nameof(RemoveItem), RpcTarget.AllBuffered);
            }
            else
            {
                _requestToDestroy = true; // флаг: хотим удалить объект
                photonView.RequestOwnership(); // ожидаем ответ
            }
        }

    }

    [PunRPC]
    private void RemoveItem()
    {
        // Предмет исчезает у всех
        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }// или PhotonNetwork.Destroy(gameObject);
    }


    private void OnValidate()
    {
        if (_uiItem.GetComponent<IUiItem>() == null)
        {
            Debug.LogError($"Not found UiItem {this.name} ");
        }
    }

    public void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
    {
    }
    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
    }
}
