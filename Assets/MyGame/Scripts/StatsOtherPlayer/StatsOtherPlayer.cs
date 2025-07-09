using Photon.Pun;
using TMPro;
using UnityEngine;

public class StatsOtherPlayer : MonoBehaviour, IPunObservable
{
    [SerializeField] private TextMeshPro _statsPlayer;
    [SerializeField] private float _speed = 20f;
    private const string info = "\nLevel: ";
    private string namePlayer;
    private string tempText;
    private PlayerCharecter playerCharecter;
    private Transform _target;

    private void Update()
    {
        if (_target == null) return;
        if (transform.position != _target.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);
        }
    }

    private void OnDisable()
    {
        playerCharecter.CharacterData.SystemLevel.OnLevelUp -= UpdateInfo;
    }

    public void SetPlayer(PlayerCharecter target)
    {
        playerCharecter = target;
        namePlayer = playerCharecter.photonView.Owner.NickName;
        UpdateInfo();
        playerCharecter.CharacterData.SystemLevel.OnLevelUp += UpdateInfo;
    }

    private void UpdateInfo()
    {
        SetInfo(namePlayer, playerCharecter.CharacterData.SystemLevel.Level.ToString());
        _target = playerCharecter.gameObject.transform;
    }

    private void SetInfo(string name, string Level)
    {
        tempText = name + info + Level;

        _statsPlayer.SetText(tempText);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(tempText);
        }
        else
        {
            // Network player, receive data
            this.tempText = (string)stream.ReceiveNext();
            _statsPlayer.SetText(tempText);
        }
    }
}
