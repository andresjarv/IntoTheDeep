using UnityEngine;

public class OxygenPickup : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float restoreAmount = 20f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Intenta obtener el componente PlayerOxygen del objeto que colisiono
        PlayerOxygen playerOxygen = collision.GetComponent<PlayerOxygen>();

        // Si el componente existe, significa que es el jugador
        if (playerOxygen != null)
        {
            playerOxygen.AddOxygen(restoreAmount);

            // Llama a tu AudioManager si existe
            if (AudioManager.Instance != null)
            {
                // AudioManager.Instance.PlaySFX(AudioManager.Instance.sfxOxigeno);
            }

            // Destruye el objeto recolectable
            Destroy(gameObject);
        }
    }
}