/*
 * Name: Ciaran McIlvaney
 */

using System.Collections;
using UnityEngine;
using Snowmobile;
using TMPro;

public class SnowmobileMount : MonoBehaviour
{
    [Header("Player")]
    public Transform player;
    public CharacterController playerController;
    public CharacterMove playerMovement;
    public CapsuleCollider playerCapsuleCollider;
    public MeshRenderer playerMeshRenderer;
    public GameObject playerCharacter;

    [Header("Player Camera Rig")]
    public Eyeballs playerLook;
    public CameraCollision playerCameraCollision;
    public Camera playerCamera;

    [Header("Snowmobile")]
    public SnowmobileInput snowmobileInput;
    public Camera snowmobileCamera;
    public Rigidbody snowmobileRigidbody;

    [Header("Mount Points")]
    public Transform seatPoint;
    public Transform exitPoint;

    [Header("Interaction")]
    public KeyCode interactKey = KeyCode.E;
    public float remountDelay = 0.25f;

    [Header("UI")]
    public TextMeshProUGUI interactText;

    [Header("HUD")]
    public GameObject snowmobileHUD;
    public GameObject staminaHUD;

    private bool playerInRange = false;
    private bool mounted = false;
    private bool canInteract = true;

    void Start()
    {
        if (snowmobileInput != null)
            snowmobileInput.enabled = false;

        if (snowmobileCamera != null)
            snowmobileCamera.gameObject.SetActive(false);

        if (snowmobileHUD != null)
            snowmobileHUD.SetActive(false);

        if (staminaHUD != null)
            staminaHUD.SetActive(true);
    }

    void Update()
    {
        // Snowmobile prompt
        if (interactText != null)
        {
            if (mounted)
            {
                interactText.text = "Press E to dismount";
            }
            else if (playerInRange && canInteract)
            {
                interactText.text = "Press E to ride Snowmobile";
            }
        }

        if (!canInteract)
            return;

        if (Input.GetKeyDown(interactKey))
        {
            Debug.Log("Pressed E. Mounted = " + mounted + " | InRange = " + playerInRange);

            if (!mounted && playerInRange)
            {
                MountSnowmobile();
            }
            else if (mounted)
            {
                StartCoroutine(DismountSnowmobile());
            }
        }
    }

    void MountSnowmobile()
    {
        mounted = true;
        playerInRange = false;
        canInteract = false;

        if (playerMovement != null)
            playerMovement.enabled = false;

        if (playerController != null)
            playerController.enabled = false;

        if (playerCapsuleCollider != null)
            playerCapsuleCollider.enabled = false;

        if (playerMeshRenderer != null)
            playerMeshRenderer.enabled = false;

        if (playerLook != null)
            playerLook.enabled = false;

        if (playerCameraCollision != null)
            playerCameraCollision.enabled = false;

        player.SetParent(seatPoint);
        player.localPosition = Vector3.zero;
        player.localRotation = Quaternion.identity;

        if (playerCamera != null)
            playerCamera.gameObject.SetActive(false);

        if (snowmobileCamera != null)
            snowmobileCamera.gameObject.SetActive(true);

        if (snowmobileInput != null)
            snowmobileInput.enabled = true;

        if (snowmobileHUD != null)
            snowmobileHUD.SetActive(true);

        if (staminaHUD != null)
            staminaHUD.SetActive(false);

        StartCoroutine(RemountCooldown());
        Debug.Log("Mounted snowmobile");
    }

    IEnumerator DismountSnowmobile()
    {
        mounted = false;
        canInteract = false;

        if (snowmobileInput != null)
            snowmobileInput.enabled = false;

        if (snowmobileRigidbody != null && snowmobileRigidbody.velocity.magnitude < 3f)
        {
            snowmobileRigidbody.velocity = Vector3.zero;
            snowmobileRigidbody.angularVelocity = Vector3.zero;
        }

        player.SetParent(null);

        if (playerController != null)
            playerController.enabled = false;

        if (playerCapsuleCollider != null)
            playerCapsuleCollider.enabled = false;

        player.position = exitPoint.position + Vector3.up * 0.1f;
        player.rotation = exitPoint.rotation;

        if (snowmobileCamera != null)
            snowmobileCamera.gameObject.SetActive(false);

        if (playerCamera != null)
            playerCamera.gameObject.SetActive(true);

        yield return null;

        if (playerCapsuleCollider != null)
            playerCapsuleCollider.enabled = true;

        if (playerController != null)
            playerController.enabled = true;

        if (playerMovement != null)
            playerMovement.enabled = true;

        if (playerMeshRenderer != null)
            playerMeshRenderer.enabled = true;

        if (playerLook != null)
        {
            playerLook.ResetLook();
            playerLook.enabled = true;
        }

        if (playerCameraCollision != null)
            playerCameraCollision.enabled = true;

        if (snowmobileHUD != null)
            snowmobileHUD.SetActive(false);

        if (staminaHUD != null)
            staminaHUD.SetActive(true);

        StartCoroutine(RemountCooldown());
        Debug.Log("Dismounted snowmobile");
    }

    IEnumerator RemountCooldown()
    {
        yield return new WaitForSeconds(remountDelay);
        canInteract = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Player entered snowmobile zone");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Player left snowmobile zone");
        }
    }
}