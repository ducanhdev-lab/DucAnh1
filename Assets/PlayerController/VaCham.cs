using UnityEngine;

public class VaCham : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            CombatManager.Instance.StartCombat();
        }
    }
}