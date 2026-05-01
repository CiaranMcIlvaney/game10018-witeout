using UnityEngine;

public class BatteryPickup : MonoBehaviour
{
    [Header("Battery Settings")]
    public bool isPickedUp = false;
    public GameObject batterySpotlight;
    public Rigidbody rb;
    public Collider col;

    [Header("Battery ID")]
    public string outpostID;

    public void PickUp(PlayerInventory inventory)
    {
        if (isPickedUp) return;
        if (inventory == null) return;
        if (inventory.holdPoint == null)
        {
            Debug.LogWarning("HoldPoint is missing on PlayerInventory.");
            return;
        }

        isPickedUp = true;
        inventory.PickUpBattery(this);

        if (batterySpotlight != null)
        {
            batterySpotlight.SetActive(false);
        }

        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        if (col != null)
        {
            col.enabled = false;
        }

        transform.SetParent(inventory.holdPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;

        Debug.Log("Battery moved to HoldPoint");
    }
}