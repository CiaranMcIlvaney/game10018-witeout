/*
 * Name: Ciaran McIlvaney
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eyeballs : MonoBehaviour
{
    // Mouse movement variables 
    public float mouseSensitivity = 5f;
    public float smoothing = 1.5f; // Smoother mouse movement 

    // Vectors to store the calculations
    private Vector2 mouseLook;
    private Vector2 smoothMovement;

    // Reference to the player
    private GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        // Call reference to the player
        player = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        // This will make your cursor invisible in the game
        // Press ESC to get cursor back
        Cursor.lockState = CursorLockMode.Locked;

        // Variable for mouse movemment
        Vector2 mouseDirection = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        // Times the mouse input by the mouseSensitivity and smoothing
        mouseDirection.x *= mouseSensitivity * smoothing;
        mouseDirection.y *= mouseSensitivity * smoothing;

        // Linear interpolation between two positions. Moving between where the mouse is currently looking as well as the calculations done above
        smoothMovement.x = Mathf.Lerp(smoothMovement.x, mouseDirection.x, 1f / smoothing);
        smoothMovement.y = Mathf.Lerp(smoothMovement.y, mouseDirection.y, 1f / smoothing);

        // Add these calculations together
        mouseLook += smoothMovement;

        // Restrict the mouse position to make sure the player cannot rotate forever on the x-axis
        mouseLook.y = Mathf.Clamp(mouseLook.y, -80f, 90f);

        // Move the camera to the newest calculated position.
        transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);

        // Move the player object on the x-axis only
        player.transform.rotation = Quaternion.AngleAxis(mouseLook.x, player.transform.up);
    }
}
