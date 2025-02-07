using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Front_Behind : MonoBehaviour
{
    public Transform player;
    [SerializeField] private SpriteRenderer[] treeRenderers;
    [SerializeField] private SpriteRenderer[] houseRenders;
    public Transform[] enemies;

    void Start()
    {
        treeRenderers = GetComponentsInChildren<SpriteRenderer>(); // Lấy các SpriteRenderer của cây
        houseRenders = GetComponentsInChildren<SpriteRenderer>(); // Lấy các SpriteRenderer của nhà
    }

    void Update()
    {
        UpdatePlayerAndEnemyOrder(); // Cập nhật order cho player và enemy
    }

    void UpdatePlayerAndEnemyOrder()
    {
        if (player == null || enemies == null) return;

        // Cập nhật sortingOrder cho player
        UpdateObjectOrder(player);

        // Cập nhật sortingOrder cho từng enemy
        foreach (Transform enemy in enemies)
        {
            if (enemy != null && enemy.CompareTag("Enemy"))
            {
                UpdateObjectOrder(enemy);
            }
        }
    }

    void UpdateObjectOrder(Transform obj)
    {
        if (obj == null) return;

        SpriteRenderer objRenderer = obj.GetComponent<SpriteRenderer>();
        if (objRenderer == null) return;

        float objY = obj.position.y;
        int sortingOrder = 0;

        // Tìm đối tượng gần nhất (tree hoặc house)
        Transform closestObject = FindClosestObject(obj);

        if (closestObject != null)
        {
            // So sánh với đối tượng gần nhất
            float closestObjectY = closestObject.position.y;
            if (objY > closestObjectY)
            {
                sortingOrder = -1; // Đối tượng ở dưới vật gần nhất
            }
            else
            {
                sortingOrder = 1; // Đối tượng ở trên vật gần nhất
            }
        }

        objRenderer.sortingOrder = sortingOrder;
    }

    Transform FindClosestObject(Transform obj)
    {
        Transform closestObject = null;
        float closestDistance = Mathf.Infinity;

        // Kiểm tra tất cả các đối tượng tree
        foreach (SpriteRenderer treeRenderer in treeRenderers)
        {
            if (treeRenderer != null && treeRenderer.CompareTag("Tree"))
            {
                float distance = Vector3.Distance(obj.position, treeRenderer.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestObject = treeRenderer.transform;
                }
            }
        }

        // Kiểm tra tất cả các đối tượng house
        foreach (SpriteRenderer houseRenderer in houseRenders)
        {
            if (houseRenderer != null && houseRenderer.CompareTag("House"))
            {
                float distance = Vector3.Distance(obj.position, houseRenderer.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestObject = houseRenderer.transform;
                }
            }
        }

        return closestObject;
    }
}
