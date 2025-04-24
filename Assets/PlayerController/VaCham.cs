using UnityEngine;

public class VaCham : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Player caught! Starting puzzle.");
            CombatManager.Instance.StartCombat(); // Dùng StartCombat để vào CombatScene
        }
    }
}