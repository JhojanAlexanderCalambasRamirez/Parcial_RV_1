using UnityEngine;
using System.Collections; // 📌 Necesario para usar Corrutinas

/// <summary>
/// Clase encargada de manejar el comportamiento del jugador.
/// Controla el movimiento en el eje X y la velocidad lateral en el eje Z.
/// También gestiona colisiones con barritas y obstáculos.
/// </summary>
public class Jugador : MonoBehaviour
{
    // ======================== 📌 VARIABLES DE MOVIMIENTO 📌 ========================

    public float velocidadInicial = 5f;  // 🔹 Velocidad inicial del jugador en el eje X.
    public float incrementoVelocidad = 0.1f;  // 🔹 Aumento gradual de la velocidad con el tiempo.
    public float velocidadLateral = 4f;  // 🔹 Velocidad de desplazamiento lateral en el eje Z.

    private float velocidadActual;  // 🔹 Variable que almacena la velocidad dinámica del jugador.
    private Rigidbody rb;  // 🔹 Componente Rigidbody para manejar la física del jugador.
    private Renderer jugadorRenderer; // 🔹 Componente Renderer para cambiar el color del jugador.

    /// <summary>
    /// Método `Start()`. Se ejecuta una sola vez al inicio del juego.
    /// Inicializa el Rigidbody y configura restricciones en su movimiento.
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody>();  // 🔹 Obtiene el componente Rigidbody del jugador.
        jugadorRenderer = GetComponent<Renderer>(); // 🔹 Obtiene el Renderer del jugador.

        // 🔹 Se desactiva la gravedad para que el jugador no caiga.
        rb.useGravity = false;

        // 🔹 Se congela la rotación y la posición en el eje Y para evitar movimientos no deseados.
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;

        // 🔹 Se asigna la velocidad inicial del jugador.
        velocidadActual = velocidadInicial;
    }

    /// <summary>
    /// Método `Update()`. Se ejecuta una vez por cada frame.
    /// Controla el movimiento automático del jugador y la navegación lateral.
    /// </summary>
    void Update()
    {
        // 🔹 Si el juego no está en curso, no se ejecuta el movimiento.
        if (!UIManager.instancia.EstaEnJuego()) return;

        // 🔹 Incrementa la velocidad con el tiempo, permitiendo que el jugador acelere de forma progresiva.
        velocidadActual += incrementoVelocidad * Time.deltaTime;

        // 🔹 Se actualiza la velocidad en el eje X, manteniendo la velocidad en Z sin cambios.
        rb.velocity = new Vector3(velocidadActual, 0, rb.velocity.z);

        // 🔹 Control de movimiento lateral con las teclas A/D o flechas izquierda/derecha.
        float movimientoLateral = Input.GetAxis("Horizontal") * velocidadLateral;

        // 🔹 Se actualiza la velocidad en Z con el movimiento lateral (negativo para la dirección correcta).
        rb.velocity = new Vector3(rb.velocity.x, 0, -movimientoLateral);
    }

    // ======================== 📌 COLISIONES 📌 ========================

    /// <summary>
    /// Método `OnCollisionEnter(Collision collision)`.
    /// Se ejecuta cuando el jugador colisiona con un objeto sólido (no `isTrigger`).
    /// Se usa principalmente para las paredes que limitan el movimiento del jugador.
    /// </summary>
    /// <param name="collision">Objeto con el que colisiona el jugador.</param>
    private void OnCollisionEnter(Collision collision)
    {
        // 🔹 Si el jugador choca contra una pared límite, se bloquea el movimiento lateral.
        if (collision.gameObject.CompareTag("ParedLimite"))
        {
            Debug.Log("🚧 Colisión con pared - Movimiento bloqueado.");
            rb.velocity = new Vector3(rb.velocity.x, 0, 0); // 🔹 Detiene el movimiento lateral.
        }
    }

    /// <summary>
    /// Método `OnTriggerEnter(Collider other)`.
    /// Se ejecuta cuando el jugador entra en contacto con un objeto que tiene `isTrigger` activado.
    /// Maneja la recogida de barritas y las colisiones con obstáculos.
    /// </summary>
    /// <param name="other">Objeto con el que el jugador colisiona.</param>
    private void OnTriggerEnter(Collider other)
    {
        // ======================== 📌 COLISIÓN CON BARRITA 📌 ========================
        if (other.CompareTag("Barrita"))
        {
            Debug.Log("✅ Colisión con Barrita detectada. Aumentando energía...");

            // 🔹 Si la barrita tiene un `AudioSource`, se reproduce su sonido antes de eliminarla.
            AudioSource sonido = other.GetComponent<AudioSource>();
            if (sonido != null)
            {
                AudioSource.PlayClipAtPoint(sonido.clip, transform.position);
            }

            // 🔹 Se destruye la barrita de inmediato.
            Destroy(other.gameObject);

            // 🔹 Se actualiza la energía y el puntaje en la UI.
            UIManager.instancia.RecogerBarrita();

            // 🟢 Inicia el efecto de energía (se pinta de verde temporalmente).
            StartCoroutine(FlashGreen());
        }

        // ======================== 📌 COLISIÓN CON OBSTÁCULO 📌 ========================
        else if (other.CompareTag("Obstaculo"))
        {
            Debug.Log("❌ Colisión con Obstáculo detectada. Aplicando penalización...");

            // 🔹 Si el obstáculo tiene un `AudioSource`, se reproduce su sonido antes de eliminarlo.
            AudioSource sonido = other.GetComponent<AudioSource>();
            if (sonido != null)
            {
                AudioSource.PlayClipAtPoint(sonido.clip, transform.position);
            }

            // 🔹 Se destruye el obstáculo de inmediato.
            Destroy(other.gameObject);

            // 🔹 Se reduce la energía del jugador con un daño aleatorio entre 10 y 20.
            ControladorSlider.instancia.ReducirEnergia(Random.Range(10f, 20f));

            // 🔴 Inicia el efecto de daño (se pinta de rojo temporalmente).
            StartCoroutine(FlashRed());
        }
    }

    // ======================== 📌 MÉTODOS AUXILIARES 📌 ========================

    /// <summary>
    /// Corrutina que cambia el color del jugador a rojo temporalmente
    /// para dar feedback visual cuando recibe daño.
    /// </summary>
    private IEnumerator FlashRed()
    {
        jugadorRenderer.material.color = Color.red; // 🔴 Cambia a rojo
        yield return new WaitForSeconds(0.5f); // ⏳ Espera medio segundo
        jugadorRenderer.material.color = Color.white; // ⚪ Restaura el color original
    }

    /// <summary>
    /// Corrutina que cambia el color del jugador a verde temporalmente
    /// para dar feedback visual cuando recoge una barrita.
    /// </summary>
    private IEnumerator FlashGreen()
    {
        jugadorRenderer.material.color = Color.green; // 🟢 Cambia a verde
        yield return new WaitForSeconds(0.5f); // ⏳ Espera medio segundo
        jugadorRenderer.material.color = Color.white; // ⚪ Restaura el color original
    }

    /// <summary>
    /// Método para obtener la velocidad actual del jugador.
    /// Se usa en otros scripts, como la cámara o el UIManager, para calcular efectos basados en la velocidad.
    /// </summary>
    /// <returns>Retorna la velocidad actual en el eje X.</returns>
    public float GetVelocidadActual()
    {
        return velocidadActual;
    }
}
