using System.Collections;
using UnityEngine;

public class Action_Enemy : MonoBehaviour
{
    [Header("Basic Attributes")]
    public float moveSpeed = 2f;
    public float chaseRange = 5f;
    [Tooltip("Các layer của chướng ngại vật (tường, cây, v.v.)")]
    public LayerMask[] obstacleLayers;

    [Header("Patrol Settings")]
    public float patrolRadius = 3f; 
    public float waitTime = 1f;     
    private Transform player;
    private Vector3 patrolCenter;   
    private Vector3 targetPosition; 
    private bool isChasing = false;
    private bool isWalking = false;
    private bool isWaiting = false;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private LayerMask combinedObstacleLayer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("Player with tag 'Player' not found!");
        }

        anim = GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogError("Animator not found on Enemy!");
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found on Enemy!");
        }

        if (GetComponent<Rigidbody2D>() == null)
        {
            Debug.LogWarning("Action_Enemy: Rigidbody2D not found! Adding one.");
            gameObject.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        }
        if (GetComponent<Collider2D>() == null)
        {
            Debug.LogWarning("Action_Enemy: Collider2D not found! Adding BoxCollider2D.");
            gameObject.AddComponent<BoxCollider2D>();
        }

        patrolCenter = transform.position;
        combinedObstacleLayer = CombineLayerMasks(obstacleLayers);
        SetRandomPatrolTarget();
    }

    void Update()
    {
        if (player != null && Vector3.Distance(transform.position, player.position) <= chaseRange && HasLineOfSightToPlayer())
        {
            isChasing = true;
            ChasePlayer();
        }
        else
        {
            isChasing = false;
            Patrol();
        }

        anim.SetBool("isWalking", isWalking);
    }

    void Patrol()
    {
        if (!isWaiting && !isChasing)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            Vector3 newPosition = transform.position + direction * moveSpeed * Time.deltaTime;

            if (!IsPathBlocked(newPosition))
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                UpdateFlipDirection(targetPosition);
                isWalking = true;
            }
            else
            {
                isWalking = false;
                StartCoroutine(WaitAndSetNewTarget());
            }

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                StartCoroutine(WaitAndSetNewTarget());
            }
        }
        else
        {
            isWalking = false;
        }
    }

    IEnumerator WaitAndSetNewTarget()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        SetRandomPatrolTarget();
        isWaiting = false;
    }

    void SetRandomPatrolTarget()
    {
        Vector2 randomPoint = Random.insideUnitCircle * patrolRadius;
        targetPosition = patrolCenter + new Vector3(randomPoint.x, randomPoint.y, 0f); 
    }

    void ChasePlayer()
    {
        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            Vector3 newPosition = transform.position + direction * moveSpeed * Time.deltaTime;

            if (!IsPathBlocked(newPosition))
            {
                transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
                UpdateFlipDirection(player.position);
                isWalking = true;
            }
            else
            {
                isWalking = false;
            }
        }
    }

    void UpdateFlipDirection(Vector3 targetPosition)
    {
        if (targetPosition.x < transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }

    bool IsPathBlocked(Vector3 targetPosition)
    {
        Vector2 direction = (targetPosition - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, targetPosition);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, combinedObstacleLayer);
        if (hit.collider != null)
        {
            return true;
        }
        return false;
    }

    bool HasLineOfSightToPlayer()
    {
        if (player == null) return false;

        Vector2 direction = (player.position - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, player.position);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, combinedObstacleLayer);
        if (hit.collider != null)
        {
            return false;
        }
        return true;
    }

    private LayerMask CombineLayerMasks(LayerMask[] layers)
    {
        int combinedMask = 0;
        if (layers != null)
        {
            foreach (LayerMask layer in layers)
            {
                combinedMask |= layer.value;
            }
        }
        return combinedMask;
    }

    void OnDrawGizmos()
    {
        // Vẽ vòng tròn tuần tra
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(patrolCenter, patrolRadius);

        // Vẽ đường đến điểm đích
        Gizmos.color = Color.red;
        if (!isChasing)
        {
            Gizmos.DrawLine(transform.position, targetPosition);
        }
        else if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            float distance = Vector3.Distance(transform.position, player.position);
            Gizmos.DrawRay(transform.position, direction * distance);
        }
    }
}