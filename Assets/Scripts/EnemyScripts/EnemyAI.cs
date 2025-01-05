// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.AI;
//
// public class EnemyAI : MonoBehaviour
// {
//     public NavMeshAgent agent;
//     public Transform player;
//     public LayerMask whatIsGround, whatIsPlayer;
//     
//
//     public Vector3 walkPoint;
//     bool walkPointSet;
//     public float walkPointRange;
//
//     public float timeBetweenAttacks;
//     bool alreadyAttacked;
//     public GameObject projectile;
//
//     public float sightRange, attackRange;
//     public bool playerInSightRange, playerInAttackRange;
//
//     public float accuracy = 100f;
//
//     private float checkInterval = 0.5f;
//     private float checkTimer = 0f;
//     
//     public GameObject enemyProjectile;
//
//     private Animator animator;
//
//     void Start()
//     {
//         animator = GetComponent<Animator>();
//     }
//
//     private void Awake()
//     {
//         player = GameObject.Find("Player").transform;
//         agent = GetComponent<NavMeshAgent>();
//
//         agent.updateRotation = false;
//         agent.updateUpAxis = false;
//     }
//
//     void Update()
//     {
//         checkTimer += Time.deltaTime;
//
//         if (checkTimer >= checkInterval)
//         {
//             checkTimer = 0f;
//             playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
//             playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
//         }
//
//         if (!playerInSightRange && !playerInAttackRange) Patrolling();
//         if (playerInSightRange && !playerInAttackRange) ChasePlayer();
//         if (playerInAttackRange && playerInSightRange) Attacking();
//     }
//
//     private void Patrolling()
//     {
//         if (!walkPointSet) SearchWalkPoint();
//
//         if (walkPointSet)
//         {
//             // Restricting movement to X-axis (keeping Y and Z constant)
//             Vector3 targetPosition = new Vector3(walkPoint.x, transform.position.y,0);
//             agent.SetDestination(targetPosition);
//
//             if (Vector3.Distance(transform.position, targetPosition) < 1f)
//             {
//                 walkPointSet = false;
//             }
//         }
//     }
//
//     private void SearchWalkPoint()
//     {
//         float randomX = Random.Range(-walkPointRange, walkPointRange);
//         // Restrict walkPoint to X-axis, keeping Y and Z constant
//         walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, 0);
//
//         if (Physics.Raycast(new Vector3(walkPoint.x, transform.position.y, 0), -transform.up, 2f, whatIsGround))
//         {
//             walkPointSet = true;
//         }
//     }
//
//     private void ChasePlayer()
//     {
//         // Restricting chase movement to X-axis (keeping Y and Z constant)
//         Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, 0);
//         agent.SetDestination(targetPosition);
//     }
//
//     private void Attacking()
//     {
//         Vector3 heightOffset = new Vector3(0, 1f, 0); //height offset for enemy to slash
//         
//         agent.SetDestination(transform.position); 
//
//         Vector3 lookAtPosition = new Vector3(player.position.x, transform.position.y, 0);
//         transform.LookAt(lookAtPosition);
//
//         if (!alreadyAttacked)
//         {
//             // GameObject projectileInstance = Instantiate(projectile, transform.position, Quaternion.identity);
//             GameObject projectileInstance = Instantiate(enemyProjectile, (transform.position + heightOffset), Quaternion.identity);
//             animator.SetTrigger("animateProjectile");
//             
//             projectileInstance.transform.localScale = transform.localScale * 0.5f;
//
//             Rigidbody rb = projectileInstance.GetComponent<Rigidbody>();
//
//             Vector3 directionToPlayer = (player.position - transform.position).normalized;
//
//             Vector3 inaccurateDirection = AddInaccuracy(directionToPlayer, accuracy);
//
//             rb.velocity = Vector3.zero;
//
//             rb.AddForce(inaccurateDirection * 32f, ForceMode.Impulse);
//
//             alreadyAttacked = true;
//             Invoke(nameof(ResetAttack), timeBetweenAttacks);
//         }
//     }
//
//     private void ResetAttack()
//     {
//         alreadyAttacked = false;
//     }
//
//     private Vector3 AddInaccuracy(Vector3 originalDirection, float accuracy)
//     {
//         float inaccuracyFactor = (100f - accuracy) / 100f;
//
//         float randomOffsetX = Random.Range(-inaccuracyFactor, inaccuracyFactor);
//         float randomOffsetY = Random.Range(-inaccuracyFactor, inaccuracyFactor);
//
//         Vector3 inaccurateDirection = new Vector3(originalDirection.x + randomOffsetX, originalDirection.y + randomOffsetY, 0);
//
//         return inaccurateDirection.normalized;
//     }
// }



using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public GameObject projectile;
    public float attackRange;
    private bool playerInRange;

    private void Start()
    {
        // Find the Player object in the scene
        player = GameObject.Find("Player").transform;
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
            GameObject newProjectile = Instantiate(projectile, transform.position + heightOffset, Quaternion.identity);
            Vector2 direction = (player.position - transform.position).normalized;
            newProjectile.GetComponent<Rigidbody2D>().velocity = direction * 5f; // Adjust velocity as needed
        }
    }
}
