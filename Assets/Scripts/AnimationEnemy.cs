using UnityEngine;

public class AnimationEnemy : MonoBehaviour
{
    [SerializeField] private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("AnimationEnemy: Animator component not found on Enemy!");
        }
    }

    public void HandleHPAnimation()
    {
        if (animator != null)
        {
            string trigger = CombatManager.Instance.IsBotTurn ? "Attack" : "Hit";
            animator.SetTrigger(trigger);
            Debug.Log($"Enemy HP Animation triggered: {trigger}");
        }
    }

    public void HandleDeathAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("Death");
            Debug.Log("Enemy Death Animation triggered");
        }
    }
}