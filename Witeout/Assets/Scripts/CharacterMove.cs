/*
* Name: Ciaran McIlvaney
*/

using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 6f;
    public float sprintSpeed = 10f;
    public float jumpHeight = 8f;
    public float gravity = -20f;

    [Header("Stamina")]
    public float maxStamina = 5f;
    public float currentStamina = 5f;
    public float staminaDrainPerSecond = 2.5f;
    public float staminaRecoveryPerSecond = 1.5f;
    public float staminaRecoveryDelay = 1.25f;

    [Header("Push Force")]
    [Range(0f, 1f)]
    public float pushForce = 0.1f;

    private CharacterController controller;
    private Vector3 velocity;
    private Vector3 launchHorizontal = Vector3.zero;

    private float recoveryTimer = 0f;
    private bool isSprinting = false;

    public bool IsSprinting => isSprinting;
    public float StaminaPercent => maxStamina > 0f ? currentStamina / maxStamina : 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 inputDirection = (transform.forward * v + transform.right * h);
        bool isMoving = inputDirection.magnitude > 0.1f;
        bool sprintHeld = Input.GetKey(KeyCode.LeftShift);

        // Sprint logic
        isSprinting = false;
        float currentMoveSpeed = walkSpeed;

        if (sprintHeld && isMoving && currentStamina > 0f)
        {
            isSprinting = true;
            currentMoveSpeed = sprintSpeed;

            currentStamina -= staminaDrainPerSecond * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);

            recoveryTimer = staminaRecoveryDelay;
        }
        else
        {
            if (recoveryTimer > 0f)
            {
                recoveryTimer -= Time.deltaTime;
            }
            else
            {
                currentStamina += staminaRecoveryPerSecond * Time.deltaTime;
                currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
            }
        }

        Vector3 move = inputDirection * currentMoveSpeed;

        // Add jump pad movement
        move += launchHorizontal;

        if (controller.isGrounded)
        {
            if (velocity.y < 0)
            {
                velocity.y = -2f;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                velocity.y = jumpHeight;
            }
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move((move + velocity) * Time.deltaTime);

        launchHorizontal = Vector3.Lerp(launchHorizontal, Vector3.zero, 2f * Time.deltaTime);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.rigidbody;

        if (body == null || body.isKinematic)
        {
            return;
        }

        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        body.AddForce(pushDir * pushForce, ForceMode.Impulse);
    }

    public void Launch(Vector3 horizontalBoost, float verticalBoost)
    {
        launchHorizontal = horizontalBoost;
        velocity.y = verticalBoost;
    }
}
