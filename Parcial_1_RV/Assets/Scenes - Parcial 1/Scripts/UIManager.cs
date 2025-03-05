using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public class UIManager : MonoBehaviour
{
    public static UIManager instancia;

    [Header("Paneles")]
    public GameObject panelInicioSesion;
    public GameObject panelJuego;
    public GameObject panelDatosJugador;
    public GameObject panelDatosJugadores;

    [Header("UI Inicio Sesión")]
    public TMP_InputField inputNombre;
    public Button botonIniciar;

    [Header("UI Juego")]
    public TMP_Text textoPuntaje;
    public TMP_Text textoCuentaRegresiva;

    [Header("UI Datos Jugador")]
    public TMP_Text textoNombreJugador;
    public TMP_Text textoBarritasJugador;
    public TMP_Text textoDistanciaJugador;
    public Button botonSalirInicio;
    public Button botonVerDatosJugadores;
    public Button botonRegresarInicio;

    [Header("UI Datos Jugadores (Ranking)")]
    public TMP_Text textoTop1;
    public TMP_Text textoTop2;
    public TMP_Text textoTop3;
    public Button botonRegresarPanelDatosJugador;

    private float tiempoRestante = 40f;
    private int puntaje;
    private bool juegoEnCurso = false;
    private List<PuntuacionDatos> listaPuntuaciones = new List<PuntuacionDatos>();

    void Awake()
    {
        if (instancia == null)
            instancia = this;
    }

    void Start()
    {
        Debug.Log("🎮 Inicio del juego - UIManager cargado.");

        panelInicioSesion.SetActive(true);
        panelJuego.SetActive(false);
        panelDatosJugador.SetActive(false);
        panelDatosJugadores.SetActive(false);

        botonIniciar.onClick.AddListener(IniciarJuego);
        botonSalirInicio.onClick.AddListener(VolverInicio);
        botonVerDatosJugadores.onClick.AddListener(MostrarPanelDatosJugadores);
        botonRegresarPanelDatosJugador.onClick.AddListener(VolverPanelDatosJugador);
        botonRegresarInicio.onClick.AddListener(VolverInicio);

        puntaje = 0;
        textoPuntaje.text = puntaje.ToString();

        CargarPuntuaciones();
    }

    void Update()
    {
        if (juegoEnCurso)
        {
            tiempoRestante -= Time.deltaTime;
            textoCuentaRegresiva.text = Mathf.CeilToInt(tiempoRestante).ToString();

            if (tiempoRestante <= 0 || ControladorSlider.instancia.GetEnergiaActual() <= 0)
            {
                TerminarJuego();
            }
        }
    }

    public bool EstaEnJuego()
    {
        return juegoEnCurso;
    }

    public void IniciarJuego()
    {
        if (string.IsNullOrEmpty(inputNombre.text))
        {
            Debug.LogWarning("⚠️ ¡Debe ingresar un nombre antes de jugar!");
            return;
        }

        Debug.Log($"▶️ Juego iniciado por {inputNombre.text}.");

        panelInicioSesion.SetActive(false);
        panelJuego.SetActive(true);
        juegoEnCurso = true;
        tiempoRestante = 40f;
        puntaje = 0;
        textoPuntaje.text = puntaje.ToString();

        ControladorSlider.instancia.RestablecerEnergia();
    }

    public void RecogerBarrita()
    {
        Debug.Log("✅ Barrita recogida - Aumentando energía y puntaje.");

        ControladorSlider.instancia.AumentarEnergia(5f);  // Aumentar energía
        puntaje += 1;  // Sumar puntaje
        textoPuntaje.text = puntaje.ToString();  // Actualizar TMP del puntaje

        Debug.Log($"🔋 Energía total: {ControladorSlider.instancia.GetEnergiaActual()}, Puntaje: {puntaje}");
    }


    public void ColisionObstaculo()
    {
        float danio = Random.Range(10f, 20f);
        Debug.Log($"❌ Colisión con Obstáculo - Daño recibido: {danio}");
        ControladorSlider.instancia.ReducirEnergia(danio);
    }

    public void TerminarJuego()
    {
        juegoEnCurso = false;
        Debug.Log("🏁 Juego terminado - Energía agotada o tiempo finalizado.");

        string nombre = inputNombre.text;
        float distancia = 0;
        int barritas = puntaje;

        textoNombreJugador.text = nombre;
        textoDistanciaJugador.text = $"{distancia:F2}m";
        textoBarritasJugador.text = barritas.ToString();

        GuardarPuntuacion(nombre, distancia, barritas);
        panelJuego.SetActive(false);
        panelDatosJugador.SetActive(true);
    }

    public void VolverInicio()
    {
        panelDatosJugador.SetActive(false);
        panelDatosJugadores.SetActive(false);
        panelInicioSesion.SetActive(true);
    }

    public void MostrarPanelDatosJugadores()
    {
        panelDatosJugador.SetActive(false);
        panelDatosJugadores.SetActive(true);
        MostrarRanking();
    }

    public void VolverPanelDatosJugador()
    {
        panelDatosJugadores.SetActive(false);
        panelDatosJugador.SetActive(true);
    }

    void GuardarPuntuacion(string nombre, float distancia, int barritas)
    {
        PuntuacionDatos nuevaPuntuacion = new PuntuacionDatos { nombre = nombre, distancia = distancia, barritas = barritas };
        listaPuntuaciones.Add(nuevaPuntuacion);
        listaPuntuaciones.Sort((a, b) => b.distancia.CompareTo(a.distancia));

        if (listaPuntuaciones.Count > 3)
        {
            listaPuntuaciones.RemoveAt(3);
        }

        string json = JsonUtility.ToJson(new PuntuacionLista { puntuaciones = listaPuntuaciones }, true);
        File.WriteAllText(Application.persistentDataPath + "/ranking.json", json);
    }

    void CargarPuntuaciones()
    {
        string path = Application.persistentDataPath + "/ranking.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            PuntuacionLista datos = JsonUtility.FromJson<PuntuacionLista>(json);
            listaPuntuaciones = datos.puntuaciones;
        }
    }

    void MostrarRanking()
    {
        Debug.Log("📊 Mostrando ranking...");

        if (listaPuntuaciones.Count > 0)
            textoTop1.text = $"{listaPuntuaciones[0].nombre} {listaPuntuaciones[0].distancia:F2}m {listaPuntuaciones[0].barritas}";
        else
            textoTop1.text = "---";

        if (listaPuntuaciones.Count > 1)
            textoTop2.text = $"{listaPuntuaciones[1].nombre} {listaPuntuaciones[1].distancia:F2}m {listaPuntuaciones[1].barritas}";
        else
            textoTop2.text = "---";

        if (listaPuntuaciones.Count > 2)
            textoTop3.text = $"{listaPuntuaciones[2].nombre} {listaPuntuaciones[2].distancia:F2}m {listaPuntuaciones[2].barritas}";
        else
            textoTop3.text = "---";
    }
}

[System.Serializable]
public class PuntuacionDatos
{
    public string nombre;
    public float distancia;
    public int barritas;
}

[System.Serializable]
public class PuntuacionLista
{
    public List<PuntuacionDatos> puntuaciones;
}

