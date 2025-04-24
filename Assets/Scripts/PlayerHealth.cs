using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public int maxMana = 100;
    public int currentMana = 0;
    public int maxDEF = 100;
    public int currentDEF = 0;

    public HealthBar healthBar;
    public HealthBar manaBar;
    public HealthBar defBar;

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        if (healthBar != null) healthBar.SetMaxHealth(maxHealth);
        if (manaBar != null) manaBar.SetMaxMana(maxMana);
        if (defBar != null) defBar.SetMaxDEF(maxDEF);
        UpdateHealthBar();
        UpdateManaBar();
        UpdateDEFBar();
    }

    public void TakeDamage(int damage)
{
    if (isDead) return;

    int remainingDamage = damage;
    if (currentDEF > 0)
    {
        int defDamage = Mathf.Min(currentDEF, remainingDamage);
        currentDEF -= defDamage;
        remainingDamage -= defDamage;
        UpdateDEFBar();
        Debug.Log($"Player DEF reduced by {defDamage}. Remaining DEF: {currentDEF}");
    }

    if (remainingDamage > 0)
    {
        currentHealth -= remainingDamage;
        Debug.Log($"Player HP reduced by {remainingDamage}. Remaining HP: {currentHealth}");
    }

    if (currentHealth <= 0)
    {
        currentHealth = 0;
        UpdateHealthBar();
        Die();
    }
    else
    {
        UpdateHealthBar();
    }
}

    private void Die()
    {
        isDead = true;
        Debug.Log("Player has died");
        // Thêm logic chết của Player nếu cần
    }

    public void Heal(int amount)
    {
        if (isDead) return;

        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void AddMana(int amount)
    {
        if (isDead) return;

        currentMana += amount;
        if (currentMana > maxMana) currentMana = maxMana;
        UpdateManaBar();
    }

    public void AddDEF(int amount)
    {
        if (isDead) return;

        currentDEF += amount;
        if (currentDEF > maxDEF) currentDEF = maxDEF;
        UpdateDEFBar();
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null) healthBar.SetHealth(currentHealth);
    }

    private void UpdateManaBar()
    {
        if (manaBar != null) manaBar.SetMana(currentMana);
    }

    private void UpdateDEFBar()
    {
        if (defBar != null) defBar.SetDEF(currentDEF);
    }
}