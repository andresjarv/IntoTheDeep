using UnityEngine;

public enum SphereState { Light, Shield, Attacking }

public class SphereController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Camera mainCamera;

    // Preparación para tu integración futura de arte
    [SerializeField] private SpriteRenderer sphereVisuals;

    [Header("Light Mode (Default)")]
    [SerializeField] private float lightOrbitRadius = 2.5f;
    [SerializeField] private float lightFollowSpeed = 15f;

    [Header("Shield Mode (Right Click)")]
    [SerializeField] private float shieldDistance = 1.2f;
    [SerializeField] private float shieldFollowSpeed = 30f;

    [Header("Spear Attack (Left Click)")]
    [SerializeField] private float stabDistance = 3f;
    [SerializeField] private float stabSpeed = 50f;

    private Rigidbody2D rb;
    private SphereState currentState = SphereState.Light;
    private Vector2 mousePosition;

    // Control del ataque
    private bool isReturning = false;
    private Vector2 stabDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (mainCamera == null) mainCamera = Camera.main;

        // Oculta el cursor y lo confina a la ventana
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    void Update()
    {
        // 1. Obtener posicion del mouse
        Vector3 mouseScreen = Input.mousePosition;
        mouseScreen.z = Mathf.Abs(mainCamera.transform.position.z);
        mousePosition = mainCamera.ScreenToWorldPoint(mouseScreen);

        // 2. Transiciones orgánicas (Flujo Twin-Stick)
        // No evaluamos inputs si esta en medio de un ataque
        if (currentState != SphereState.Attacking)
        {
            if (Input.GetMouseButtonDown(0)) // Clic Izquierdo -> Atacar
            {
                StartStab();
            }
            else if (Input.GetMouseButton(1)) // Clic Derecho Mantenido -> Escudo
            {
                currentState = SphereState.Shield;
            }
            else // Sin botones -> Modo Luz
            {
                currentState = SphereState.Light;
            }
        }

        // 3. Gestionar transiciones visuales
        UpdateVisuals();
    }

    void FixedUpdate()
    {
        switch (currentState)
        {
            case SphereState.Light:
                ExecuteLightBehavior();
                break;
            case SphereState.Shield:
                ExecuteShieldBehavior();
                break;
            case SphereState.Attacking:
                ExecuteStabMovement();
                break;
        }
    }

    private void ExecuteLightBehavior()
    {
        Vector2 direction = (mousePosition - (Vector2)player.position).normalized;
        Vector2 distanceToMouse = mousePosition - (Vector2)player.position;

        Vector2 targetPosition = (distanceToMouse.magnitude < lightOrbitRadius)
            ? mousePosition
            : (Vector2)player.position + (direction * lightOrbitRadius);

        rb.MovePosition(Vector2.Lerp(rb.position, targetPosition, lightFollowSpeed * Time.fixedDeltaTime));
    }

    private void ExecuteShieldBehavior()
    {
        Vector2 direction = (mousePosition - (Vector2)player.position).normalized;
        Vector2 targetPosition = (Vector2)player.position + (direction * shieldDistance);

        rb.MovePosition(Vector2.Lerp(rb.position, targetPosition, shieldFollowSpeed * Time.fixedDeltaTime));
    }

    private void StartStab()
    {
        currentState = SphereState.Attacking;
        isReturning = false;
        stabDirection = (mousePosition - (Vector2)player.position).normalized;
    }

    private void ExecuteStabMovement()
    {
        Vector2 targetPosition = !isReturning
            ? (Vector2)player.position + (stabDirection * stabDistance)
            : (Vector2)player.position + (stabDirection * lightOrbitRadius);

        Vector2 newPos = Vector2.MoveTowards(rb.position, targetPosition, stabSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        if (Vector2.Distance(rb.position, targetPosition) < 0.1f)
        {
            if (!isReturning)
            {
                isReturning = true; // Llego al final de la punzada, regresa
            }
            else
            {
                currentState = SphereState.Light; // Volvio al jugador, termina el ataque
            }
        }
    }

    private void UpdateVisuals()
    {
        // Esta función aisla la lógica visual de la mecánica. 
        // Aquí cambiarás tus sprites en el futuro sin romper el movimiento.
        if (sphereVisuals == null) return;

        switch (currentState)
        {
            case SphereState.Light:
                // sphereVisuals.sprite = lightSprite;
                sphereVisuals.color = Color.yellow; // Placeholder
                break;
            case SphereState.Shield:
                // sphereVisuals.sprite = shieldSprite;
                sphereVisuals.color = Color.blue; // Placeholder
                break;
            case SphereState.Attacking:
                // sphereVisuals.sprite = spearSprite;

                // Rotar el sprite para que la punta mire hacia donde ataca
                float angle = Mathf.Atan2(stabDirection.y, stabDirection.x) * Mathf.Rad2Deg;
                sphereVisuals.transform.rotation = Quaternion.Euler(0, 0, angle);
                sphereVisuals.color = Color.red; // Placeholder
                break;
        }

        // Si no está atacando, reinicia la rotación
        if (currentState != SphereState.Attacking)
        {
            sphereVisuals.transform.rotation = Quaternion.identity;
        }

    }
    // Añadir al final de SphereController.cs
    public SphereState GetCurrentState()
    {
        return currentState;
    }
}