using UnityEngine;

public class FistScript : MonoBehaviour
{
    public float damage = 10f; // Damage dealt by the fist
    public float knockbackForce = 5f; // Knockback force applied to the enemy
    public float knockbackVerticalForce = 2f; // Vertical force applied during knockback

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) // Ensure enemies are tagged as "Enemy"
        {
            Debug.Log($"{gameObject.name} hit: {other.name}");

            // Apply damage and knockback
            ScarecrowController enemy = other.GetComponent<ScarecrowController>();
            if (enemy != null)
            {
                Vector3 knockbackDirection = (other.transform.position - transform.position).normalized;
                enemy.TakeDamage(damage, knockbackDirection);
            }
        }
    }
}
