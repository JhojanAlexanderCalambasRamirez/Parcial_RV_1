using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
/// <summary>
/// Clase encargada de la gestión de la interfaz de usuario (UI) en el juego.
/// Se ocupa de manejar los paneles de navegación, actualizar los datos del jugador en la interfaz,
/// gestionar la lógica del juego (como el puntaje y el tiempo), y almacenar el ranking en un archivo JSON.
/// </summary>
public class UIManager : MonoBehaviour
{
    // 🔹 Singleton para facilitar el acceso desde otros scripts
    public static UIManager instancia;

    // ======================== 📌 PANELES DE LA INTERFAZ 📌 ========================
    [Header("Paneles")]
    public GameObject panelInicioSesion;     // Panel donde el jugador introduce su nombre y correo.
    public GameObject panelTutorial;         // Panel con el tutorial del juego.
    public GameObject panelJuego;            // Panel que muestra la UI en tiempo real durante la partida.
    public GameObject panelDatosJugador;     // Panel con los datos finales del jugador.
    public GameObject panelDatosJugadores;   // Panel donde se muestra el ranking de los jugadores.

    // ======================== 📌 UI DEL INICIO DE SESIÓN 📌 ========================
    [Header("UI Inicio Sesión")]
    public TMP_InputField inputNombre;   // Campo de texto para el nombre del jugador.
    public TMP_InputField inputCorreo;   // Campo de texto para el correo del jugador.
    public Button botonTutorial;         // Botón para acceder al tutorial.

    // ======================== 📌 UI DEL TUTORIAL 📌 ========================
    [Header("UI Tutorial")]
    public Button botonIniciarJuego;     // Botón dentro del tutorial que inicia la partida.

    // ======================== 📌 UI DEL JUEGO 📌 ========================
    [Header("UI Juego")]
    public TMP_Text textoPuntaje;         // Muestra el puntaje del jugador en tiempo real.
    public TMP_Text textoCuentaRegresiva; // Muestra el tiempo restante en el juego.

    // ======================== 📌 UI DE DATOS DEL JUGADOR 📌 ========================
    [Header("UI Datos Jugador")]
    public TMP_Text textoNombreJugador;   // Muestra el nombre del jugador en el panel de resultados.
    public TMP_Text textoCorreoRanking;   // Muestra el correo del jugador en el ranking.
    public TMP_Text textoBarritasJugador; // Muestra la cantidad de barritas recogidas.
    public TMP_Text textoDistanciaJugador;// Muestra la distancia recorrida.
    public Button botonSalirInicio;       // Botón para volver al inicio.
    public Button botonVerDatosJugadores; // Botón para ver el ranking de jugadores.
    public Button botonRegresarInicio;    // Botón para regresar al inicio desde el ranking.

    // ======================== 📌 UI DEL RANKING 📌 ========================
    [Header("UI Datos Jugadores (Ranking)")]
    public TMP_Text textoTop1; // Muestra el primer puesto del ranking.
    public TMP_Text textoTop2; // Muestra el segundo puesto del ranking.
    public TMP_Text textoTop3; // Muestra el tercer puesto del ranking.
    public Button botonRegresarPanelDatosJugador; // Botón para volver a los datos del jugador.

    // ======================== 📌 VARIABLES INTERNAS 📌 ========================
    private float tiempoRestante = 40f;    // Tiempo total del juego en segundos.
    private int puntaje;                   // Puntaje del jugador.
    private bool juegoEnCurso = false;     // Indica si el juego está activo.
    private List<PuntuacionDatos> listaPuntuaciones = new List<PuntuacionDatos>(); // Lista con el ranking.

    /// <summary>
    /// Se ejecuta antes de `Start()`. Asigna la instancia de UIManager.
    /// </summary>
    void Awake()
    {
        if (instancia == null)
            instancia = this;
    }

