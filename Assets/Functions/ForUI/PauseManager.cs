using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public void OnGamePause()
    {
        Time.timeScale = 0;
    }
    public void OnGameContinue()
    {
        Time.timeScale = 1;
    }
}
