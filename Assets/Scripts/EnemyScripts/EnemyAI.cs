using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    Animator animator;
    public Transform player;
    public GameObject enemyProjectile;
    public float attackRange;
    private bool playerInRange;
    private Rigidbody rb;
    
    //Enemy walking AI
    public NavMeshAgent agent;
    //public LayerMask whatIsWalkable;
    //private Vector3 initialPosition;
    
    //public Vector3 walkPoint;
    //bool walkPointSet;
    //public float walkPointRange;
    
    

    private void Start()
    {
        // Find the Player object in the scene
        player = GameObject.Find("Player").transform;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false;
        agent.updateRotation = false;

        
        //initialPosition = transform.position;
        //MoveEnemy();

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

    // void MoveEnemy()
    // {
    //     //if the platform the enemy is on is walkable
    //     //it can move up and down the platform
    //     //whilst any player movement occurs
    //     //the walk animation plays
    //
    //     if (!walkPointSet)
    //     {
    //         SearchForWalkPoint();
    //     }
    //     if (walkPointSet)
    //     {
    //         // Restricting movement to X-axis (keeping Y and Z constant)
    //         Vector3 targetPosition = new Vector3(walkPoint.x, transform.position.y,0);
    //         agent.SetDestination(targetPosition);
    //
    //         if (Vector3.Distance(transform.position, targetPosition) < 1f)
    //         {
    //             walkPointSet = false;
    //         }
    //     }
    // }
    
    // private void SearchForWalkPoint()
    // {
    //     float randomX = Random.Range(-walkPointRange, walkPointRange);
    //     // Restrict walkPoint to X-axis, keeping Y and Z constant
    //     walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, 0);
    //
    //     if (Physics.Raycast(new Vector3(walkPoint.x, transform.position.y, 0), -transform.up, 2f, whatIsWalkable))
    //     {
    //         walkPointSet = true;
    //     }
    // }
    
    
}