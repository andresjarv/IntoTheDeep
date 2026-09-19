using UnityEngine;
using System;

public class PlayerOxygen : MonoBehaviour
{
    [Header("Oxygen Parameters")]
    [SerializeField] private float maxOxygen = 30f;
    [SerializeField] private float depletionRate = 1f;

    // Eventos a los que otros scripts se pueden suscribir
    public event Action<float, float> OnOxygenChanged; // Envía (current, max)
    public event Action OnOxygenDepleted;

    private float currentOxygen;
    private DeadPlayer deathScript;

    void Awake()
    {
        currentOxygen = maxOxygen;
        deathScript = GetComponent<DeadPlayer>();
    }

    void Update()
    {
        DepleteOxygen();
    }

    private void DepleteOxygen()
    {
        if (currentOxygen <= 0) return;

        currentOxygen -= depletionRate * Time.deltaTime;
        currentOxygen = Mathf.Max(currentOxygen, 0);

        // Notificar a cualquiera que esté escuchando (UI, Luces, etc.)
        OnOxygenChanged?.Invoke(currentOxygen, maxOxygen);

        if (currentOxygen <= 0)
        {
            Die();
        }
    }

    public void AddOxygen(float amount)
    {
        currentOxygen = Mathf.Min(currentOxygen + amount, maxOxygen);
        OnOxygenChanged?.Invoke(currentOxygen, maxOxygen);
    }

    private void Die()
    {
        OnOxygenDepleted?.Invoke();
        if (deathScript != null) deathScript.Die();
        this.enabled = false;
    }
}