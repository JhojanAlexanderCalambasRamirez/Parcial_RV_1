using UnityEngine;

/// <summary>
/// Clase encargada de gestionar el comportamiento de los obstáculos en el juego.
/// Los obstáculos reducen la energía del jugador cuando colisiona con ellos.
/// Además, pueden reproducir un sonido al colisionar si tienen un AudioSource asignado.
/// </summary>
public class Obstaculo : MonoBehaviour
{
    // ======================== 📌 CONFIGURACIÓN DE SONIDO 📌 ========================

    [Header("Configuración de Sonido")]
    public AudioSource sonidoColision;  // 🔹 Referencia al componente AudioSource para reproducir sonido en colisión.

    /// <summary>
    /// Método `Start()`. Se ejecuta una vez al inicio del juego.
    /// Verifica si el obstáculo tiene un AudioSource asignado. Si no, lo busca en el GameObject.
    /// </summary>
    void Start()
    {
        // 🔹 Si el AudioSource no fue asignado en el Inspector, intenta encontrarlo en el mismo objeto.
        if (sonidoColision == null)
        {
            sonidoColision = GetComponent<AudioSource>();

            // 🔹 Si no se encuentra un AudioSource, se muestra una advertencia en la consola.
            if (sonidoColision == null)
            {
                Debug.LogWarning($"⚠️ Obstáculo {gameObject.name} no tiene AudioSource asignado.");
            }
        }
    }

    // ======================== 📌 DETECCIÓN DE COLISIONES 📌 ========================

    /// <summary>
    /// Método `OnTriggerEnter(Collider other)`.
    /// Se ejecuta cuando el jugador entra en contacto con el obstáculo.
    /// Aplica daño al jugador, reproduce un sonido (si hay uno disponible) y destruye el obstáculo después de 0.5 segundos.
    /// </summary>
    /// <param name="other">Objeto que entra en contacto con el obstáculo.</param>
    private void OnTriggerEnter(Collider other)
    {
        // 🔹 Si el objeto que colisiona tiene la etiqueta "Player", significa que el jugador ha chocado con el obstáculo.
        if (other.CompareTag("Player"))
        {
            // 🔹 Se genera un valor de daño aleatorio entre 10 y 20 puntos.
            float danio = Random.Range(10f, 20f);
            Debug.Log($"❌ Colisión con Obstáculo detectada. Daño recibido: {danio}");

            // 🔹 Se verifica que el AudioSource esté presente y habilitado antes de reproducir el sonido.
            if (sonidoColision != null && sonidoColision.enabled)
            {
                sonidoColision.Play();
            }
            else
            {
                Debug.LogWarning($"⚠️ No se pudo reproducir el sonido en {gameObject.name}");
            }

            // 🔹 Se reduce la energía del jugador usando el ControladorSlider.
            ControladorSlider.instancia.ReducirEnergia(danio);

            // 🔹 Se destruye el obstáculo después de 0.5 segundos para permitir que el sonido se reproduzca completamente.
            Destroy(gameObject, 0.5f);
        }
    }
}
