using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Thêm namespace cho UI

public class Victory : MonoBehaviour
{
    [Header("Victory Conditions")]
    private int[] items;                          // Mảng chứa ID vật phẩm yêu cầu
    private int itemsCollected = 0;               // Số vật phẩm đã thu thập
    private int requiredItems;                    // Số vật phẩm cần thiết
    private List<int> collectedItemIds = new List<int>(); // Danh sách ID đã nhặt
    private ItemManager itemManager;              // Tham chiếu đến ItemManager

    [Header("UI Toggles")]
    [SerializeField] private Toggle[] itemToggles; // Mảng các Toggle trên UI

    void Start()
    {
        // Tìm GameObject chứa ItemManager
        itemManager = FindObjectOfType<ItemManager>();
        if (itemManager == null)
        {
            Debug.LogError("ItemManager not found in scene!");
            items = new int[] { 0, 1, 2 }; // Giá trị mặc định nếu không tìm thấy
        }
        else
        {
            items = itemManager.GetRequiredItemIds(); // Lấy danh sách ID từ ItemManager
        }

        requiredItems = items.Length > 0 ? items.Length : 3;
        Debug.Log($"Required items: {requiredItems}");

        // Kiểm tra số lượng Toggle có khớp với số vật phẩm yêu cầu không
        if (itemToggles.Length != requiredItems)
        {
            Debug.LogWarning("Number of UI Toggles does not match required items!");
        }

        // Khởi tạo tất cả Toggle là false
        foreach (var toggle in itemToggles)
        {
            if (toggle != null)
            {
                toggle.isOn = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Item"))
        {
            Item item = other.GetComponent<Item>();
            if (item != null && System.Array.Exists(items, id => id == item.id) && !collectedItemIds.Contains(item.id))
            {
                collectedItemIds.Add(item.id);
                itemsCollected++;
                Debug.Log($"Collected item ID: {item.id}. Total: {itemsCollected}/{requiredItems}");

                // Cập nhật Toggle tương ứng trên UI
                UpdateUIToggle(item.id);

                Destroy(other.gameObject);

                if (itemsCollected >= requiredItems)
                {
                    CombatManager.Instance.OnAllItemsCollected(); // Thông báo cho CombatManager
                }
            }
        }
    }

    private void UpdateUIToggle(int itemId)
    {
        // Tìm chỉ số của itemId trong mảng items
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == itemId && i < itemToggles.Length && itemToggles[i] != null)
            {
                itemToggles[i].isOn = true; // Kích hoạt Toggle tương ứng
                break;
            }
        }
    }

    public bool HasAllItems()
    {
        return itemsCollected >= requiredItems;
    }
}