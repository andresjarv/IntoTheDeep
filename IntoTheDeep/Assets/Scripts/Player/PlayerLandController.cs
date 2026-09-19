using UnityEngine;

public class PlayerLandController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 8f;

    [Header("Jump Physics")]
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float fallMultiplier = 2.5f; // Aumenta la gravedad al caer (sensacion de peso real)
    [SerializeField] private float lowJumpMultiplier = 2f; // Corta el salto si sueltas el boton antes

    [Header("Ground Detection")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private float horizontalInput;
    private bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 1. Obtener input horizontal
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // 2. Detectar si estamos tocando el suelo usando un pequeño circulo en los pies
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // 3. Detectar el inicio del salto
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // Unity 6 usa linearVelocity en lugar de velocity
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    void FixedUpdate()
    {
        // Movimiento horizontal seco y preciso (sin inercia)
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);

        // Manipulacion de la gravedad para un salto menos flotante
        if (rb.linearVelocity.y < 0)
        {
            // Si el jugador esta cayendo, aplicamos peso extra
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !Input.GetButton("Jump"))
        {
            // Si el jugador esta subiendo pero ya solto el boton, lo obligamos a caer antes
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }
}