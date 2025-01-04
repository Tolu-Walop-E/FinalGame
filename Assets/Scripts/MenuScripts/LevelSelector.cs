using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public GameObject LevelSelectMenu;
    public GameObject[] levels;
    public string[] levelNames = {"Level-1-Forest","Level-2-Sandstorm","Level-3-Oasis","","Level-5-Ice-Level"};
    public bool[] levelUnlocked = {true,false,false,false,false};
    private Button playButton;

    void Start()
    {
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

    public void PlayLevel(){
        int currentIndex = GetActivePanelIndex();
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
}
