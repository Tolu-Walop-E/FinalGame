using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FinalCheckPoint : MonoBehaviour
{
    public bool[] levelUnlocked;
    public int levelIndex;
    void Start()
    {
        LoadLevelUnlockedState();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  // Assuming the player has the "Player" tag
        {
            unlockLevel();  // Unlock the level when the player reaches the checkpoint
        }
    }

    void unlockLevel(){
        if (levelIndex < 5){
            levelUnlocked[levelIndex] = true;
            SaveLevelUnlockedState();
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
}
