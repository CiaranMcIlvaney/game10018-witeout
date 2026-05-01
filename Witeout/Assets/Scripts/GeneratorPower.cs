using UnityEngine;

public class GeneratorPower : MonoBehaviour
{
    [Header("Generator State")]
    public bool isPowered = false;

    [Header("Generator ID")]
    public string outpostID;

    [Header("Power Results")]
    public Light[] lightsToTurnOn;
    public GameObject[] objectsToEnable;

    public bool CanAcceptBattery(PlayerInventory inventory)
    {
        if (inventory == null) return false;
        if (!inventory.hasBattery) return false;
        if (inventory.heldBattery == null) return false;

        return inventory.heldBattery.outpostID == outpostID;
    }

    public void InstallBattery(PlayerInventory inventory)
    {
        if (isPowered) return;
        if (!inventory.hasBattery) return;
        if (inventory.heldBattery == null) return;
        if (inventory.heldBattery.outpostID != outpostID) return;

        isPowered = true;

        if (inventory.heldBattery != null)
        {
            Destroy(inventory.heldBattery.gameObject);
        }

        inventory.RemoveBattery();

        foreach (Light lightObj in lightsToTurnOn)
        {
            if (lightObj != null)
            {
                lightObj.enabled = true;
            }
        }

        foreach (GameObject obj in objectsToEnable)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }

        if (GameProgressManager.Instance != null)
        {
            GameProgressManager.Instance.RegisterGeneratorRepair();
        }

        Debug.Log("Generator powered on");
    }
}