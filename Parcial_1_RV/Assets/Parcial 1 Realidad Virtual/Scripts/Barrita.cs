using UnityEngine;

public class Barrita : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float velocidadRotacion = 150f;  // Velocidad de giro en el eje Z
    public float amplitudMovimiento = 0.3f; // Cuánto sube y baja
    public float velocidadMovimiento = 2f;  // Velocidad del movimiento arriba/abajo

    [Header("Configuración de Sonido")]
    public AudioSource sonidoRecoger;  // Referencia al AudioSource

    private Vector3 posicionInicial;

    void Start()
    {
        posicionInicial = transform.position; // Guarda la posición inicial
    }

    void Update()
    {
        // 🔄 Rotación en el eje Z para un mejor efecto visual
        transform.Rotate(0, 0, velocidadRotacion * Time.deltaTime);

        // 🔼 Movimiento arriba y abajo con efecto de flotación
        float nuevaY = posicionInicial.y + Mathf.Sin(Time.time * velocidadMovimiento) * amplitudMovimiento;
        transform.position = new Vector3(posicionInicial.x, nuevaY, posicionInicial.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("✅ Colisión con Barrita detectada. Aumentando energía...");

            // 🔊 Reproduce el sonido ANTES de destruir el objeto
            if (sonidoRecoger != null)
            {
                sonidoRecoger.Play();
            }

            UIManager.instancia.RecogerBarrita();

            // 🔹 Destruir el objeto de inmediato sin retraso
            Destroy(gameObject);
        }
    }
}
