using UnityEngine;
using UnityEngine.UI;

public class ControladorSlider : MonoBehaviour
{
    public static ControladorSlider instancia;

    [Header("Componentes UI")]
    public Slider barraEnergia;

    private float energiaMax = 100f;
    private float energiaActual;

    void Awake()
    {
        if (instancia == null)
            instancia = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        RestablecerEnergia();
        InvokeRepeating("ReducirEnergiaPorTiempo", 1f, 1f);
    }

    public void RestablecerEnergia()
    {
        energiaActual = energiaMax;
        barraEnergia.maxValue = energiaMax;
        barraEnergia.value = energiaActual;
        Debug.Log("🔄 Energía restablecida al máximo.");
    }

    public void AumentarEnergia(float cantidad)
    {
        energiaActual = Mathf.Clamp(energiaActual + cantidad, 0, energiaMax);
        barraEnergia.value = energiaActual;
        Debug.Log($"🔋 Energía aumentada: {cantidad} | Total: {energiaActual}");
    }

    public void ReducirEnergia(float cantidad)
    {
        energiaActual = Mathf.Clamp(energiaActual - cantidad, 0, energiaMax);
        barraEnergia.value = energiaActual;
        Debug.Log($"⚠️ Energía reducida: {cantidad} | Total: {energiaActual}");
    }

    public float GetEnergiaActual()
    {
        return energiaActual;
    }

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
