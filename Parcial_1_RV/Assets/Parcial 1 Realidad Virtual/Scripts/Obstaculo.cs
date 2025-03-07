using UnityEngine;

public class Obstaculo : MonoBehaviour
{
    [Header("Configuración de Sonido")]
    public AudioSource sonidoColision; // Referencia al AudioSource

    void Start()
    {
        // 🔹 Si el obstáculo no tiene AudioSource asignado, intenta buscarlo automáticamente
        if (sonidoColision == null)
        {
            sonidoColision = GetComponent<AudioSource>();

            if (sonidoColision == null)
            {
                Debug.LogWarning($"⚠️ Obstáculo {gameObject.name} no tiene AudioSource asignado.");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            float danio = Random.Range(10f, 20f);
            Debug.Log($"❌ Colisión con Obstáculo detectada. Daño recibido: {danio}");

            // 🔊 Solo reproduce el sonido si el `AudioSource` está presente y habilitado
            if (sonidoColision != null && sonidoColision.enabled)
            {
                sonidoColision.Play();
            }
            else
            {
                Debug.LogWarning($"⚠️ No se pudo reproducir el sonido en {gameObject.name}");
            }

            ControladorSlider.instancia.ReducirEnergia(danio);
            Destroy(gameObject, 0.5f);
        }
    }
}
