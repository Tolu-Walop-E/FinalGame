using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    Animator animator;
    public Transform player;
    public GameObject enemyProjectile;
    public float attackRange;
    private bool playerInRange;
    private Rigidbody rb;

    private void Start()
    {
        // Find the Player object in the scene
        player = GameObject.Find("Player").transform;
        animator = GetComponent<Animator>();
        
    }

    private void Update()
    {
        // Check if the player is within attack range
        CheckPlayerInRange();
        
        // If player is in range, turn to face the player and attack
        if (playerInRange)
        {
            TurnToPlayer();
            AttackPlayer();
        }
    }

    private void CheckPlayerInRange()
    {
        if (player != null)
        {
            // Calculate distance to player and check if it's within attack range
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            playerInRange = distanceToPlayer <= attackRange;
        }
    }

    private void TurnToPlayer()
    {
        if (player != null)
        {
            // Turn enemy to face the player on the right or left side
            Vector2 direction = player.position - transform.position;
            transform.right = direction.x > 0 ? Vector2.right : Vector2.left;
        }
    }

    private void AttackPlayer()
    {
        // Perform attack logic here, for example firing projectiles
        if (!IsInvoking(nameof(FireProjectile)))
        {
            // Start repeatedly firing projectiles
            InvokeRepeating(nameof(FireProjectile), 0f, 2f); // Fire projectile every 2 seconds
        }
    }

    private void FireProjectile()
    {
        Vector3 heightOffset = new Vector3(0, 1f, 0);
        if (player != null)
        {
            // Instantiate a projectile and launch it towards the player
            Instantiate(enemyProjectile, (transform.position + heightOffset), Quaternion.identity);
            Debug.Log("Projectile instantiated");
            animator.SetTrigger("animateProjectile");
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        // Prevent rotation in any axis
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetTrigger("animateKicking");
        }
    }
    
    //function moveEnenmy()
        //animator.SetBool("animateWalking", true)
    
}
