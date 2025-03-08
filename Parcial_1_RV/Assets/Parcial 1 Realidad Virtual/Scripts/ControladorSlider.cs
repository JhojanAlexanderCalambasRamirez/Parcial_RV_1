using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Clase `ControladorSlider` encargada de gestionar la energía del jugador mediante una barra deslizante (`Slider`).
/// - Controla el valor de la energía y su disminución con el tiempo.
/// - Permite aumentar y reducir la energía según eventos del juego.
/// - Es un Singleton, lo que significa que solo puede existir una instancia de este controlador.
/// </summary>
public class ControladorSlider : MonoBehaviour
{
    // ======================== 📌 SINGLETON ========================
    public static ControladorSlider instancia; // 🔹 Instancia única del controlador

    // ======================== 📌 COMPONENTES UI ========================
    [Header("Componentes UI")]
    public Slider barraEnergia; // 🔹 Referencia a la barra de energía en la interfaz

    // ======================== 📌 VARIABLES DE ENERGÍA ========================
    private float energiaMax = 100f; // 🔹 Valor máximo de la energía
    private float energiaActual; // 🔹 Energía actual del jugador

    /// <summary>
    /// Método `Awake()`.
    /// - Implementa el patrón Singleton para garantizar que solo haya una instancia de `ControladorSlider`.
    /// - Si ya existe una instancia, destruye el nuevo objeto duplicado.
    /// </summary>
    void Awake()
    {
        if (instancia == null)
            instancia = this;
        else
            Destroy(gameObject);
    }

    /// <summary>
    /// Método `Start()`.
    /// - Restablece la energía al máximo al iniciar el juego.
    /// - Activa un temporizador que reduce la energía automáticamente cada segundo.
    /// </summary>
    void Start()
    {
        RestablecerEnergia(); // 🔄 Inicializa la energía al máximo
        InvokeRepeating("ReducirEnergiaPorTiempo", 1f, 1f); // 🔄 Reduce energía cada segundo
    }

    /// <summary>
    /// Método `RestablecerEnergia()`.
    /// - Restaura la energía al valor máximo.
    /// - Actualiza la barra de energía en la UI.
    /// </summary>
    public void RestablecerEnergia()
    {
        energiaActual = energiaMax;
        barraEnergia.maxValue = energiaMax;
        barraEnergia.value = energiaActual;
        Debug.Log("🔄 Energía restablecida al máximo.");
    }

    /// <summary>
    /// Método `AumentarEnergia(float cantidad)`.
    /// - Incrementa la energía en la cantidad indicada sin superar el máximo permitido.
    /// - Actualiza la barra de energía en la UI.
    /// </summary>
    /// <param name="cantidad">Cantidad de energía a aumentar.</param>
    public void AumentarEnergia(float cantidad)
    {
        energiaActual = Mathf.Clamp(energiaActual + cantidad, 0, energiaMax);
        barraEnergia.value = energiaActual;
        Debug.Log($"🔋 Energía aumentada: {cantidad} | Total: {energiaActual}");
    }

    /// <summary>
    /// Método `ReducirEnergia(float cantidad)`.
    /// - Reduce la energía en la cantidad indicada sin permitir valores negativos.
    /// - Actualiza la barra de energía en la UI.
    /// </summary>
    /// <param name="cantidad">Cantidad de energía a reducir.</param>
    public void ReducirEnergia(float cantidad)
    {
        energiaActual = Mathf.Clamp(energiaActual - cantidad, 0, energiaMax);
        barraEnergia.value = energiaActual;
        Debug.Log($"⚠️ Energía reducida: {cantidad} | Total: {energiaActual}");
    }

    /// <summary>
    /// Método `GetEnergiaActual()`.
    /// - Devuelve la energía actual del jugador.
    /// </summary>
    /// <returns>Valor actual de la energía.</returns>
    public float GetEnergiaActual()
    {
        return energiaActual;
    }

    /// <summary>
    /// Método `ReducirEnergiaPorTiempo()`.
    /// - Reduce la energía automáticamente en 1 unidad cada segundo.
    /// - Se ejecuta de manera recurrente gracias a `InvokeRepeating()` en `Start()`.
    /// </summary>
    private void ReducirEnergiaPorTiempo()
    {
        if (energiaActual > 0)
        {
            energiaActual = Mathf.Clamp(energiaActual - 1f, 0, energiaMax);
            barraEnergia.value = energiaActual;
            Debug.Log($"🕒 Energía reducida por tiempo: {energiaActual}");
        }
    }
}
