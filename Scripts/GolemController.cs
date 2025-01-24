using UnityEngine;
using System.Collections;

public class GolemController : MonoBehaviour
{
    public float maxHealth = 200f;
    public float walkSpeed = 3f;
    public float chargeSpeed = 15f;
    public float chargeDuration = 1.5f;
    public float idleCooldown = 3f;
    public float damageToPlayer = 50f;

    private float currentHealth;
    private Transform player;
    private Rigidbody rb;
    private bool isCharging = false;
    private bool isCoolingDown = false;
    private Vector3 chargeDirection;

    public Animator animator;

    private void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.useGravity = true;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }

        StartCoroutine(FindPlayer());
    }

    private void Update()
    {
        if (player == null) return;

        if (!isCharging && !isCoolingDown)
        {
            RotateTowardsPlayer();
            MoveTowardsPlayer();
            StartCharge();
        }
    }

    private IEnumerator FindPlayer()
    {
        while (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
                Debug.Log($"Player assigned to Golem: {player.name}");
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void RotateTowardsPlayer()
    {
        if (!isCharging)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Vector3 nextPosition = transform.position + direction * walkSpeed * Time.deltaTime;
        rb.MovePosition(nextPosition);
    }

    private void StartCharge()
    {
        if (!isCharging && !isCoolingDown)
        {
            StartCoroutine(ChargeCoroutine());
        }
    }

    private IEnumerator ChargeCoroutine()
    {
        isCharging = true;

        // Trigger prepare animation
        animator.SetTrigger("PrepareCharge");

        // Lock the charge direction
        chargeDirection = (player.position - transform.position).normalized;
        chargeDirection.y = 0;

        yield return new WaitForSeconds(1f);

        // Trigger charge animation
        animator.SetTrigger("Charge");

        float elapsed = 0f;
        while (elapsed < chargeDuration)
        {
            Vector3 nextPosition = transform.position + chargeDirection * chargeSpeed * Time.deltaTime;
            rb.MovePosition(nextPosition);
            elapsed += Time.deltaTime;
            yield return null;
        }

        isCharging = false;
        isCoolingDown = true;

        // Trigger idle animation
        animator.SetTrigger("Idle");

        yield return new WaitForSeconds(idleCooldown);
        isCoolingDown = false;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        ScoreManager.Instance.AddScore(50);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isCharging && collision.gameObject.CompareTag("Player"))
        {
            PlayerStats playerStats = collision.gameObject.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(damageToPlayer);
            }
        }
    }
}
