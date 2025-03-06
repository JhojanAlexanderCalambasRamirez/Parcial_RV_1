using UnityEngine;

public class Jugador : MonoBehaviour
{
    public float velocidadInicial = 5f;  // Velocidad inicial en X
    public float incrementoVelocidad = 0.1f;  // Aumento de velocidad con el tiempo
    public float velocidadLateral = 4f;  // Velocidad lateral (Z)

    private float velocidadActual;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;  // Evitar efectos de gravedad
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
        velocidadActual = velocidadInicial;
    }

    void Update()
    {
        if (!UIManager.instancia.EstaEnJuego()) return;

        // 🔹 Incrementar velocidad gradualmente en X
        velocidadActual += incrementoVelocidad * Time.deltaTime;
        rb.velocity = new Vector3(velocidadActual, 0, rb.velocity.z);

        // 🔹 Movimiento lateral en Z con teclas A/D (corregido)
        float movimientoLateral = Input.GetAxis("Horizontal") * velocidadLateral;
        rb.velocity = new Vector3(rb.velocity.x, 0, -movimientoLateral);
    }

    // ✅ Mantiene las colisiones con las paredes
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ParedLimite"))
        {
            Debug.Log("🚧 Colisión con pared - Movimiento bloqueado.");
            rb.velocity = new Vector3(rb.velocity.x, 0, 0); // Detener movimiento lateral
        }
    }

    // ✅ Ahora detecta Barritas y Obstáculos con `Trigger`
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Barrita"))
        {
            Debug.Log("✅ Colisión con Barrita detectada. Aumentando energía...");
            UIManager.instancia.RecogerBarrita();
            Destroy(other.gameObject); // Destruir la barrita después de recogerla
        }
        else if (other.CompareTag("Obstaculo"))
        {
            Debug.Log("❌ Colisión con Obstáculo detectada. Aplicando penalización...");
            ControladorSlider.instancia.ReducirEnergia(Random.Range(10f, 20f));
            Destroy(other.gameObject); // Puedes aplicar un efecto antes de destruir
        }
    }

    // 🔹 Método para obtener la velocidad actual
    public float GetVelocidadActual()
    {
        return velocidadActual;
    }
}
