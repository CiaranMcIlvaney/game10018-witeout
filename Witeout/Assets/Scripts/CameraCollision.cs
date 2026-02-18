/*
 * Name: Ciaran McIlvaney
 */

using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    // The camera we want to move and protect from clipping through walls
    public Transform cameraTransform;   

    // The normal distance the camaera should stay behind the player
    public float maximumDistance = 0f;      

    // The closest distance the camera can move to avoid going through colliders
    public float minimumDistance = 0f;    

    // How smooth the camera will adjust when it moves
    public float cameraSmoothness = 0f;          

    // This will let you pick what layers the camera should collide with (which will be everything but neat to have just in case)
    public LayerMask collideWith;       

    // Keeps track of how far the camera currently is from the player
    private float currentDistance;

    // Called at the start of the game
    void Start()
    {
        // Set the starting distance to the maximum distance (regular camera position without collisions in the way)
        currentDistance = maximumDistance;
    }

    // Called every frame AFTER everything else (this is great for camera logic as it makes sure the camera moves on the most up to date final position)
    void LateUpdate()
    {
        // Calculate where the wants to try to be, behind the player
        Vector3 desiredPosition = transform.position - transform.forward * maximumDistance;

        // Stores info incase we hit something with our raycast
        RaycastHit hit;

        // Start by assuming the camera is able to stay at the max distance (it will)
        float targetDistance = maximumDistance;

        // Cast a ray backwards from the player to the wanted camera position, if it hits a colldier or something then we will just adjust the camera's distance
        if (Physics.Raycast(transform.position, -transform.forward, out hit, maximumDistance, collideWith))
        {
            // Move the camera just on front of the object we collided with but not too close
            targetDistance = Mathf.Clamp(hit.distance - 0.1f, minimumDistance, maximumDistance);
        }

        // Smoothly move the camera fro its current distance to the new target distance 
        currentDistance = Mathf.Lerp(currentDistance, targetDistance, cameraSmoothness * Time.deltaTime);

        // Update the camera's real position behind the player
        cameraTransform.position = transform.position - transform.forward * currentDistance;
    }
}
