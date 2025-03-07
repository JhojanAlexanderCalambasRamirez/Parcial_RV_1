using UnityEngine;
using UnityEngine.UI;

public class BotonSonido : MonoBehaviour
{
    [Header("Configuración de Sonido")]
    public AudioClip sonidoBoton; // Sonido asignado desde el inspector
    private AudioSource audioSource;

    void Start()
    {
        // Buscar si hay un AudioSource en el objeto, si no hay, agregar uno
        audioSource = gameObject.GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogWarning($"⚠️ No se encontró un AudioSource en {gameObject.name}, agregando uno nuevo...");
            audioSource = gameObject.AddComponent<AudioSource>(); // Agregar un nuevo AudioSource
        }

        // Configurar el AudioSource
        audioSource.playOnAwake = false;
        audioSource.volume = 1f;
        audioSource.mute = false;
        audioSource.enabled = true; // 🔹 Forzar activación
        audioSource.loop = false;

        // Agregar la función al botón
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

    void ReproducirSonido()
    {
        Debug.Log($"🔊 Intentando reproducir sonido en {gameObject.name}");

        // 🔹 Forzar la activación del AudioSource ANTES de reproducir el sonido
        if (audioSource == null)
        {
            Debug.LogError($"❌ Error: No hay AudioSource en {gameObject.name}, agregando uno nuevo...");
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.volume = 1f;
        }

        if (!audioSource.enabled)
        {
            Debug.LogWarning($"⚠️ AudioSource en {gameObject.name} estaba desactivado. Se ha reactivado.");
            audioSource.enabled = true;
        }

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
