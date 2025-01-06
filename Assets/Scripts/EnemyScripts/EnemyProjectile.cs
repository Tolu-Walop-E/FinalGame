using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyScripts
{
    public class EnemyProjectile : MonoBehaviour
    {
        public float speed = 5f; //speed of projectile
        public static int noProjectiles = 0; //number of projectiles on map
        private bool isOriginal = true; //whether this projectile is the original projectile
        private Renderer renderer; 
        public Transform player; //so the attack faces towards the player
        private Vector3 initialDirection; //initial direction of the projectile
        public int damage = 10; //damage the projectile can deal

        private void Start()
        {
            Debug.Log("In EnemyProjectile.cs");
            renderer = GetComponent<Renderer>();
            if (noProjectiles == 0) //if there are no projectiles on the screen
            {
                Debug.Log("There are no projectiles on the screen");
                isOriginal = true; //then this projectile is the original
                noProjectiles++; //so, another projectile must be made 
            }
            else
            {
                Debug.Log("There is at least one projectile on the screen");
                isOriginal = false; //this projectile is not the original
                Vector3 playerForwardDirection = player.forward; //the forward direction of the player is determined
                
                //the initial direction of the projectile is set as the negative direction of the player
                // initialDirection = new Vector3(playerForwardDirection.z, 0, playerForwardDirection.x).normalized; 
                // initialDirection = -initialDirection;
                
                Vector3 playerToEnemyDirection = (player.position - transform.position).normalized;
                initialDirection = new Vector3(playerToEnemyDirection.x, 0, 0).normalized;
                Debug.Log(initialDirection);
            }
        }

        void Update()
        {
            if (isOriginal)
            {
                renderer.enabled = false; //if the projectile is the original, it is not rendered in the game, but still exists
            }
            if (!isOriginal)
            {
                Debug.Log("Enemy Projectile is not Original");
                renderer.enabled = true; //if it's not, its rendered
                MoveProjectile(); //and moves
            
            }
        }
        
        void MoveProjectile()
        {
            Debug.Log("Enemy Projectile will move");
            // Calculate the movement direction based on the x component of the initial direction
            float movementDirection = Mathf.Sign(initialDirection.x); // 1 for right, -1 for left

            // Set the rotation based on the movement direction
            if (movementDirection == -1)
            { 
                transform.rotation = Quaternion.Euler(0, 180 * movementDirection, 0);
            }


            // Move the projectile in the initial direction
            transform.position += new Vector3(initialDirection.x, 0, 0) * speed * Time.deltaTime;
        
            // Check if the projectile is out of bounds and destroy clones only
            if (!IsWithinBounds() && !isOriginal) //if the projectile leaves bounds (hits nothing and goes off screen)
            {
                Destroy(gameObject); //its destroyed
            }
        }
        
        bool IsWithinBounds() //check if within bounds
        {
            Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
            if (screenPosition.x < 0 || screenPosition.x > Screen.width ||
                screenPosition.y < 0 || screenPosition.y > Screen.height)
            {
                return false;
            }
            return true;
        }
        
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player")) //if the projectile collides with a player
            {
                PlayerMovement playerMovement = other.GetComponent<PlayerMovement>(); //checks for existence of PlayerMovement script
                if (playerMovement != null)
                {
                    playerMovement.TakeDamage(damage); //uses the TakeDamage function in the script
                }
                Debug.Log("PLAYER HIT");
                Destroy(gameObject); //the projectile is destroyed
            }
            else if (!other.gameObject.CompareTag("Enemy")) //if the projectile collides with anything else
            {
                Debug.Log("OTHER HIT"); 
                Destroy(gameObject); //the projectile is destroyed
            }
        }
        
    }
}