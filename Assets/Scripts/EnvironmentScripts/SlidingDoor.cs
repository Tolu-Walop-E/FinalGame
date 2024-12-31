using UnityEngine;

public class DoorAndWaterHandler : MonoBehaviour
{
    public GameObject door;
    public float doorCloseSpeed = 1.0f;
    public float doorMinZ = -0.58f;
    private Vector3 originalDoorPosition;

    public GameObject water;
    public float waterRiseSpeed = 0.1f;
    public float maxWaterScaleY = 5.0f;
    public float defaultWaterScaleY = 4.0f;

    private bool isClosingDoor = false;
    private bool isPlayerInTrap = false;

    void Start()
    {
        // Store the original door position when the script starts
        if (door != null)
        {
            originalDoorPosition = door.transform.position;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isClosingDoor = true;
            isPlayerInTrap = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrap = false;
            ResetWaterLevel();
            ResetDoorPosition();
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
                Debug.Log($"Door closing - Current position: {door.transform.position.z}");
            }
            else
            {
                isClosingDoor = false; // Stop moving the door once it reaches the limit
                Debug.Log("Door fully closed.");
            }
        }

        // Handle water rising
        if (isPlayerInTrap && water != null)
        {
            Vector3 waterScale = water.transform.localScale;
            if (waterScale.y < maxWaterScaleY)
            {
                water.transform.localScale += new Vector3(0, waterRiseSpeed * Time.deltaTime, 0);
            }
        }
    }

    void ResetWaterLevel()
    {
        if (water != null)
        {
            Vector3 waterScale = water.transform.localScale;
            water.transform.localScale = new Vector3(waterScale.x, defaultWaterScaleY, waterScale.z);
        }
    }

    void ResetDoorPosition()
    {
        if (door != null)
        {
            door.transform.position = originalDoorPosition;
            isClosingDoor = false;
            Debug.Log("Door reset to original position.");
        }
    }
}
