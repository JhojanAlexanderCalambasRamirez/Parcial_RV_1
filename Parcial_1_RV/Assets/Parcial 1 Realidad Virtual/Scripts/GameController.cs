using UnityEngine;

/**
 * GameController es una clase que maneja la lógica principal del juego,
 * incluyendo la gestión de la distancia recorrida por el jugador y la
 * inicialización del juego.
 */
public class GameController : MonoBehaviour
{
    // Instancia única de GameController para permitir el acceso global a sus métodos y variables.
    public static GameController instancia;

    // Referencia al transform del jugador para obtener su posición en tiempo real.
    public Transform jugador;

    // Variables para calcular la distancia recorrida por el jugador.
    private float posicionInicialX;  // Guarda la posición inicial en el eje X al inicio del juego.
    private float distanciaRecorrida;  // Acumula la distancia recorrida por el jugador.

    // Estado del juego
    private bool juegoIniciado = false; // Indica si el juego está en curso o no.

    // Datos del jugador
    private string nombreJugador;  // Guarda el nombre del jugador ingresado en la UI.
    private int barritasRecogidas; // Contador de barritas recogidas en la partida.

    /**
     * Método Awake():
     * Se ejecuta antes de Start(). Garantiza que solo haya una instancia de GameController.
     */
    void Awake()
    {
        if (instancia == null)
            instancia = this; // Se asigna la instancia única de GameController.
    }

    /**
     * Método Start():
     * Se ejecuta al iniciar el juego y guarda la posición inicial del jugador en X.
     */
    private void Start()
    {
        if (jugador != null)
        {
            posicionInicialX = jugador.position.x; // Guarda la posición inicial del jugador en X.
            Debug.Log($"📍 Posición inicial del jugador en X: {posicionInicialX}");
        }
        else
        {
            Debug.LogError("🚨 No se ha asignado el jugador en GameController.");
        }
    }

    /**
     * Método Update():
     * Se ejecuta en cada frame y actualiza la distancia recorrida si el juego está en curso.
     */
    void Update()
    {
        if (juegoIniciado && jugador != null)
        {
            float nuevaPosicionX = jugador.position.x; // Obtiene la posición actual del jugador en X.
            distanciaRecorrida = nuevaPosicionX - posicionInicialX; // Calcula la distancia recorrida.

            // Evita valores negativos si el jugador se mueve hacia atrás.
            distanciaRecorrida = Mathf.Max(distanciaRecorrida, 0);

            Debug.Log($"📏 Distancia recorrida: {distanciaRecorrida:F2}m (Posición X actual: {nuevaPosicionX})");
        }
    }

    /**
     * Método IniciarJuego(string nombre):
     * Se llama cuando el jugador inicia el juego, reseteando la distancia recorrida y registrando el nombre del jugador.
     * @param nombre Nombre del jugador.
     */
    public void IniciarJuego(string nombre)
    {
        nombreJugador = nombre; // Almacena el nombre del jugador.
        juegoIniciado = true; // Marca el juego como iniciado.
        distanciaRecorrida = 0; // Resetea la distancia recorrida.
        barritasRecogidas = 0; // Resetea la cantidad de barritas recogidas.

        if (jugador != null)
        {
            posicionInicialX = jugador.position.x; // Guarda la nueva posición inicial del jugador.
            Debug.Log($"🔄 Nueva posición inicial en X: {posicionInicialX}");
        }
    }

    /**
     * Método GetDistancia():
     * Retorna la distancia recorrida por el jugador desde el inicio del juego.
     * @return distanciaRecorrida La distancia recorrida en metros.
     */
    public float GetDistancia()
    {
        if (jugador != null)
        {
            distanciaRecorrida = jugador.position.x - posicionInicialX; // Recalcula la distancia.
            distanciaRecorrida = Mathf.Max(distanciaRecorrida, 0); // Asegura que no sea negativa.
            Debug.Log($"📢 Distancia final antes de mostrar en UI: {distanciaRecorrida:F2}m");
        }
        return distanciaRecorrida; // Retorna la distancia recorrida.
    }
}
