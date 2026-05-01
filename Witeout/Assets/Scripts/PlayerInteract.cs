using UnityEngine;
using TMPro;

public class PlayerInteract : MonoBehaviour
{
    [Header("Raycast")]
    public Camera playerCamera;
    public float interactDistance = 4f;
    public LayerMask interactLayer;

    [Header("UI")]
    public TextMeshProUGUI interactText;

    [Header("Inventory")]
    public PlayerInventory inventory;

    void Update()
    {
        interactText.text = "";

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance, interactLayer))
        {
            BatteryPickup battery = hit.collider.GetComponentInParent<BatteryPickup>();
            GeneratorPower generator = hit.collider.GetComponentInParent<GeneratorPower>();

            if (battery != null)
            {
                if (!inventory.hasBattery && !battery.isPickedUp)
                {
                    interactText.text = "Press E to pick up Battery";

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        battery.PickUp(inventory);
                    }
                }
            }
            else if (generator != null)
            {
                if (!generator.isPowered)
                {
                    if (!inventory.hasBattery)
                    {
                        interactText.text = "Generator needs power";
                    }
                    else if (generator.CanAcceptBattery(inventory))
                    {
                        interactText.text = "Press E to install Battery";

                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            generator.InstallBattery(inventory);
                        }
                    }
                    else
                    {
                        interactText.text = "Wrong battery for this generator";
                    }
                }
                else
                {
                    interactText.text = "Generator is powered";
                }
            }
        }
    }
}
