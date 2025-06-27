using System;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        ICharacterData characterData = other.GetComponentInChildren<ICharacterData>();
        if (characterData == null) { return; }

        characterData.SetScore(1);
        Destroy(gameObject);
    }
}
