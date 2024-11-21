using UnityEngine;
using UnityEngine.Rendering;

public class CaveLighting : MonoBehaviour
{
    public GameObject directionalLight; // Assign the Directional Light GameObject in the Inspector
    public GameObject globalVolume;    // Assign your Global Volume in the Inspector
    public Light playerLight;          // Assign the Player's Point Light in the Inspector

    private bool hasEnteredCave = false; // To ensure the logic only runs once per entry

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasEnteredCave) // Check if it's the player and hasn't already entered
        {
            hasEnteredCave = !hasEnteredCave; // Mark as entered
            EnterCave();
        }
    }

    private void EnterCave()
    {
        // Turn off Directional Light
        if (directionalLight != null)
        {
            directionalLight.SetActive(false);
        }

        // Set Ambient Lighting to black
        RenderSettings.ambientMode = AmbientMode.Flat;
        RenderSettings.ambientLight = Color.black;

        // Activate Global Volume for darkness
        if (globalVolume != null)
        {
            globalVolume.SetActive(true);
        }

        // Enable Player's Point Light
        if (playerLight != null)
        {
            playerLight.enabled = true;
        }
    }

    public void ResetCaveLighting()
    {
        // Turn on Directional Light
        if (directionalLight != null)
        {
            directionalLight.SetActive(true);
        }

        // Restore Ambient Lighting to default
        RenderSettings.ambientMode = AmbientMode.Skybox;
        RenderSettings.ambientLight = Color.white;

        // Deactivate Global Volume
        if (globalVolume != null)
        {
            globalVolume.SetActive(false);
        }

        // Disable Player's Point Light
        if (playerLight != null)
        {
            playerLight.enabled = false;
        }

        hasEnteredCave = false; // Allow re-entering if needed
    }

}
