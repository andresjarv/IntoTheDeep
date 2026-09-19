using UnityEngine;
using UnityEngine.UI;

public class OxygenUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerOxygen playerOxygen;
    [SerializeField] private Image oxygenCircle;

    [Header("Settings")]
    [SerializeField] private Color fullColor = Color.cyan;
    [SerializeField] private Color lowColor = Color.red;
    [SerializeField] private float dangerThreshold = 0.3f;

    void OnEnable()
    {
        // Nos suscribimos al evento
        if (playerOxygen != null)
            playerOxygen.OnOxygenChanged += UpdateVisuals;
    }

    void OnDisable()
    {
        // Nos desuscribimos para evitar errores de memoria
        if (playerOxygen != null)
            playerOxygen.OnOxygenChanged -= UpdateVisuals;
    }

    private void UpdateVisuals(float current, float max)
    {
        float fillRatio = current / max;
        oxygenCircle.fillAmount = fillRatio;
        oxygenCircle.color = (fillRatio < dangerThreshold) ? lowColor : fullColor;

        // Ocultar si está lleno
        oxygenCircle.gameObject.SetActive(current < max);
    }
}
