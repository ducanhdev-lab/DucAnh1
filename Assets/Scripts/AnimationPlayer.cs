using UnityEngine;

public class AnimationPlayer : MonoBehaviour
{
    [SerializeField] private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("AnimationPlayer: Animator component not found on Player!");
        }
    }

    public void HandleHPAnimation()
    {
        if (animator != null)
        {
            string trigger = CombatManager.Instance.IsBotTurn ? "Hit" : "Attack";
            animator.SetTrigger(trigger);
            Debug.Log($"Player HP Animation triggered: {trigger}");
        }
    }

    public void HandleDeathAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("Death");
            Debug.Log("Player Death Animation triggered");
        }
    }
}