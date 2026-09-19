using UnityEngine;

public class EnvironmentSwitcher : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Rigidbody2D rb;

    // Arrastra aqui tu script de nado y el nuevo de tierra
    [SerializeField] private MonoBehaviour swimController;
    [SerializeField] private MonoBehaviour landController;

    [Header("Water Physics Profile")]
    [SerializeField] private float waterGravity = 0f;      // En el agua no hay gravedad
    [SerializeField] private float waterLinearDamping = 3f; // Friccion alta para frenar el nado

    [Header("Land Physics Profile")]
    [SerializeField] private float landGravity = 3f;       // Gravedad fuerte en tierra
    [SerializeField] private float landLinearDamping = 0f;  // Friccion baja para movimiento puro

    private void Start()
    {
        // Asumimos que empezamos en el agua por defecto. Ajustalo segun tu nivel.
        SetWaterMode();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si entramos al agua, activamos modo nado
        if (collision.CompareTag("Water"))
        {
            SetWaterMode();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Si salimos del volumen de agua, activamos modo tierra
        if (collision.CompareTag("Water"))
        {
            SetLandMode();
        }
    }

    private void SetWaterMode()
    {
        landController.enabled = false;
        swimController.enabled = true;

        rb.gravityScale = waterGravity;

        // linearDamping es la propiedad correcta en Unity 6 para la resistencia/roce
        rb.linearDamping = waterLinearDamping;
    }

    private void SetLandMode()
    {
        swimController.enabled = false;
        landController.enabled = true;

        rb.gravityScale = landGravity;
        rb.linearDamping = landLinearDamping;
    }
}