    /// <summary>
    /// Inicializa la UI, desactiva paneles innecesarios y configura eventos de los botones.
    /// </summary>
    void Start()
    {
        Debug.Log("🎮 Inicio del juego - UIManager cargado.");

        panelInicioSesion.SetActive(true);
        panelTutorial.SetActive(false);
        panelJuego.SetActive(false);
        panelDatosJugador.SetActive(false);
        panelDatosJugadores.SetActive(false);

        botonTutorial.onClick.AddListener(MostrarTutorial);
        botonIniciarJuego.onClick.AddListener(IniciarJuego);
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

            // 🔹 Actualiza la distancia en tiempo real en la UI
            float distancia = GameController.instancia.GetDistancia();
            textoDistanciaJugador.text = $"{distancia:F2}m";
            Debug.Log($"📏 Actualizando UI - Distancia mostrada: {distancia:F2}m");

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

    // 🔹 Método para mostrar el tutorial
    public void MostrarTutorial()
    {
        panelInicioSesion.SetActive(false);
        panelTutorial.SetActive(true);
    }

    public void IniciarJuego()
    {
        if (string.IsNullOrEmpty(inputNombre.text) || string.IsNullOrEmpty(inputCorreo.text))
        {
            Debug.LogWarning("⚠️ ¡Debe ingresar un nombre y un correo antes de jugar!");
            return;
        }

        Debug.Log($"▶️ Juego iniciado por {inputNombre.text} con correo {inputCorreo.text}");

        panelTutorial.SetActive(false);
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

        ControladorSlider.instancia.AumentarEnergia(5f);
        puntaje += 1;
        textoPuntaje.text = puntaje.ToString();

        Debug.Log($"🔋 Energía total: {ControladorSlider.instancia.GetEnergiaActual()}, Puntaje: {puntaje}");
    }

    public void ColisionObstaculo()
    {
        float danio = Random.Range(5f, 10f);
        Debug.Log($"❌ Colisión con Obstáculo - Daño recibido: {danio}");
        ControladorSlider.instancia.ReducirEnergia(danio);
    }

    public void TerminarJuego()
    {
        juegoEnCurso = false;
        Debug.Log("🏁 Juego terminado - Energía agotada o tiempo finalizado.");

        string nombre = inputNombre.text;
        string correo = inputCorreo.text;
        float distancia = GameController.instancia.GetDistancia();
        int barritas = puntaje;

        textoNombreJugador.text = nombre;
        textoCorreoRanking.text = correo; // 🔹 Muestra el correo en el panel de datos del jugador
        textoDistanciaJugador.text = $"{distancia:F2}m";
        textoBarritasJugador.text = barritas.ToString();

        GuardarPuntuacion(nombre, correo, distancia, barritas);
        panelJuego.SetActive(false);
        panelDatosJugador.SetActive(true);

        Debug.Log($"🏆 Distancia final mostrada en UI: {distancia:F2}m");
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

    void GuardarPuntuacion(string nombre, string correo, float distancia, int barritas)
    {
        PuntuacionDatos nuevaPuntuacion = new PuntuacionDatos { nombre = nombre, correo = correo, distancia = distancia, barritas = barritas };
        listaPuntuaciones.Add(nuevaPuntuacion);
        listaPuntuaciones.Sort((a, b) => b.distancia.CompareTo(a.distancia));

        if (listaPuntuaciones.Count > 3)
        {
            listaPuntuaciones.RemoveAt(3);
        }

        string directorio = Application.dataPath + "/Scenes - Parcial 1/JSON";

        if (!Directory.Exists(directorio))
        {
            Directory.CreateDirectory(directorio);
        }

        string rutaArchivo = directorio + "/ranking.json";

        string json = JsonUtility.ToJson(new PuntuacionLista { puntuaciones = listaPuntuaciones }, true);
        File.WriteAllText(rutaArchivo, json);

        Debug.Log($"📁 JSON guardado en: {rutaArchivo}");
    }

    void CargarPuntuaciones()
    {
        string rutaArchivo = Application.dataPath + "/Scenes - Parcial 1/JSON/ranking.json";

        if (File.Exists(rutaArchivo))
        {
            string json = File.ReadAllText(rutaArchivo);
            PuntuacionLista datos = JsonUtility.FromJson<PuntuacionLista>(json);
            listaPuntuaciones = datos.puntuaciones;
            Debug.Log($"📂 Datos cargados desde: {rutaArchivo}");
        }
        else
        {
            Debug.LogWarning("⚠️ No se encontró el archivo de ranking, se creará uno nuevo.");
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
    public string correo;
    public float distancia;
    public int barritas;
}

[System.Serializable]
public class PuntuacionLista
{
    public List<PuntuacionDatos> puntuaciones;
}