using UnityEngine;

public class Obstaculo : MonoBehaviour
{
    [Header("Configuración de Crecimiento")]
    public bool activarCrecimiento = false;
    public float factorCrecimiento = 1.2f;
    public float frecuenciaCrecimiento = 2f;

    [Header("Configuración de Sonido")]
    public AudioSource sonidoColision; // Referencia al AudioSource

    private Vector3 tamañoInicial;

    void Start()
    {
        tamañoInicial = transform.localScale;

        // 🔹 Si el obstáculo no tiene AudioSource, intenta buscarlo automáticamente
        if (sonidoColision == null)
        {
            sonidoColision = GetComponent<AudioSource>();

            if (sonidoColision == null)
            {
                Debug.LogWarning($"⚠️ Obstáculo {gameObject.name} no tiene AudioSource asignado.");
            }
        }
    }

    void Update()
    {
        if (activarCrecimiento)
        {
            float escala = 1 + Mathf.PingPong(Time.time * frecuenciaCrecimiento, factorCrecimiento - 1);
            transform.localScale = tamañoInicial * escala;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            float danio = Random.Range(10f, 20f);
            Debug.Log($"❌ Colisión con Obstáculo detectada. Daño recibido: {danio}");

            // 🔊 Solo reproduce el sonido si el `AudioSource` está presente
            if (sonidoColision != null)
            {
                sonidoColision.Play();
            }
            else
            {
                Debug.LogWarning($"⚠️ No se encontró AudioSource en el obstáculo {gameObject.name}");
            }

            ControladorSlider.instancia.ReducirEnergia(danio);
            Destroy(gameObject, 0.5f);
        }
    }
}
