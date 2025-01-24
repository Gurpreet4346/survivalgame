using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 3.0f;
    public float jumpHeight = 2.0f;
    public float sprintSpeed = 5.0f;
    public float dashSpeed = 30.0f;
    public float dashDistance = 10.0f;
    public float dashDeceleration = 30.0f;
    public float lookSensitivity = 2.0f;
    public float gravityMultiplier = 2.0f;
    public Camera playerCamera;

    private CharacterController characterController;
    private Vector3 moveDirection;
    private float verticalVelocity;
    private bool isDashing = false;
    private float dashStartTime;
    private float currentDashSpeed;
    private Vector3 dashDirection;
    private PlayerStats playerStats;
    private int health;
    public float pushForce = 5f; // Force to apply to objects
    private Rigidbody rb;

    


    public void SetStats(int health, float speed)
    {
        this.health = health;
        this.moveSpeed = speed;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        characterController = GetComponent<CharacterController>();
        playerStats = GetComponent<PlayerStats>();

        if (playerCamera == null)
        {
            Debug.LogError("Player Camera is not assigned in the Inspector.");
        }
    }

    void Update()
    {
        if (isDashing)
        {
            PerformDash();
        }
        else
        {
            Move();
        }

        LookAround();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Collided with: {other.gameObject.name}");

        // Get the Rigidbody of the collided object
        Rigidbody rb = other.GetComponent<Rigidbody>();

        if (rb != null && !rb.isKinematic)
        {
            Debug.Log($"Applying force to: {other.gameObject.name}");

            // Calculate the direction of the push
            Vector3 pushDirection = other.transform.position - transform.position;
            pushDirection.y = 0; // Keep the push horizontal

            // Apply force to the object
            rb.AddForce(pushDirection.normalized * pushForce, ForceMode.Impulse);
        }
    }



    void Move()
    {
        float speed = moveSpeed;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W) && playerStats.CanSprint())
        {
            speed = sprintSpeed;
            playerStats.StartSprint();
        }
        else
        {
            playerStats.StopSprint();
        }

        if (Input.GetMouseButtonDown(2))
        {
            StartDash();
        }

        moveDirection = new Vector3(horizontal, 0, vertical);
        moveDirection = transform.TransformDirection(moveDirection) * speed;

        if (characterController.isGrounded)
        {
            verticalVelocity = -2f;

            if (Input.GetButtonDown("Jump"))
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y * gravityMultiplier);
            }
        }

        verticalVelocity += Physics.gravity.y * gravityMultiplier * Time.deltaTime;
        moveDirection.y = verticalVelocity;

        characterController.Move(moveDirection * Time.deltaTime);
    }

    void StartDash()
    {
        if (playerStats.CanDash())
        {
            playerStats.Dash();
            isDashing = true;
            dashStartTime = Time.time;
            dashDirection = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
            currentDashSpeed = dashSpeed;
        }
    }

    void PerformDash()
    {
        float dashDuration = dashDistance / dashSpeed;
        if (Time.time - dashStartTime < dashDuration)
        {
            currentDashSpeed = Mathf.Max(0, currentDashSpeed - dashDeceleration * Time.deltaTime);
            Vector3 dashMovement = dashDirection * currentDashSpeed * Time.deltaTime;
            dashMovement.y = -2f * Time.deltaTime;
            characterController.Move(dashMovement);
        }
        else
        {
            isDashing = false;
        }
    }

    void LookAround()
    {
        if (playerCamera == null) return;

        float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity;

        transform.Rotate(0, mouseX, 0);
        playerCamera.transform.Rotate(-mouseY, 0, 0);
    }

}
