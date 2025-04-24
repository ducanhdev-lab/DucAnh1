using UnityEngine;
using TMPro;

public class BlinkingText : MonoBehaviour
{
    private TextMeshProUGUI text;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (text != null)
        {
            Color color = text.color;
            color.a = (Mathf.Sin(Time.time * 6f) + 1f) * 0.5f; 
            text.color = color;
        }
    }
}