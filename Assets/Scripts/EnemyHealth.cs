using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyHealth : MonoBehaviour
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
        healthBar.SetMaxHealth(maxHealth);
        UpdateHealthBar();

        manaBar.SetMaxMana(maxMana);
        manaBar.SetMana(currentMana);

        defBar.SetMaxDEF(maxDEF);
        defBar.SetDEF(currentDEF);

        // Đảm bảo Enemy có tên duy nhất
        if (string.IsNullOrEmpty(gameObject.name) || gameObject.name == "Enemy(Clone)")
        {
            gameObject.name = "Enemy_" + Random.Range(1000, 9999);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

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
        Debug.Log("Enemy has died");

        AnimationEnemy animE = GetComponent<AnimationEnemy>();
        if (animE != null)
        {
            animE.HandleDeathAnimation();
        }

        Invoke("ReturnToMovingScene", 1f);
    }

    private void ReturnToMovingScene()
    {
        SceneManager.LoadScene("MovingScene");
    }

    public void Heal(int amount)
    {
        if (isDead) return;

        currentHealth += amount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        UpdateHealthBar();
    }

    public void AddMana(int amount)
    {
        if (isDead) return;

        currentMana += amount;

        if (currentMana > maxMana)
        {
            currentMana = maxMana;
        }
        UpdateManaBar();
    }

    public void AddDEF(int amount)
    {
        if (isDead) return;

        currentDEF += amount;

        if (currentDEF > maxDEF)
        {
            currentDEF = maxDEF;
        }
        UpdateDEFBar();
    }
    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }
    }

    private void UpdateManaBar()
    {
        if (manaBar != null)
        {
            manaBar.SetMana(currentMana);
        }
    }

    private void UpdateDEFBar()
    {
        if (defBar != null)
        {
            defBar.SetDEF(currentDEF);
        }
    }
}