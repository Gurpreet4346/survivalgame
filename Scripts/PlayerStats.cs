using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerStats : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("Stamina Settings")]
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaRegenRate = 5f;
    public float staminaSprintConsumption = 10f;
    public float staminaDashConsumption = 50f;
    private bool isSprinting = false;
    private bool isBlinking = false;

    [Header("UI Elements")]
    public Image healthBar;
    public Image staminaBar;
    public Color normalStaminaColor = Color.green;
    public Color lowStaminaColor = Color.red;

    private void Start()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        healthBar = GameObject.Find("HealthBar").GetComponent<Image>();
        staminaBar = GameObject.Find("StaminaBar").GetComponent<Image>();

        if (staminaBar != null)
        {
            staminaBar.color = normalStaminaColor;
        }
    }

    private void Update()
    {
        if (isSprinting && currentStamina > 0)
        {
            ConsumeStamina(staminaSprintConsumption * Time.deltaTime);
        }
        else
        {
            RegenerateStamina();
        }

        if (currentStamina < maxStamina * 0.2f || (Input.GetMouseButtonDown(2) && !CanDash()))
        {
            if (!isBlinking)
            {
                StartCoroutine(BlinkStaminaBar(2));
            }
        }

        UpdateUI();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player has died!");
        var scoreManager = Object.FindFirstObjectByType<ScoreManager>();
        if (scoreManager != null)
        {
            scoreManager.StopScoring();
        }

        // Additional player death logic (e.g., respawn, game over screen).
    }

    public bool CanSprint()
    {
        return currentStamina > 0;
    }

    public bool CanDash()
    {
        return currentStamina >= staminaDashConsumption;
    }

    public void Dash()
    {
        if (CanDash())
        {
            ConsumeStamina(staminaDashConsumption);
            Debug.Log("Player Dashed");
        }
    }

    private void ConsumeStamina(float amount)
    {
        currentStamina -= amount;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
    }

    private void RegenerateStamina()
    {
        if (!isSprinting)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        }
    }

    private void UpdateUI()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = currentHealth / maxHealth;
        }

        if (staminaBar != null)
        {
            staminaBar.fillAmount = currentStamina / maxStamina;
        }
    }

    private IEnumerator BlinkStaminaBar(int blinkCount)
    {
        isBlinking = true;

        for (int i = 0; i < blinkCount; i++)
        {
            if (staminaBar != null)
            {
                staminaBar.color = lowStaminaColor;
                yield return new WaitForSeconds(0.5f);

                staminaBar.color = normalStaminaColor;
                yield return new WaitForSeconds(0.5f);
            }
        }

        isBlinking = false;
    }

    public void StartSprint()
    {
        isSprinting = true;
    }

    public void StopSprint()
    {
        isSprinting = false;
    }
    public void ApplyKnockback(Vector3 direction, float strength)
    {
        StartCoroutine(KnockbackCoroutine(direction, strength));
    }

    private IEnumerator KnockbackCoroutine(Vector3 direction, float strength)
    {
        float knockbackDuration = 0.2f; // Duration of the knockback
        float elapsedTime = 0f;

        while (elapsedTime < knockbackDuration)
        {
            // Move the player in the knockback direction
            transform.Translate(direction * strength * Time.deltaTime, Space.World);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    public void RegenerateHealth()
    {
        currentHealth = maxHealth;
        Debug.Log($"Health fully regenerated. Current health: {currentHealth}");

        // Update health UI if applicable
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        // Example logic to update the health bar UI
        // Ensure this matches your existing health bar implementation
        float healthPercentage = currentHealth / maxHealth;
        // Update your health bar UI here
    }
}
