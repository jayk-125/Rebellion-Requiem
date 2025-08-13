using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Reference to main menu page
    public GameObject mainMenuPage;

    // Reference to controls page
    public GameObject controlsPage;
    // Reference to controls characters pages
    public GameObject golemPage;
    public GameObject butcherPage;
    public GameObject slingerPage;

    // Reference to credits page
    public GameObject creditsPage;

    // Reference to forth sfx
    public AudioSource forthSFX;
    // Reference to back sfx
    public AudioSource backSFX;

    // Main menu
    public void OpenMain()
    {
        mainMenuPage.SetActive(true);
    }

    public void CloseMain()
    {
        mainMenuPage.SetActive(false);
    }

    // Start game
    public void NewGame()
    {
        // Get current scene number
        int scene = SceneManager.GetActiveScene().buildIndex;
        // Move to next scene
        SceneManager.LoadScene(scene + 1);
    }

    // Quit game
    public void QuitGame()
    {
        Application.Quit();
    }

    // Controls
    public void OpenControls()
    {
        controlsPage.SetActive(true);
    }

    public void CloseControls()
    {
        controlsPage.SetActive(false);
    }

    // Golem
    public void OpenGolem()
    {
        golemPage.SetActive(true);
    }

    public void CloseGolem()
    {
        golemPage.SetActive(false);
    }

    // Butcher
    public void OpenButcher()
    {
        butcherPage.SetActive(true);
    }

    public void CloseButcher()
    {
        butcherPage.SetActive(false);
    }

    // Slinger
    public void OpenSlinger()
    {
        slingerPage.SetActive(true);
    }

    public void CloseSlinger()
    {
        slingerPage.SetActive(false);
    }

    // Credits
    public void OpenCredit()
    {
        creditsPage.SetActive(true);
    }

    public void CloseCredit()
    {
        creditsPage.SetActive(false);
    }

    // SFX forward
    public void ForthSFX()
    {
        forthSFX.Play();
    }
    // SFX backward
    public void BackSFX()
    {
        backSFX.Play();
    }
}
