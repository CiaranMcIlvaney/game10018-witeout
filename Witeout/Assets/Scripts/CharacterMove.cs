/*
 * Name: Ciaran McIlvaney
 */

using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    // How fast the player moves left/right/forward/back
    public float moveSpeed = 6f;

    // How high the player can jump
    public float jumpHeight = 8f;

    // How strong gravity pulls the player down
    public float gravity = -20f;

    // How much force is applied when pushing objects with Rigidbodys 
    [Range(0f, 1f)] // This makes the float appear as a slider inside the inspector
    public float pushForce = 0.1f;

    // Reference to the character controller which handles movement and collisions
    private CharacterController controller;

    // Keeps track of the players current up/down speed 
    private Vector3 velocity;

    // Extra horizontal push from the jump pads / the swings
    private Vector3 launchHorizontal = Vector3.zero;

    // Called at the start of the game
    void Start()
    {
        // Finds the character controller connected to the player
        controller = GetComponent<CharacterController>();
    }

    // Called once every frame
    void Update()
    {
        // ----- MOVEMENT INPUT -----

        // Horizontal is A/D or Left and Right arrow keys 
        float h = Input.GetAxis("Horizontal");

        // Vertical is W/S or Up and Down arrow keys
        float v = Input.GetAxis("Vertical");

        // Combines the horizontal and vertical movement and uses the players forward/right directions so it moves where you are facing
        Vector3 move = (transform.forward * v + transform.right * h) * moveSpeed;

        // Add jump pad movement
        move += launchHorizontal;

        // ----- JUMPING AND GROUND CHECKING -----

        // If the player is touching the ground
        if (controller.isGrounded)
        {
            // Keeps the player sort of "stuck" to the ground so they dont float on the floor
            if (velocity.y < 0)
            {
                velocity.y = -2f;
            }
                
            // When you press Spacebar set the upward velocity Y
            if (Input.GetKeyDown(KeyCode.Space))
            {
                velocity.y = jumpHeight;
            }  
        }

        // ----- APPLY GRAVITY -----

        // Adds gravity every frame so you fall down naturally 
        velocity.y += gravity * Time.deltaTime;

        // ----- FINAL MOVEMENT -----

        // This moves the player based on input (move) and gravity (velocity) and Time.deltaTime makes the movement frame-rate independent
        controller.Move((move + velocity) * Time.deltaTime);

        // Reduce the launch speed overtime so it does not last forever
        launchHorizontal = Vector3.Lerp(launchHorizontal, Vector3.zero, 2f * Time.deltaTime);
    }

    // This function will automatically run when the character controller hits something (Unity is awesome I dont got to do any of this manually)
    void OnControllerColliderHit(ControllerColliderHit hit)
    { 
        // Try and get the Rigidbody of the thing you hit
        Rigidbody body = hit.rigidbody;

        // If it doesnt have a rigidbody or it is set to kinematic (ew) then skip it
        if (body == null || body.isKinematic)
        {
            return;
        }
           
        // Figure out which direction to push in only sideways no upside down
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        // Apply a small push to the object the ForceMode.Impulse means its giving the object a quick bump of force to get things going
        body.AddForce(pushDir * pushForce, ForceMode.Impulse);
    }

    // Called by JumpPad script to launch the player
    public void Launch(Vector3 horizontalBoost, float verticalBoost)
    {
        // Add extra horizontal velocity 
        launchHorizontal = horizontalBoost;

        // Set upward speed for a big jump
        velocity.y = verticalBoost;
    }
}