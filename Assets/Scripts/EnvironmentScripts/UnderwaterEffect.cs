using UnityEngine;

public class WaterDepthTestHandler : MonoBehaviour
{
    public Material waterMaterial; // Assign your water material in the Inspector
    public Collider waterCollider; // Assign your water object's collider

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure the player triggers this
        {
            Debug.Log("Player has entered the water."); // Log for debugging
            SetDepthTestNotEqual(); // Switch to Depth Test: NotEqual
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure the player triggers this
        {
            Debug.Log("Player has exited the water."); // Log for debugging
            SetDepthTestLEqual(); // Switch back to Depth Test: LEqual
        }
    }

    private void SetDepthTestNotEqual()
    {
        if (waterMaterial != null)
        {
            // Set Depth Test to NotEqual
            waterMaterial.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.NotEqual);
            Debug.Log("Depth Test set to NotEqual."); // Log for debugging
        }
        else
        {
            Debug.LogWarning("Water material is not assigned."); // Warn if material is missing
        }
    }

    private void SetDepthTestLEqual()
    {
        if (waterMaterial != null)
        {
            // Set Depth Test to LEqual
            waterMaterial.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.LessEqual);
            Debug.Log("Depth Test set to LEqual."); // Log for debugging
        }
        else
        {
            Debug.LogWarning("Water material is not assigned."); // Warn if material is missing
        }
    }
}
