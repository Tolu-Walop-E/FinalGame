using UnityEngine;

public class DoorAndWaterHandler : MonoBehaviour
{
    public GameObject door; // The door object
    public float doorCloseSpeed = 1.0f; // Speed of door movement
    public float doorMinZ = -0.58f; // Minimum Z position for the door when closed

    public GameObject water; // The water object
    public float waterRiseSpeed = 0.1f; // Speed of water rising
    public float maxWaterScaleY = 5.0f; // Maximum Y scale for the water

    private bool isClosingDoor = false; // Track if the door should close
    private bool isWaterRising = false; // Track if the water should rise
    private bool playerPastDoor = false; // Track if the player has passed the door

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!playerPastDoor) // Initial trigger for entering the room
            {
                isClosingDoor = true; // Start closing the door
                isWaterRising = true; // Start raising the water
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !playerPastDoor)
        {
            playerPastDoor = true; // Mark the player as having passed the door
        }
    }

    void Update()
    {
        // Handle door closing
        if (isClosingDoor && door != null)
        {
            Vector3 doorPosition = door.transform.position;
            if (doorPosition.z > doorMinZ)
            {
                door.transform.position -= new Vector3(0, 0, doorCloseSpeed * Time.deltaTime);
            }
            else
            {
                isClosingDoor = false; // Stop moving the door once it reaches the limit
            }
        }

        // Handle water rising
        if (isWaterRising && water != null && playerPastDoor)
        {
            Vector3 waterScale = water.transform.localScale;
            if (waterScale.y < maxWaterScaleY)
            {
                water.transform.localScale += new Vector3(0, waterRiseSpeed * Time.deltaTime, 0);
            }
            else
            {
                isWaterRising = false; // Stop scaling the water once it reaches the limit
            }
        }
    }
}
