using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public static GameController instancia;
    public Transform jugador;  // Referencia al jugador para obtener su posición real
    private float posicionInicialX; // Guardamos la posición inicial en X
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
            posicionInicialX = jugador.position.x; // Guardamos la posición inicial en X
        }
    }

    void Update()
    {
        if (juegoIniciado && jugador != null)
        {
            // 🔹 Calcula la distancia recorrida en X
            distanciaRecorrida = jugador.position.x - posicionInicialX;
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
            posicionInicialX = jugador.position.x; // Reiniciamos la posición inicial en X
        }
    }

    public void RecogerBarrita()
    {
        barritasRecogidas++;
    }

    public void TerminarJuego()
    {
        juegoIniciado = false;
        UIManager.instancia.TerminarJuego();
    }

    public string GetNombreJugador()
    {
        return nombreJugador;
    }

    public float GetDistancia()
    {
        return distanciaRecorrida;
    }

    public int GetBarritasRecogidas()
    {
        return barritasRecogidas;
    }
}
