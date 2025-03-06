using UnityEngine;

public class Jugador : MonoBehaviour
{
    public float velocidadInicial = 5f; // Velocidad inicial del jugador
    public float incrementoVelocidad = 0.1f; // Cuánto aumenta la velocidad por segundo
    public float velocidadLateral = 4f; // Velocidad de movimiento lateral

    private float velocidadActual; // Velocidad dinámica del jugador
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ; // Evitar giros involuntarios
        velocidadActual = velocidadInicial; // Se inicia con la velocidad base
    }

    void Update()
    {
        if (!UIManager.instancia.EstaEnJuego()) return;

        // Incrementar la velocidad con el tiempo
        velocidadActual += incrementoVelocidad * Time.deltaTime;

        // Movimiento automático hacia adelante
        rb.MovePosition(rb.position + transform.forward * velocidadActual * Time.deltaTime);

        // Movimiento lateral con teclas A y D
        float movimientoLateral = Input.GetAxis("Horizontal") * velocidadLateral * Time.deltaTime;
        rb.MovePosition(rb.position + transform.right * movimientoLateral);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ParedLimite"))
        {
            Debug.Log("🚧 Colisión con pared - Movimiento bloqueado.");
        }
    }

    // 🔹 Método para obtener la velocidad actual y evitar el error
    public float GetVelocidadActual()
    {
        return velocidadActual;
    }
}
