using UnityEngine;

public class Obstaculo : MonoBehaviour
{
    public AudioClip sonidoColision; // Sonido asignado en el Inspector
    private AudioSource audioSource;

    void Start()
    {
        // Verificar si hay un AudioSource en el obstáculo, si no, agregarlo
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Configuración del AudioSource
        audioSource.playOnAwake = false; // No reproducir sonido al inicio
        audioSource.volume = 0.7f; // Ajusta el volumen si es necesario
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            float danio = Random.Range(10f, 20f);
            Debug.Log($"❌ Colisión con Obstáculo detectada. Daño recibido: {danio}");
            ControladorSlider.instancia.ReducirEnergia(danio);

            // Reproducir sonido de colisión
            if (sonidoColision != null)
            {
                audioSource.PlayOneShot(sonidoColision);
            }
            else
            {
                Debug.LogWarning("⚠️ No se ha asignado un sonido al obstáculo " + gameObject.name);
            }
        }
    }
}
