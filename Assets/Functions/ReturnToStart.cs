using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToStart : MonoBehaviour
{
    // Goes to start
    public void GoToStart()
    {
        // Return to start
        SceneManager.LoadScene(0);
    }
}
