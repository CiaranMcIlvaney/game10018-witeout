using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("Checkpoint")]
    public Transform respawnPoint;

    [Header("Linked Generator")]
    public GeneratorPower linkedGenerator;

    [Header("UI")]
    public CheckpointUI checkpointUI;

    private void TrySetCheckpoint(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        // Only allow checkpoint if generator is powered
        if (linkedGenerator != null && !linkedGenerator.isPowered)
            return;

        PlayerRespawn playerRespawn = other.GetComponent<PlayerRespawn>();

        if (playerRespawn != null && respawnPoint != null)
        {
            if (playerRespawn.currentCheckpoint != respawnPoint)
            {
                playerRespawn.SetCheckpoint(respawnPoint);

                if (checkpointUI != null)
                {
                    checkpointUI.ShowCheckpointMessage();
                }

                Debug.Log("Checkpoint set: " + respawnPoint.name);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        TrySetCheckpoint(other);
    }

    private void OnTriggerStay(Collider other)
    {
        TrySetCheckpoint(other);
    }
}
