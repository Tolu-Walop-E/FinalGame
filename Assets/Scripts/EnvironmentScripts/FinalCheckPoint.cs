using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class FinalCheckPoint : MonoBehaviour
{
    public bool[] levelUnlocked;
    public int levelIndex;
    public GameObject levelCompleteMenu;

    void Start()
    {
        LoadLevelUnlockedState();
        levelCompleteMenu.SetActive(false);
    }

    void OnTriggerEnter(Collider other) // Unlock the next level and triggers the level end screen when the player reaches the checkpoint
    {
        if (other.CompareTag("Player")) 
        {
            unlockLevel();
            levelCompleteMenu.SetActive(true);  
        }
    }

    void ReturnToLevelSelect() //Returns to the level select menu
    {
        SceneManager.LoadScene("Level-Select");
    }

    void unlockLevel(){ //Unlocks the next level
        if (levelIndex < 5){
            levelUnlocked[levelIndex] = true;
            SaveLevelUnlockedState();
        }
    }

    void SaveLevelUnlockedState() //Saves the contents of the unlocked levels array
    {
        // Convert the bool array to a comma-separated string
        string state = string.Join(",", levelUnlocked.Select(b => b.ToString()).ToArray());
        PlayerPrefs.SetString("LevelUnlocked", state);
        PlayerPrefs.Save();
    }

    void LoadLevelUnlockedState() //Loads the contents of the unlocked levels array
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
}
