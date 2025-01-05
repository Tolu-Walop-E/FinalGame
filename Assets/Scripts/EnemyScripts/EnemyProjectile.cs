using UnityEngine;

namespace EnemyScripts
{
    public class EnemyProjectile : MonoBehaviour
    {
        public float speed = 5f; //speed of projectile
        public static int noProjectiles = 0; //number of projectiles on map
        private bool isOriginal = true; //whether this projectile is the original projectile
        private Renderer renderer; 
        public Transform enemy;
        private Vector3 initialDirection; //initial direction of the projectile
        public int damage = 10; //damage the projectile can deal

        private void Start()
        {
            renderer = GetComponent<Renderer>();
            if (noProjectiles == 0) //if there are no projectiles on the screen
            {
                isOriginal = true; //then this projectile is the original
                noProjectiles++; //so, another projectile can be made
            }
            else
            {
                isOriginal = false; //this projectile is not the original
                Vector3 enemyForwardDirection = enemy.forward; //the forward direction of the player is determined
                //the initial direction of the projectile is set as the direction of the player
                initialDirection = new Vector3(enemyForwardDirection.z, 0, enemyForwardDirection.x).normalized; 
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
                renderer.enabled = true; //if it's not, its rendered
                MoveProjectile(); //and moves
            
            }
        }
        
        void MoveProjectile()
        {
            Debug.Log(initialDirection);
            //rotate in 180 degrees by y if in negative direction
            // Rotate by 180 degrees in Y if initial direction is negative
            if (initialDirection.z < 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            transform.position += initialDirection * speed * Time.deltaTime; //via the initial direction
        
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
            if (other.gameObject.CompareTag("Player")) //if the projectile collides with an player
            {
                PlayerDamage playerDamage = other.gameObject.GetComponent<PlayerDamage>();
                playerDamage.TakeDamage(damage); //the enemy takes damage
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
