using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public float damage = 15f; // Damage dealt to the player
    public float lifetime = 25f; // Lifetime of the projectile

    private void Start()
    {
        Debug.Log($"Projectile spawned at {transform.position}, lifetime: {lifetime}");
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Projectile hit the player!");

            // Access the player's stats script and apply damage
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(damage);

                // Calculate the knockback direction (away from the projectile)
                Vector3 knockbackDirection = (other.transform.position - transform.position).normalized;
                knockbackDirection.y = 1f; // Add slight upward knockback
                playerStats.ApplyKnockback(knockbackDirection, 10f); // Pass knockback strength
            }

            // Destroy the projectile after hitting the player
            Destroy(gameObject);
        }
       
    }




}
