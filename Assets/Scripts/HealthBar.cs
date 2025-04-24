using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider; // Slider để điều khiển giá trị
    public Image fill; // Image để hiển thị sprite
    public Sprite[] healthSprites; // 5 sprite cho thanh máu (0%, 25%, 50%, 75%, 100%)
    public Sprite[] manaSprites; // 5 sprite cho thanh mana
    public Sprite[] defSprites; // 5 sprite cho thanh DEF

    // Thiết lập giá trị tối đa và sprite ban đầu cho máu
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;

        UpdateSprite(healthSprites, slider.normalizedValue);
    }

    // Cập nhật giá trị máu và sprite
    public void SetHealth(int health)
    {
        slider.value = health;
        UpdateSprite(healthSprites, slider.normalizedValue);
    }

    // Thiết lập giá trị tối đa và sprite ban đầu cho mana
    public void SetMaxMana(int mana)
    {
        slider.maxValue = mana;
        slider.value = mana;

        UpdateSprite(manaSprites, slider.normalizedValue);
    }

    // Cập nhật giá trị mana và sprite
    public void SetMana(int mana)
    {
        slider.value = mana;
        UpdateSprite(manaSprites, slider.normalizedValue);
    }

    // Thiết lập giá trị tối đa và sprite ban đầu cho DEF
    public void SetMaxDEF(int def)
    {
        slider.maxValue = def;
        slider.value = def;

        UpdateSprite(defSprites, slider.normalizedValue);
    }

    // Cập nhật giá trị DEF và sprite
    public void SetDEF(int def)
    {
        slider.value = def;
        UpdateSprite(defSprites, slider.normalizedValue);
    }

    // Hàm cập nhật sprite dựa trên giá trị normalized của slider
    private void UpdateSprite(Sprite[] sprites, float normalizedValue)
    {
        if (sprites == null || sprites.Length != 5)
        {
            Debug.LogWarning("Sprite array must contain exactly 5 sprites!");
            return;
        }

        // Chia normalizedValue thành 5 mức (0%, 25%, 50%, 75%, 100%)
        int spriteIndex;
        if (normalizedValue >= 0.875f) // 87.5% - 100%
            spriteIndex = 4; // Sprite 100%
        else if (normalizedValue >= 0.625f) // 62.5% - 87.5%
            spriteIndex = 3; // Sprite 75%
        else if (normalizedValue >= 0.375f) // 37.5% - 62.5%
            spriteIndex = 2; // Sprite 50%
        else if (normalizedValue >= 0.125f) // 12.5% - 37.5%
            spriteIndex = 1; // Sprite 25%
        else // 0% - 12.5%
            spriteIndex = 0; // Sprite 0%

        // Gán sprite cho fill
        fill.sprite = sprites[spriteIndex];
    }
}