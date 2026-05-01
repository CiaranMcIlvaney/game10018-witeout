/*
 * Name: Ciaran McIlvaney
 */

using UnityEngine;

public class Eyeballs : MonoBehaviour
{
    [Header("Mouse Settings")]
    public float mouseSensitivity = 5f;
    public float smoothing = 1.5f;

    [Header("References")]
    public Transform bodyToRotate;

    private Vector2 mouseLook;
    private Vector2 smoothMovement;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        // If no body assigned in inspector, try using parent
        if (bodyToRotate == null && transform.parent != null)
        {
            bodyToRotate = transform.parent;
        }

        ResetLook();
    }

    void OnEnable()
    {
        ResetLook();
    }

    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;

        Vector2 mouseDirection = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        mouseDirection.x *= mouseSensitivity * smoothing;
        mouseDirection.y *= mouseSensitivity * smoothing;

        smoothMovement.x = Mathf.Lerp(smoothMovement.x, mouseDirection.x, 1f / smoothing);
        smoothMovement.y = Mathf.Lerp(smoothMovement.y, mouseDirection.y, 1f / smoothing);

        mouseLook += smoothMovement;
        mouseLook.y = Mathf.Clamp(mouseLook.y, -80f, 90f);

        // vertical camera look
        transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);

        // horizontal player/body rotation
        if (bodyToRotate != null)
        {
            bodyToRotate.rotation = Quaternion.AngleAxis(mouseLook.x, Vector3.up);
        }
    }

    public void ResetLook()
    {
        smoothMovement = Vector2.zero;

        // Sync stored values with current transforms
        float yaw = 0f;
        float pitch = 0f;

        if (bodyToRotate != null)
        {
            yaw = bodyToRotate.eulerAngles.y;
        }

        pitch = transform.localEulerAngles.x;
        if (pitch > 180f)
        {
            pitch -= 360f;
        }

        mouseLook = new Vector2(yaw, -pitch);
    }
}