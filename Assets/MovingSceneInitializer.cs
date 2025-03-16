using UnityEngine;

public class MovingSceneInitializer : MonoBehaviour
{
    void Start()
    {
        // Tìm tất cả Enemy trong scene
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            // Đảm bảo Enemy có tên
            if (string.IsNullOrEmpty(enemy.name) || enemy.name == "Enemy_Slime")
            {
                enemy.name = "Enemy_Slime_" + Random.Range(1000, 9999);
            }

            // Nếu Enemy đã được đánh dấu là va chạm trước đó, xóa nó
            if (EnemyTracker.IsEnemyDefeated(enemy.name))
            {
                Destroy(enemy);
                Debug.Log($"Enemy {enemy.name} was previously defeated and removed on scene load");
            }
        }
    }
}