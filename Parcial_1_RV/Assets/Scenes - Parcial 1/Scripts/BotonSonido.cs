using UnityEngine;
using UnityEngine.UI;

public class BotonSonido : MonoBehaviour
{
    [Header("Configuración de Sonido")]
    public AudioClip sonidoBoton; // Sonido asignado desde el inspector
    private AudioSource audioSource;

    void Start()
    {
        // Verificar si hay un AudioSource en el botón, si no, agregarlo
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Configurar el AudioSource
        audioSource.playOnAwake = false; // Que no suene al iniciar
        audioSource.volume = 1f; // Ajusta el volumen (puedes cambiarlo si es necesario)
        audioSource.mute = false; // Asegurar que no está silenciado
        audioSource.enabled = true; // Habilitar el AudioSource por si acaso

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

        if (sonidoBoton != null)
        {
            if (!audioSource.enabled)
            {
                audioSource.enabled = true; // Habilita el AudioSource si está desactivado
            }

            audioSource.PlayOneShot(sonidoBoton);
            Debug.Log("✅ Sonido reproducido correctamente.");
        }
        else
        {
            Debug.LogWarning($"⚠️ No se ha asignado un sonido al botón {gameObject.name}");
        }
    }
}
