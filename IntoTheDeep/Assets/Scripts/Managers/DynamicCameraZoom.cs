using UnityEngine;
using Unity.Cinemachine; // Nuevo namespace obligatorio en Unity 6

public class DynamicCameraZoom : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CinemachineCamera cinemachineCam;
    [SerializeField] private SphereController sphereController;

    [Header("Zoom Settings")]
    [SerializeField] private float defaultOrthoSize = 5f;
    [SerializeField] private float spearOrthoSize = 6.5f; // Aleja la camara al apuntar/atacar
    [SerializeField] private float shieldOrthoSize = 4f;  // Acerca la camara al defender
    [SerializeField] private float zoomSpeed = 3f;

    void Start()
    {
        if (cinemachineCam == null)
            cinemachineCam = GetComponent<CinemachineCamera>();
    }

    void Update()
    {
        if (cinemachineCam == null || sphereController == null) return;

        float targetSize = defaultOrthoSize;
        SphereState currentState = sphereController.GetCurrentState();

        // Determinar el nivel de zoom basado en el modo del Twin-Stick
        if (currentState == SphereState.Attacking)
        {
            targetSize = spearOrthoSize;
        }
        else if (currentState == SphereState.Shield)
        {
            targetSize = shieldOrthoSize;
        }

        // Interpolar suavemente el tamaño de la lente de Cinemachine 3
        cinemachineCam.Lens.OrthographicSize = Mathf.Lerp(
            cinemachineCam.Lens.OrthographicSize,
            targetSize,
            zoomSpeed * Time.deltaTime
        );
    }
}