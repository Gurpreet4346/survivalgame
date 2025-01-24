using UnityEngine;
using System.Collections;

public class ScarecrowController : MonoBehaviour
{
    public float maxHealth = 30f;
    private float currentHealth;

    public GameObject projectilePrefab; // The projectile prefab
    public Transform projectileSpawnPoint; // The point where projectiles are fired
    public float projectileSpeed = 5f; // Speed of the projectile
    public float attackCooldown = 2f; // Time between attacks
    public float damageToPlayer = 15f; // Damage dealt to the player
    public float knockbackForce = 5f; // Force applied when taking damage
    public float knockbackVerticalForce = 2f; // Vertical velocity applied during knockback
    public GameObject deathEffect; // Effect when scarecrow dies (e.g., smoke)
    public int scoreValue = 10; // Score added when the scarecrow is killed

    public Renderer scarecrowRenderer; // Renderer for changing the scarecrow's color
    private Color originalColor; // Store the original material color

    private Transform player; // Reference to the player
    private Rigidbody rb; // Rigidbody component
    private float lastAttackTime; // Tracks the last time an attack occurred
  


    private void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.useGravity = true; // Enable gravity
            rb.isKinematic = false; // Ensure it's non-kinematic
        }

        if (scarecrowRenderer != null)
        {
            originalColor = scarecrowRenderer.material.color; // Save the original color
        }

        FindPlayer(); // Dynamically assign the player
    }

    private void Update()
    {
        if (player == null)
        {
            FindPlayer(); // Continuously look for the player if not already assigned
            return;
        }

        RotateTowardsPlayer();

        // Despawn if the scarecrow is too far below the player (10 y-levels below)
        if (transform.position.y < player.position.y - 10f || transform.position.y > player.position.y + 5f)
        {
            Debug.Log($"Scarecrow at {transform.position} despawned because it was out of range.");
            Despawn();
            return;
        }

        TryAttack();
    }

    private void FindPlayer()
    {
        var playerObject = GameObject.FindFirstObjectByType<PlayerController>();
        if (playerObject != null)
        {
            player = playerObject.transform;
            Debug.Log($"Player dynamically assigned to {player.name}.");
        }
    }

    private void RotateTowardsPlayer()
    {
        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0; // Ignore vertical differences
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // Smooth rotation
        }
    }

    private void TryAttack()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
        }
    }

    private void Attack()
    {
        lastAttackTime = Time.time;

        // Instantiate the projectile
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);

        if (projectile != null)
        {
            Debug.Log($"Projectile spawned at {projectileSpawnPoint.position}");
        }
        else
        {
            Debug.LogError("Failed to spawn projectile!");
        }

        // Add velocity to the projectile
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
        if (projectileRb != null && player != null)
        {
            Vector3 direction = (player.position - projectileSpawnPoint.position).normalized;
            projectileRb.linearVelocity = direction * projectileSpeed; // Apply velocity for movement
            Debug.Log($"Projectile velocity set: {projectileRb.linearVelocity}");
        }
        else if (projectileRb == null)
        {
            Debug.LogError("Rigidbody not found on the projectile prefab!");
        }
    }


    public void TakeDamage(float damage, Vector3 knockbackDirection)
    {
        currentHealth -= damage;

        // Apply knockback force
        rb.AddForce(new Vector3(knockbackDirection.x, knockbackVerticalForce, knockbackDirection.z).normalized * knockbackForce, ForceMode.Impulse);

        // Blink red when taking damage
        if (scarecrowRenderer != null)
        {
            StartCoroutine(BlinkRed());
        }

        // Check if the scarecrow should die
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator BlinkRed()
    {
        MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
        Renderer renderer = scarecrowRenderer;

        // Set color to red
        renderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetColor("_Color", Color.red);
        renderer.SetPropertyBlock(propertyBlock);

        // Wait for a short duration
        yield return new WaitForSeconds(0.1f);

        // Revert to original color
        propertyBlock.SetColor("_Color", originalColor);
        renderer.SetPropertyBlock(propertyBlock);
    }




    private void Die()
    {
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity); // Trigger death effect
        }

        ScoreManager.Instance.AddScore(scoreValue); // Only add score if the player killed it

        Debug.Log($"Scarecrow at {transform.position} has died due to player attack.");
        Destroy(gameObject);
    }

    private void Despawn()
    {
        // Despawn logic (no score added)
        Debug.Log($"Scarecrow at {transform.position} has despawned.");
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the scarecrow's interaction trigger.");
            PushScarecrow();
        }

        if (other.CompareTag("PlayerFist"))
        {
            Vector3 attackDirection = (transform.position - other.transform.position).normalized;
            TakeDamage(10f, attackDirection); // Assume fists deal 10 damage
        }
    }

    private void PushScarecrow()
    {
        if (player != null && rb != null)
        {
            Vector3 pushDirection = (transform.position - player.position).normalized;
            rb.AddForce(new Vector3(pushDirection.x, knockbackVerticalForce, pushDirection.z).normalized * knockbackForce, ForceMode.Impulse);

            Debug.Log("Scarecrow pushed by player entering trigger.");
        }
    }
}
