using System.Collections;
using UnityEngine;

public class Action_Enemy : MonoBehaviour
{
    [Header("Basic Attributes")]
    public float moveSpeed = 2f;
    public Transform[] patrolPoints;
    public float chaseRange = 5f;

    private Transform player;
    private int currentPatrolIndex = 0;
    private bool isChasing = false;
    private bool isWalking = false;
    private bool isWaiting = false;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Lấy SpriteRenderer để thay đổi Flip
    }

    void Update()
    {
        if (player != null && Vector3.Distance(transform.position, player.position) <= chaseRange)
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
        if (patrolPoints.Length > 0 && !isWaiting && !isChasing)
        {
            Transform targetPatrolPoint = patrolPoints[currentPatrolIndex];
            // Di chuyển tới điểm tuần tra
            transform.position = Vector3.MoveTowards(transform.position, targetPatrolPoint.position, moveSpeed * Time.deltaTime);

            // Cập nhật hướng (Flip) dựa trên hướng di chuyển
            UpdateFlipDirection(targetPatrolPoint.position);

            if (Vector3.Distance(transform.position, targetPatrolPoint.position) < 0.1f)
            {
                StartCoroutine(WaitAtPatrolPoint());
            }

            isWalking = true; // Đang di chuyển
        }
        else
        {
            isWalking = false;
        }
    }

    IEnumerator WaitAtPatrolPoint()
    {
        isWaiting = true;
        yield return new WaitForSeconds(1f);
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        isWaiting = false;
    }

    void ChasePlayer()
    {
        if (player != null)
        {
            // Di chuyển về phía Player
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

            // Cập nhật hướng (Flip) dựa trên hướng di chuyển
            UpdateFlipDirection(player.position);

            isWalking = true; // Đang di chuyển
        }
    }

    /// <summary>
    /// Cập nhật Flip dựa trên hướng di chuyển.
    /// </summary>
    /// <param name="targetPosition">Vị trí mục tiêu.</param>
    void UpdateFlipDirection(Vector3 targetPosition)
    {
        // Nếu di chuyển về bên trái thì bật FlipX
        if (targetPosition.x < transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }
}
