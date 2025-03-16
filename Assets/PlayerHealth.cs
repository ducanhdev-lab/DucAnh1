using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public int MaxMana = 100;
    public int currentMana = 0;
    public int MaxDEF = 100;
    public int currentDEF = 0;
    
    public HealthBar healthBar;
    public HealthBar manaBar;
    public HealthBar defBar;

    void Start()
    {
    currentHealth = maxHealth;
    healthBar.SetMaxHealth(maxHealth);
    
    manaBar.SetMaxMana(MaxMana);
    manaBar.SetMana(currentMana);
    
    defBar.SetMaxDEF(MaxDEF);
    defBar.SetDEF(currentDEF);
    }

    void Update()
    {
        // For testing: Press space to take 10 damage
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        
        // Prevent health from going below zero
        if (currentHealth < 0)
            currentHealth = 0;
            UpdateHealthBar();
    }
    
    public void Heal(int amount)
    {
        currentHealth += amount;
        
        // Prevent health from exceeding max health
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
            UpdateHealthBar();
    }
    public void AddMana(int amount)
        {

        currentMana += amount;

        if (currentMana > MaxMana)
        {
            currentMana = MaxMana;
        }
            
            UpdateManaBar();
    }
    public void AddDEF(int amount) 
        {
        currentDEF += amount;

        if (currentDEF > MaxDEF)
            currentDEF = MaxDEF;
            UpdateDEFBar();
    }
    private void UpdateHealthBar()
    {
        healthBar.SetHealth(currentHealth);
    }
    private void UpdateManaBar()
    {
        manaBar.SetMana(currentMana);
    }
    private void UpdateDEFBar()
    {
        defBar.SetDEF(currentDEF);
    }
}
