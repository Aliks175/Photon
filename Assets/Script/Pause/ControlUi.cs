
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ControlUi : MonoBehaviour
{
    [SerializeField] private GameObject _elementToFocus;
    [SerializeField] private List<GameObject> _elementToOff;
    [SerializeField] private List<GameObject> _elementToOn;

    public void ControlFocusElement(bool change)
    {
        ControlActiveElementUi(false, _elementToOff);
       
        ControlActiveElementUi(change, _elementToOn);
        
        EventSystem.current.SetSelectedGameObject(_elementToFocus);
    }

    private void ControlActiveElementUi(bool isActive, List<GameObject> gameObjects)
    {
        if(gameObjects==null) {return; }    
        for (int i = 0; i < gameObjects.Count; i++)
        {
            gameObjects[i].SetActive(isActive);
        }
    }
}
