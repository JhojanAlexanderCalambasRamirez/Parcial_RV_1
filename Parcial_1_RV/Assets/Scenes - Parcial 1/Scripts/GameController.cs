using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public static GameController instancia;
    public Transform jugador;  // Referencia al jugador para obtener su posición real
    private Vector3 posicionInicialJugador;
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
            posicionInicialJugador = jugador.position; // Guardamos la posición inicial
        }
    }

    void Update()
    {
        if (juegoIniciado && jugador != null)
        {
            // Calcula la distancia en función de la posición real en el eje Z
            distanciaRecorrida = jugador.position.z - posicionInicialJugador.z;
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
            posicionInicialJugador = jugador.position; // Reiniciamos la posición inicial al iniciar el juego
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
