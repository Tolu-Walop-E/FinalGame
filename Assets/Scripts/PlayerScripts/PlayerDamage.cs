using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerDamage : MonoBehaviour
{
    public Image healthBar;
    public float totalHealth = 100f;
    public Vector3 respawnPosition;
    private PlayerRespawn respawnManager;

    private PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        respawnManager = GetComponent<PlayerRespawn>();

        if (respawnPosition == Vector3.zero)
        {
            respawnPosition = new Vector3(-83, 1, -0.344f);  // Default respawn point if not set
        }
    }

    public void TakeDamage(float damage)
    {
        totalHealth -= damage;
        healthBar.fillAmount = totalHealth / 100f;

        if (totalHealth <= 0)
        {
            RespawnPlayer();
        }
    }

    void RespawnPlayer()
    {
        Debug.Log("Player is respawning...");

        respawnManager.Respawn();
        

        totalHealth = 100f;  // Reset health
        healthBar.fillAmount = totalHealth / 100f;  
    }
}
