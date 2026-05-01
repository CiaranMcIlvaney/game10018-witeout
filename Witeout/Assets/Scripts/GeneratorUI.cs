using UnityEngine;
using TMPro;

public class GeneratorUI : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI generatorText;

    [Header("Generators")]
    public GeneratorPower[] generators;

    void Update()
    {
        if (generatorText == null || generators == null)
            return;

        int powered = 0;

        foreach (var gen in generators)
        {
            if (gen != null && gen.isPowered)
            {
                powered++;
            }
        }

        generatorText.text = "GENERATORS RESTORED: " + powered + " / " + generators.Length;
    }
}