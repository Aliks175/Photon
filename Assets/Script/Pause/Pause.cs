using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    private bool _paused = false;
    public bool Paused { get{ return _paused; } private set { _paused = value; } }

    public void OnPause(PlayerCharecter playerCharecterV1_3)
    {
        Paused = !Paused;
        if (Paused)
        {
            playerCharecterV1_3.EnablePauseMenuControls();
        }
        else
        {
            playerCharecterV1_3.EnableGameplayControls();
        }
        ControlTime(Paused);
    }

    private void ControlTime(bool timeStop)
    {
        if (timeStop)
        {
            Time.timeScale = 0f;
        }
        else 
        {
            Time.timeScale = 1f;
        }
    }
}
