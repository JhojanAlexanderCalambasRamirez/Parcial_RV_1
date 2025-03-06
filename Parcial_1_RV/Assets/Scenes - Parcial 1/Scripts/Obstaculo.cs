using UnityEngine;

public class Obstaculo : MonoBehaviour
{
    public AudioSource sonidoColision; // Referencia al AudioSource

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            float danio = Random.Range(10f, 20f);
            Debug.Log($"❌ Colisión con Obstáculo detectada. Daño recibido: {danio}");

            // 🔊 Reproducir sonido de colisión
            if (sonidoColision != null)
            {
                sonidoColision.Play();
            }

            ControladorSlider.instancia.ReducirEnergia(danio);

            // Destruir el objeto con un retraso para permitir que el sonido suene
            Destroy(gameObject, 0.5f);
        }
    }
}
