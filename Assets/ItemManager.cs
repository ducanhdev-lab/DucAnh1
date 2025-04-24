using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [Header("Required Items")]
    public int[] requiredItemIds = new int[] { 0, 1, 2 }; // 3 ID vật phẩm cần nhặt

    // Phương thức để lấy danh sách ID
    public int[] GetRequiredItemIds()
    {
        return requiredItemIds;
    }

    void Start()
    {
        Debug.Log($"Required items set: {string.Join(", ", requiredItemIds)}");
    }
}