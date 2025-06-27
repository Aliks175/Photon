using NUnit.Framework;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private List<GameObject> _conectedCamera;
    [SerializeField] private PhotonView _photonView;

    void Start()
    {
        if (!_photonView.IsMine)
        {
            // Отключаем камеру на не-своих игроках
            if (_conectedCamera != null)
                foreach (var item in _conectedCamera)
                {
                    item.gameObject.SetActive(false);
                }
        }
        else
        {
            // Камера локального игрока включена
            if (_conectedCamera != null)
                foreach (var item in _conectedCamera)
                {
                    item.gameObject.SetActive(true);
                }
        }
    }
}
