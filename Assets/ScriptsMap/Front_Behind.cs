using UnityEngine;

public class Front_Behind : MonoBehaviour
{
    public Transform player;
    public Transform[] enemies;
    [SerializeField] private SpriteRenderer[] treeRenderers;
    [SerializeField] private SpriteRenderer[] houseRenderers;

    void Start()
    {
        // Lấy các SpriteRenderer của cây và nhà
        treeRenderers = GetComponentsInChildren<SpriteRenderer>();
        houseRenderers = GetComponentsInChildren<SpriteRenderer>();

        // Gán sorting order cố định cho cây và nhà dựa trên y
        SetStaticSortingOrder(treeRenderers);
        SetStaticSortingOrder(houseRenderers);
    }

    void Update()
    {
        UpdateDynamicSortingOrder(player); // Cập nhật cho Player
        foreach (Transform enemy in enemies)
        {
            UpdateDynamicSortingOrder(enemy); // Cập nhật cho Enemy
        }
    }

    void SetStaticSortingOrder(SpriteRenderer[] renderers)
    {
        foreach (SpriteRenderer renderer in renderers)
        {
            if (renderer != null)
            {
                // Sorting order dựa trên vị trí y (càng thấp càng ở trước)
                renderer.sortingOrder = Mathf.RoundToInt(-renderer.transform.position.y * 100);
            }
        }
    }

    void UpdateDynamicSortingOrder(Transform obj)
    {
        if (obj == null) return;

        SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            // Sorting order dựa trên y của Player/Enemy (càng thấp càng ở trước)
            renderer.sortingOrder = Mathf.RoundToInt(-obj.position.y * 100);
        }
    }
}