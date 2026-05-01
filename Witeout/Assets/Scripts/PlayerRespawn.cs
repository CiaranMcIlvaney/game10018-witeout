using System.Collections;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [Header("Respawn")]
    public Transform currentCheckpoint;
    public float respawnDelay = 1.5f;

    [Header("Player References")]
    public CharacterController characterController;
    public CharacterMove characterMove;
    public CapsuleCollider playerCapsuleCollider;

    private bool isRespawning = false;

    public void SetCheckpoint(Transform newCheckpoint)
    {
        currentCheckpoint = newCheckpoint;
        Debug.Log("Checkpoint updated to: " + newCheckpoint.name);
    }

    public void KillPlayer()
    {
        if (isRespawning) return;

        StartCoroutine(RespawnPlayer());
    }

    IEnumerator RespawnPlayer()
    {
        isRespawning = true;

        Debug.Log("Player died. Respawning...");

        if (characterMove != null)
            characterMove.enabled = false;

        if (characterController != null)
            characterController.enabled = false;

        if (playerCapsuleCollider != null)
            playerCapsuleCollider.enabled = false;

        yield return new WaitForSeconds(respawnDelay);

        if (currentCheckpoint != null)
        {
            transform.position = currentCheckpoint.position;
            transform.rotation = currentCheckpoint.rotation;
        }
        else
        {
            Debug.LogWarning("No checkpoint set! Player respawned at current position.");
        }

        yield return null;

        if (playerCapsuleCollider != null)
            playerCapsuleCollider.enabled = true;

        if (characterController != null)
            characterController.enabled = true;

        if (characterMove != null)
            characterMove.enabled = true;

        isRespawning = false;
    }
}