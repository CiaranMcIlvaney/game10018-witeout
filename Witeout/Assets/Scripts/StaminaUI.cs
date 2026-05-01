using UnityEngine;
using UnityEngine.UI;

public class StaminaUI : MonoBehaviour
{
    [Header("References")]
    public CharacterMove playerMovement;
    public Image staminaFill;

    private void Update()
    {
        if (playerMovement == null || staminaFill == null)
        {
            return;
        }

        staminaFill.fillAmount = playerMovement.StaminaPercent;
    }
}
