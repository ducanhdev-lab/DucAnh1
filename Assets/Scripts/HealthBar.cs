using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient healthGradient;  // Gradient cho thanh máu
    public Gradient manaGradient;    // Gradient cho thanh mana
    public Gradient defGradient;     // Gradient cho thanh phòng thủ
    public Image fill;

        
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;

        fill.color = healthGradient.Evaluate(1f);
    }

    public void SetHealth(int health)
    {
        slider.value = health;
        fill.color = healthGradient.Evaluate(slider.normalizedValue);
    }
    public void SetMaxMana(int mana)
    {
        slider.maxValue = mana;
        slider.value = mana;

        fill.color = manaGradient.Evaluate(1f);
    }

    public void SetMana(int mana)
    {
        slider.value = mana;
        fill.color = manaGradient.Evaluate(slider.normalizedValue);
    }

    public void SetMaxDEF(int def)
    {
        slider.maxValue = def;
        slider.value = def;

        fill.color = defGradient.Evaluate(1f);
    }

    public void SetDEF(int def)
    {
        slider.value = def;
        fill.color = defGradient.Evaluate(slider.normalizedValue);
    }
}