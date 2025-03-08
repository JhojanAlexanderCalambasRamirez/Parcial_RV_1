using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Clase `BotonSonido` encargada de reproducir un sonido cada vez que se presiona un botón.
/// - Comprueba si el objeto tiene un `AudioSource` y lo configura adecuadamente.
/// - Si el `AudioSource` no está presente, lo añade automáticamente.
/// - Asigna la función de reproducción de sonido al botón al inicio.
/// </summary>
public class BotonSonido : MonoBehaviour
{
    // ======================== 📌 CONFIGURACIÓN DE SONIDO 📌 ========================

    [Header("Configuración de Sonido")]
    public AudioClip sonidoBoton; // 🔹 Sonido asignado desde el inspector
    private AudioSource audioSource; // 🔹 Referencia al componente AudioSource

    /// <summary>
    /// Método `Start()`. Se ejecuta una vez al inicio del juego.
    /// - Verifica si el objeto ya tiene un `AudioSource`, si no, lo agrega automáticamente.
    /// - Configura el `AudioSource` con los parámetros adecuados.
    /// - Asigna la función `ReproducirSonido()` al evento `onClick` del botón.
    /// </summary>
    void Start()
    {
        // 🔎 Buscar si hay un AudioSource en el objeto. Si no lo encuentra, lo crea.
        audioSource = gameObject.GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogWarning($"⚠️ No se encontró un AudioSource en {gameObject.name}, agregando uno nuevo...");
            audioSource = gameObject.AddComponent<AudioSource>(); // Agregar un nuevo AudioSource
        }

        // ======================== 📌 CONFIGURACIÓN DEL AUDIOSOURCE 📌 ========================

        audioSource.playOnAwake = false; // 🔹 No debe reproducirse automáticamente al iniciar el juego
        audioSource.volume = 1f; // 🔹 Volumen al 100%
        audioSource.mute = false; // 🔹 Asegura que no esté en mute
        audioSource.enabled = true; // 🔹 Se asegura de que el `AudioSource` esté habilitado
        audioSource.loop = false; // 🔹 No se repite en bucle

        // ======================== 📌 CONFIGURACIÓN DEL BOTÓN 📌 ========================

        // 🔎 Buscar el componente Button y asignar la función `ReproducirSonido()`
        Button boton = GetComponent<Button>();
        if (boton != null)
        {
            boton.onClick.AddListener(ReproducirSonido);
        }
        else
        {
            Debug.LogError($"🚨 No se encontró un componente Button en {gameObject.name}");
        }
    }

    /// <summary>
    /// Método `ReproducirSonido()`.
    /// - Se ejecuta cuando el botón es presionado.
    /// - Reproduce el sonido asignado al botón, si está disponible.
    /// - En caso de que el `AudioSource` esté deshabilitado, lo habilita antes de reproducir el sonido.
    /// - Si el `AudioClip` no está asignado, muestra una advertencia en consola.
    /// </summary>
    void ReproducirSonido()
    {
        Debug.Log($"🔊 Intentando reproducir sonido en {gameObject.name}");

        // 🔹 Si el `AudioSource` es nulo, lo crea y lo configura
        if (audioSource == null)
        {
            Debug.LogError($"❌ Error: No hay AudioSource en {gameObject.name}, agregando uno nuevo...");
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.volume = 1f;
        }

        // 🔹 Si el `AudioSource` está deshabilitado, lo habilita antes de reproducir el sonido
        if (!audioSource.enabled)
        {
            Debug.LogWarning($"⚠️ AudioSource en {gameObject.name} estaba desactivado. Se ha reactivado.");
            audioSource.enabled = true;
        }

        // 🔊 Reproducir el sonido si está asignado
        if (sonidoBoton != null)
        {
            audioSource.PlayOneShot(sonidoBoton);
            Debug.Log("✅ Sonido reproducido correctamente.");
        }
        else
        {
            Debug.LogWarning($"⚠️ No se ha asignado un sonido al botón {gameObject.name}");
        }
    }
}
