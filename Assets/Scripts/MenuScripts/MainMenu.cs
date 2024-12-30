using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Opens Level Selector
    public void PlayGame()
    {
        SceneManager.LoadScene("Level-Select");
    }

    // Closes Game
    public void Quit()
    {
        Application.Quit();
    }

    //Opens Settings Menu
    public void OpenSettings()
    {
        SceneManager.LoadScene("Settings");
    }

    //Returns to Main Menu
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Main-Menu");
    }
}
