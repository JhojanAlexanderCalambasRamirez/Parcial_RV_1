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
    public Slider barraEnergia;
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

    private float energiaMax = 100f;
    private float energiaActual;
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
        // Configurar estado inicial de los paneles
        panelInicioSesion.SetActive(true);
        panelJuego.SetActive(false);
        panelDatosJugador.SetActive(false);
        panelDatosJugadores.SetActive(false);

        // Asignar eventos a los botones
        botonIniciar.onClick.AddListener(IniciarJuego);
        botonSalirInicio.onClick.AddListener(VolverInicio);
        botonVerDatosJugadores.onClick.AddListener(MostrarPanelDatosJugadores);
        botonRegresarPanelDatosJugador.onClick.AddListener(VolverPanelDatosJugador);
        botonRegresarInicio.onClick.AddListener(VolverInicio);

        // Configurar barra de energía
        energiaActual = energiaMax;
        barraEnergia.maxValue = energiaMax;
        barraEnergia.value = energiaActual;
        puntaje = 0;
        textoPuntaje.text = $"{puntaje}";

        // Cargar puntuaciones previas
        CargarPuntuaciones();

        // Reducir energía cada segundo
        InvokeRepeating("ReducirEnergiaPorTiempo", 1f, 1f);
    }

    void Update()
    {
        if (juegoEnCurso)
        {
            // Reducir el tiempo restante
            tiempoRestante -= Time.deltaTime;
            textoCuentaRegresiva.text = Mathf.CeilToInt(tiempoRestante).ToString();

            // Si el tiempo llega a 0 o la energía se agota, termina el juego
            if (tiempoRestante <= 0 || energiaActual <= 0)
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
            Debug.Log("Ingrese un nombre válido.");
            return;
        }

        // Activar panel de juego y desactivar el de inicio
        panelInicioSesion.SetActive(false);
        panelJuego.SetActive(true);
        juegoEnCurso = true;
        tiempoRestante = 40f;
        energiaActual = energiaMax;
        puntaje = 0;
        textoPuntaje.text = $"{puntaje}";
        barraEnergia.value = energiaActual;

        GameController.instancia.IniciarJuego(inputNombre.text);
    }

    public void RecogerBarrita()
    {
        energiaActual = Mathf.Min(energiaActual + 5f, energiaMax);
        barraEnergia.value = energiaActual;
        puntaje += 1;
        textoPuntaje.text = $"{puntaje}";
        Debug.Log($"Energía actual: {energiaActual}, Puntaje: {puntaje}");
    }

    public void ColisionObstaculo()
    {
        float danio = Random.Range(10f, 20f);
        energiaActual -= danio;
        barraEnergia.value = energiaActual;
        Debug.Log($"Daño recibido: {danio}, Energía restante: {energiaActual}");
    }
    private void ReducirEnergiaPorTiempo()
    {
        if (juegoEnCurso)
        {
            energiaActual -= 1f;
            barraEnergia.value = energiaActual;
        }
    }

    public void TerminarJuego()
    {
        juegoEnCurso = false;

        string nombre = GameController.instancia.GetNombreJugador();
        float distancia = GameController.instancia.GetDistancia();
        int barritas = puntaje;

        // Mostrar los datos en Panel_DatosJugador
        textoNombreJugador.text = $"{nombre}";
        textoDistanciaJugador.text = $"{distancia:F2}m";
        textoBarritasJugador.text = $"{barritas}";

        GuardarPuntuacion(nombre, distancia, barritas);

        panelJuego.SetActive(false);
        panelDatosJugador.SetActive(true);

        Debug.Log("Juego terminado: Sin energía.");
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
        if (listaPuntuaciones.Count > 0)
            textoTop1.text = $"{listaPuntuaciones[0].nombre} {listaPuntuaciones[0].distancia:F2} {listaPuntuaciones[0].barritas}";
        else
            textoTop1.text = " ";

        if (listaPuntuaciones.Count > 1)
            textoTop2.text = $"{listaPuntuaciones[1].nombre} {listaPuntuaciones[1].distancia:F2} {listaPuntuaciones[1].barritas}";
        else
            textoTop2.text = " ";

        if (listaPuntuaciones.Count > 2)
            textoTop3.text = $"{listaPuntuaciones[2].nombre} {listaPuntuaciones[2].distancia:F2} {listaPuntuaciones[2].barritas}";
        else
            textoTop3.text = " ";
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
