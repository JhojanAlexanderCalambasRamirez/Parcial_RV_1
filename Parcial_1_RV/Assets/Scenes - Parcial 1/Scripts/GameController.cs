using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class GameController : MonoBehaviour
{
    public static GameController instancia;
    public Transform camino;
    public int posicionEscenaZ = 58;

    private string nombreJugador;
    private float distanciaRecorrida;
    private int barritasRecogidas;
    private bool juegoIniciado = false;


    private void Start()
    {
        Instantiate(camino, new Vector3(0.8f, 0.98f, -3.99f), camino.rotation);
    }
    void Awake()
    {
        if (instancia == null)
            instancia = this;
    }

    void Update()
    {
        if (juegoIniciado)
        {
            distanciaRecorrida += Time.deltaTime * 10;
        }

        if (posicionEscenaZ < 120)
        {
            Instantiate(camino, new Vector3(0.8f, 0.98f, posicionEscenaZ), camino.rotation);
            posicionEscenaZ += 4;
        }
    }

    public void IniciarJuego(string nombre)
    {
        nombreJugador = nombre;
        juegoIniciado = true;
        distanciaRecorrida = 0;
        barritasRecogidas = 0;
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