using UnityEngine;
using UnityEngine.Rendering.Universal;

public class OxygenLight : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerOxygen playerOxygen;
    [SerializeField] private Light2D auraLight;

    [Header("Settings")]
    [SerializeField] private float maxRadius = 1.5f;
    [SerializeField] private float minRadius = 0.2f;

    void OnEnable()
    {
        // Se suscribe al evento cuando el objeto se activa
        if (playerOxygen != null)
            playerOxygen.OnOxygenChanged += UpdateLight;
    }

    void OnDisable()
    {
        // Se desuscribe para evitar errores de memoria
        if (playerOxygen != null)
            playerOxygen.OnOxygenChanged -= UpdateLight;
    }

    private void UpdateLight(float current, float max)
    {
        if (auraLight == null) return;

        float ratio = current / max;

        // Ajusta el radio y la intensidad de la luz basados en el oxigeno restante
        auraLight.pointLightOuterRadius = Mathf.Lerp(minRadius, maxRadius, ratio);
        auraLight.intensity = Mathf.Lerp(0.5f, 1.0f, ratio);
    }
}