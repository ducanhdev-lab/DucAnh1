using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaCham : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy")) // Kiểm tra nếu va chạm với Enemy
        {
            Debug.Log("Player đã va chạm với Enemy, chuyển sang CombatScene!");

            // Gửi thông tin vào CombatManager
            CombatManager.Instance.StartCombat();
        }
    }
}
