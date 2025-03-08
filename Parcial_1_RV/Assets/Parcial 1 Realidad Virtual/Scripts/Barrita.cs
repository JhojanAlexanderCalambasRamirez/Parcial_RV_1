using UnityEngine;

/// <summary>
/// Clase encargada de gestionar el comportamiento de las "Barritas" en el juego.
/// Las barritas tienen un movimiento de rotación y flotación para hacerlas más visibles.
/// Cuando el jugador colisiona con ellas, se activa un sonido (si está disponible)
/// y se aumenta la energía del jugador antes de destruir la barrita.
/// </summary>
public class Barrita : MonoBehaviour
{
    // ======================== 📌 CONFIGURACIÓN DE MOVIMIENTO 📌 ========================

    [Header("Configuración de Movimiento")]
    public float velocidadRotacion = 150f;  // 🔹 Velocidad de rotación en el eje Z
    public float amplitudMovimiento = 0.3f; // 🔹 Amplitud del movimiento vertical (flotación)
    public float velocidadMovimiento = 2f;  // 🔹 Velocidad del movimiento vertical

    // ======================== 📌 CONFIGURACIÓN DE SONIDO 📌 ========================

    [Header("Configuración de Sonido")]
    public AudioSource sonidoRecoger;  // 🔹 Referencia al AudioSource para reproducir sonido al recoger la barrita

    // ======================== 📌 VARIABLES INTERNAS 📌 ========================

    private Vector3 posicionInicial;  // 🔹 Guarda la posición inicial para el movimiento vertical

    /// <summary>
    /// Método `Start()`. Se ejecuta una vez al inicio del juego.
    /// Guarda la posición inicial de la barrita para usarla en el movimiento de flotación.
    /// </summary>
    void Start()
    {
        posicionInicial = transform.position; // 🔹 Se almacena la posición inicial en la variable `posicionInicial`
    }

    /// <summary>
    /// Método `Update()`. Se ejecuta en cada frame y gestiona:
    /// - Rotación constante en el eje Z.
    /// - Movimiento de flotación arriba/abajo con un patrón sinusoidal.
    /// </summary>
    void Update()
    {
        // 🔄 Aplicar rotación continua en el eje Z
        transform.Rotate(0, 0, velocidadRotacion * Time.deltaTime);

        // 🔼 Movimiento de flotación: Se mueve arriba y abajo basado en una onda sinusoidal.
        float nuevaY = posicionInicial.y + Mathf.Sin(Time.time * velocidadMovimiento) * amplitudMovimiento;
        transform.position = new Vector3(posicionInicial.x, nuevaY, posicionInicial.z);
    }

    // ======================== 📌 DETECCIÓN DE COLISIONES 📌 ========================

    /// <summary>
    /// Método `OnTriggerEnter(Collider other)`.
    /// Se ejecuta cuando el jugador entra en contacto con la barrita.
    /// - Aumenta la energía del jugador.
    /// - Reproduce un sonido (si está disponible).
    /// - Destruye la barrita inmediatamente.
    /// </summary>
    /// <param name="other">Objeto que entra en contacto con la barrita.</param>
    private void OnTriggerEnter(Collider other)
    {
        // 🔹 Verifica si el objeto que colisiona es el jugador
        if (other.CompareTag("Player"))
        {
            Debug.Log("✅ Colisión con Barrita detectada. Aumentando energía...");

            // 🔊 Si el AudioSource está asignado, reproducir el sonido antes de destruir la barrita
            if (sonidoRecoger != null)
            {
                sonidoRecoger.Play();
            }

            // 🔹 Notificar al `UIManager` que una barrita ha sido recogida
            UIManager.instancia.RecogerBarrita();

            // 🔥 Destruir la barrita de inmediato
            Destroy(gameObject);
        }
    }
}
