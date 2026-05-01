using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Snowmobile;

public class SnowmobileUI : MonoBehaviour
{
    [Header("References")]
    public SnowmobilPhysics snowmobilePhysics;
    public Image fuelFill;
    public TextMeshProUGUI fuelText;
    public TextMeshProUGUI boostText;

    [Header("Fuel Colors")]
    public Color normalFuelColor = new Color32(140, 190, 255, 255);
    public Color lowFuelColor = new Color32(255, 220, 120, 255);
    public Color criticalFuelColor = new Color32(255, 90, 90, 255);

    void Update()
    {
        if (snowmobilePhysics == null)
            return;

        float fuelPercent = 0f;

        if (snowmobilePhysics.MaxFuel > 0f)
        {
            fuelPercent = snowmobilePhysics.CurrentFuel / snowmobilePhysics.MaxFuel;
        }

        if (fuelFill != null)
        {
            fuelFill.fillAmount = fuelPercent;

            if (fuelPercent <= 0.10f)
            {
                fuelFill.color = criticalFuelColor;
            }
            else if (fuelPercent <= 0.30f)
            {
                fuelFill.color = lowFuelColor;
            }
            else
            {
                fuelFill.color = normalFuelColor;
            }
        }

        if (fuelText != null)
        {
            float displayFuel = fuelPercent * 100f;

            fuelText.text = "FUEL: " + Mathf.CeilToInt(displayFuel) + " / 100";
        }

        if (boostText != null)
        {
            boostText.text = "SHIFT = BOOST";
        }
    }
}