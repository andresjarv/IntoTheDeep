using UnityEngine;

public enum EstadoEsfera { Base, Lanza, Escudo }

public class ControlSphere : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform jugador;
    [SerializeField] private Camera cam;

    [Header("Configuración Base")]
    [SerializeField] private float radioOrbita = 2.5f; // Distancia máxima al personaje
    [SerializeField] private float velocidadSeguimiento = 15f;

    private Rigidbody2D rb;
    private EstadoEsfera estadoActual = EstadoEsfera.Base;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (cam == null) cam = Camera.main;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    void FixedUpdate()
    {
        Vector3 mouseScreen = Input.mousePosition;
        mouseScreen.z = Mathf.Abs(cam.transform.position.z);
        Vector2 posicionMouse = cam.ScreenToWorldPoint(mouseScreen);

        switch (estadoActual)
        {
            case EstadoEsfera.Base:
                ComportamientoBase(posicionMouse);
                break;
            case EstadoEsfera.Lanza:
                // Lógica de disparo/dash en línea recta
                break;
            case EstadoEsfera.Escudo:
                // Lógica de anclaje rígido muy cerca del jugador
                break;
        }
    }

    private void ComportamientoBase(Vector2 posicionMouse)
    {
        // 1. Obtener la dirección desde el jugador hacia el mouse
        Vector2 direccion = (posicionMouse - (Vector2)jugador.position).normalized;

        // 2. Limitar el objetivo a un radio máximo alrededor del jugador (Tethering)
        Vector2 distanciaAlMouse = posicionMouse - (Vector2)jugador.position;
        Vector2 posicionObjetivo;

        if (distanciaAlMouse.magnitude < radioOrbita)
        {
            posicionObjetivo = posicionMouse; // Si el mouse está cerca, va exacto al mouse
        }
        else
        {
            posicionObjetivo = (Vector2)jugador.position + (direccion * radioOrbita); // Topa en el borde de la órbita
        }

        // 3. Moverse hacia ese objetivo de forma rápida pero controlada, sin inercia residual
        Vector2 nuevaPos = Vector2.Lerp(rb.position, posicionObjetivo, velocidadSeguimiento * Time.fixedDeltaTime);
        rb.MovePosition(nuevaPos);
    }

    // Métodos públicos para ser llamados por tu script de inputs
    public void CambiarALanza() { estadoActual = EstadoEsfera.Lanza; }
    public void CambiarAEscudo() { estadoActual = EstadoEsfera.Escudo; }
}}