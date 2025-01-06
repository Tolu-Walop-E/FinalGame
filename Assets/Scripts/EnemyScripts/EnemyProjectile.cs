using UnityEngine;

namespace EnemyScripts
{
    public class EnemyProjectile : MonoBehaviour
    {
        public float speed = 5f; // Speed of projectile
        public int damage = 10; // Damage dealt to the player
        private Vector3 targetDirection; // Direction towards the player

        private void Start()
        {
            // Calculate the direction towards the player
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                targetDirection = (player.transform.position - transform.position).normalized;
            }
            else
            {
                Debug.LogError("Player not found! Ensure the player has the tag 'Player'.");
                targetDirection = Vector3.zero;
            }
        }

        private void Update()
        {
            if (targetDirection != Vector3.zero)
            {
                MoveProjectile();
            }
        }

        void MoveProjectile()
        {
            // Move the projectile in the calculated direction
            transform.position += targetDirection * speed * Time.deltaTime;

            // Destroy the projectile if it goes out of bounds
            if (!IsWithinBounds())
            {
                Destroy(gameObject);
            }
        }

        bool IsWithinBounds()
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
            return screenPosition.x >= 0 && screenPosition.x <= Screen.width &&
                   screenPosition.y >= 0 && screenPosition.y <= Screen.height;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                // Apply damage to the player
                PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
                if (playerMovement != null)
                {
                    playerMovement.TakeDamage(damage);
                }

                // Destroy the projectile
                Destroy(gameObject);
            }
            else if (!other.CompareTag("Enemy"))
            {
                // Destroy the projectile on collision with other objects
                Destroy(gameObject);
            }
        }
    }
}
