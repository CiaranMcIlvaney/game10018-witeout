using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("Held Item")]
    public bool hasBattery = false;
    public BatteryPickup heldBattery;
    public Transform holdPoint;

    public void PickUpBattery(BatteryPickup battery)
    {
        hasBattery = true;
        heldBattery = battery;
    }

    public void RemoveBattery()
    {
        hasBattery = false;
        heldBattery = null;
    }
}
