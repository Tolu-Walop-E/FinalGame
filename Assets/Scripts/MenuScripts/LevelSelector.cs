using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public GameObject LevelSelectMenu;
    public GameObject[] levels;
    private Button playButton;
    public bool[] levelUnlocked;

    void Start()
    {
        LoadLevelUnlockedState();
        foreach (GameObject level in levels)
        {
            level.SetActive(false);
        }

        // Activate the first panel (Main Menu)
        if (levels.Length > 0)
        {
            levels[0].SetActive(true);
        }

        
    }

    void SaveLevelUnlockedState()
    {
        // Convert the bool array to a comma-separated string
        string state = string.Join(",", levelUnlocked.Select(b => b.ToString()).ToArray());
        PlayerPrefs.SetString("LevelUnlocked", state);
        PlayerPrefs.Save();
    }

    void LoadLevelUnlockedState()
    {
        if (PlayerPrefs.HasKey("LevelUnlocked"))
        {
            // Get the saved string
            string state = PlayerPrefs.GetString("LevelUnlocked");
            
            // Convert the string back to a bool array
            levelUnlocked = state.Split(',').Select(s => bool.Parse(s)).ToArray();
        }
        else
        {
            // Set default value if no saved state exists
            levelUnlocked = new bool[]{true, false, false, false, false};
        }
    }



    private void Awake()
    {
        PanelsToArray();
    }

    void PanelsToArray()
    {
        int numLevels = LevelSelectMenu.transform.childCount; // Automatically get the number of children
        levels = new GameObject[numLevels];

        // Loop through the children and assign them to the array
        for (int i = 0; i < numLevels; i++)
        {
            // Directly assign the GameObject of each child
            levels[i] = LevelSelectMenu.transform.GetChild(i).gameObject;
        }
    }

    public void BackToMainMenu()
    {
        SaveLevelUnlockedState();
        SceneManager.LoadScene("Main-Menu");
    }

    public void PlayLevel(){
        string[] levelNames = {"Level-1-Forest","Level-2-Sandstorm","Level-3-Oasis","Level-4-Mountains","Level-5-Ice-Level"};
        int currentIndex = GetActivePanelIndex();
        Debug.Log(currentIndex);
        Debug.Log(levelNames[currentIndex]);
        SaveLevelUnlockedState();
        SceneManager.LoadScene(levelNames[currentIndex]);

    }

    public void NextPanelRight()
    {
        int currentIndex = GetActivePanelIndex();

        if (currentIndex != -1)
        {
            levels[currentIndex].SetActive(false);
            int nextIndex = (currentIndex + 1) % levels.Length;
            levels[nextIndex].SetActive(true);
            GameObject buttonObject = GameObject.Find("PlayButton");
            playButton = buttonObject.GetComponent<Button>();
            if(!(levelUnlocked[nextIndex]))
            {
                playButton.interactable = false;
            }else{
                playButton.interactable = true;
            }
        }
    }

    public void NextPanelLeft()
    {
        int currentIndex = GetActivePanelIndex();
        if(currentIndex == 0)
        {
            levels[currentIndex].SetActive(false);
            int nextIndex = levels.Length - 1;
            levels[nextIndex].SetActive(true);
            GameObject buttonObject = GameObject.Find("PlayButton");
            playButton = buttonObject.GetComponent<Button>();
            if(!(levelUnlocked[nextIndex]))
            {
                playButton.interactable = false;
            }else{
                playButton.interactable = true;
            }
        }
        else if (currentIndex != -1)
        {
            levels[currentIndex].SetActive(false);
            int nextIndex = (currentIndex - 1) % levels.Length;
            levels[nextIndex].SetActive(true);
            GameObject buttonObject = GameObject.Find("PlayButton");
            playButton = buttonObject.GetComponent<Button>();
            if(!(levelUnlocked[nextIndex]))
            {
                playButton.interactable = false;
            }else{
                playButton.interactable = true;
            }
        }
    }

   
    private int GetActivePanelIndex()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            if (levels[i].activeSelf)
            {
                return i;
            }
        }

        return -1; 
    }

    public void UnlockAll()
    {   
        for (int i = 0; i < 5; i++)
        {
            levelUnlocked[i] = true;
        }
    }

    public void ResetAll(){
        levelUnlocked = new bool[]{true, false, false, false, false};
    }
}
