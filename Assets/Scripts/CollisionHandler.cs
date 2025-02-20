using System.Collections;
using System.Linq;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    public Transform enemyDestination;
    public Transform playerDestination;
    public GameObject combatCanvas;
    public float teleportDelay = 1f;

    [SerializeField] private Transform player;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private MonoBehaviour playerInputScript;
    [SerializeField] private MonoBehaviour enemyInputScript;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player != null)
        {
            playerAnimator = player.GetComponent<Animator>();
            playerInputScript = player.GetComponent<MonoBehaviour>();
        }

        if (combatCanvas != null)
        {
            combatCanvas.SetActive(false);
        }
    }

    IEnumerator HandleTeleport(Transform enemyTransform)
    {
        if (playerInputScript != null) playerInputScript.enabled = false;
        if (enemyInputScript != null) enemyInputScript.enabled = false;

        if (playerAnimator != null && playerAnimator.parameters.Any(param => param.name == "isMove"))
        {
            playerAnimator.SetBool("isMove", false);
        }

        yield return new WaitForSeconds(teleportDelay);

        if (combatCanvas != null)
        {
            combatCanvas.SetActive(true);
        }

        if (playerDestination != null && enemyDestination != null)
        {
            player.position = playerDestination.position;
            enemyTransform.position = enemyDestination.position;

            Animator enemyAnimator = enemyTransform.GetComponent<Animator>();
            if (enemyAnimator != null && enemyAnimator.parameters.Any(param => param.name == "isWalking"))
            {
                enemyAnimator.SetBool("isWalking", false);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Transform enemyTransform = transform;

            enemyInputScript = GetComponent<MonoBehaviour>();

            StartCoroutine(HandleTeleport(enemyTransform));
        }
    }
}
