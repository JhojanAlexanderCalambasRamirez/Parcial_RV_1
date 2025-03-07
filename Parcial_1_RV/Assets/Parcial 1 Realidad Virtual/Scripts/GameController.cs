using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instancia;
    public Transform jugador;
    private float posicionInicialX;
    private float distanciaRecorrida;
    private bool juegoIniciado = false;

    private string nombreJugador;
    private int barritasRecogidas;

    void Awake()
    {
        if (instancia == null)
            instancia = this;
    }

    private void Start()
    {
        if (jugador != null)
        {
            posicionInicialX = jugador.position.x;
            Debug.Log($"📍 Posición inicial del jugador en X: {posicionInicialX}");
        }
        else
        {
            Debug.LogError("🚨 No se ha asignado el jugador en GameController.");
        }
    }

    void Update()
    {
        if (juegoIniciado && jugador != null)
        {
            float nuevaPosicionX = jugador.position.x;
            distanciaRecorrida = nuevaPosicionX - posicionInicialX;
            distanciaRecorrida = Mathf.Max(distanciaRecorrida, 0); // Evita valores negativos

            Debug.Log($"📏 Distancia recorrida: {distanciaRecorrida:F2}m (Posición X actual: {nuevaPosicionX})");
        }
    }

    public void IniciarJuego(string nombre)
    {
        nombreJugador = nombre;
        juegoIniciado = true;
        distanciaRecorrida = 0;
        barritasRecogidas = 0;

        if (jugador != null)
        {
            posicionInicialX = jugador.position.x;
            Debug.Log($"🔄 Nueva posición inicial en X: {posicionInicialX}");
        }
    }

    public float GetDistancia()
    {
        if (jugador != null)
        {
            distanciaRecorrida = jugador.position.x - posicionInicialX;
            distanciaRecorrida = Mathf.Max(distanciaRecorrida, 0);
            Debug.Log($"📢 Distancia final antes de mostrar en UI: {distanciaRecorrida:F2}m");
        }
        return distanciaRecorrida;
    }
}
