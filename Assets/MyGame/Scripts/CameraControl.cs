using NUnit.Framework;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviourPunCallbacks
{
    [SerializeField] private List<GameObject> _conectedCamera;

    private PhotonView _photonView;
    private void Awake()
    {
        if(_photonView == null)
        {
            _photonView = GetComponentInChildren<PhotonView>();
        }
    }

    void Start()
    {
        if (!_photonView.IsMine)
        {
            // ��������� ������ �� ��-����� �������
            if (_conectedCamera != null)
                foreach (var item in _conectedCamera)
                {
                    item.gameObject.SetActive(false);
                }
        }
        else
        {
            // ������ ���������� ������ ��������
            if (_conectedCamera != null)
                foreach (var item in _conectedCamera)
                {
                    item.gameObject.SetActive(true);
                }
        }
    }
}
