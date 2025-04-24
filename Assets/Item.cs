using UnityEngine;

public class Item : MonoBehaviour
{
    public int id; // ID của vật phẩm, gán trong Inspector

    void Start()
    {
        if (!gameObject.CompareTag("Item"))
        {
            gameObject.tag = "Item";
        }
        Collider2D col = GetComponent<Collider2D>();
        if (col != null && !col.isTrigger)
        {
            col.isTrigger = true;
        }
    }
}