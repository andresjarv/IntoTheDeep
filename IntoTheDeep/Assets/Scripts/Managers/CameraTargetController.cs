using UnityEngine;

public class CameraTargetController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Camera mainCamera;

    [Header("Camera Behavior Settings")]
    [Tooltip("0 = centrado en el jugador, 1 = centrado en el mouse")]
    [Range(0f, 1f)]
    [SerializeField] private float lookAheadFactor = 0.3f;

    [Tooltip("Distancia maxima que la camara puede separarse del jugador")]
    [SerializeField] private float maxLookDistance = 4f;

    void Start()
    {
        if (mainCamera == null) mainCamera = Camera.main;
    }

    void Update()
    {
        if (player == null) return;

        // 1. Obtener la posicion del mouse en el mundo espacial
        Vector3 mouseScreen = Input.mousePosition;
        mouseScreen.z = Mathf.Abs(mainCamera.transform.position.z);
        Vector2 mouseWorld = mainCamera.ScreenToWorldPoint(mouseScreen);

        // 2. Calcular el vector desde el jugador hacia el mouse
        Vector2 playerToMouse = mouseWorld - (Vector2)player.position;

        // 3. Aplicar el factor de adelantamiento (look ahead)
        Vector2 desiredLookAhead = playerToMouse * lookAheadFactor;

        // 4. Limitar la distancia para que la camara no se aleje demasiado del jugador
        Vector2 clampedLookAhead = Vector2.ClampMagnitude(desiredLookAhead, maxLookDistance);

        // 5. Posicionar el objetivo de la camara
        // Cinemachine seguira a este objeto suavemente gracias a sus propios ajustes de 'Damping'
        transform.position = player.position + (Vector3)clampedLookAhead;
    }
}