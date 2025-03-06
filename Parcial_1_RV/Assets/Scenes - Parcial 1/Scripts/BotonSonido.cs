using UnityEngine;
using UnityEngine.UI;

public class BotonSonido : MonoBehaviour
{
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

        // Asignar propiedades al AudioSource
        audioSource.playOnAwake = false; // Que no suene al iniciar
        audioSource.volume = 0.5f; // Ajusta el volumen si es necesario

        // Agregar la función al botón
        Button boton = GetComponent<Button>();
        if (boton != null)
        {
            boton.onClick.AddListener(ReproducirSonido);
        }
        else
        {
            Debug.LogError("🚨 No se encontró un componente Button en " + gameObject.name);
        }
    }

    void ReproducirSonido()
    {
        if (sonidoBoton != null)
        {
            audioSource.PlayOneShot(sonidoBoton); // Reproducir sonido asignado
        }
        else
        {
            Debug.LogWarning("⚠️ No se ha asignado un sonido al botón " + gameObject.name);
        }
    }
}
