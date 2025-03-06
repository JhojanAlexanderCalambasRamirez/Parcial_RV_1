using UnityEngine;

public class Barrita : MonoBehaviour
{
    public float velocidadRotacion = 150f;
    public float amplitudMovimiento = 0.3f;
    public float velocidadMovimiento = 5f;

    public AudioClip sonidoRecoger; // Clip de sonido
    private AudioSource audioSource;
    private Vector3 posicionInicial;

    void Start()
    {
        posicionInicial = transform.position;
        audioSource = GetComponent<AudioSource>(); // Obtener el AudioSource del objeto
    }

    void Update()
    {
        // Rotación en el eje Z (parece que gira como un trompo acostado)
        transform.Rotate(0, 0, velocidadRotacion * Time.deltaTime);

        // Movimiento arriba y abajo
        float nuevaY = posicionInicial.y + Mathf.Sin(Time.time * velocidadMovimiento) * amplitudMovimiento;
        transform.position = new Vector3(posicionInicial.x, nuevaY, posicionInicial.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("✅ Colisión con Barrita detectada. Aumentando energía y puntaje...");
            UIManager.instancia.RecogerBarrita();

            // Reproducir sonido
            if (sonidoRecoger != null && audioSource != null)
            {
                audioSource.PlayOneShot(sonidoRecoger);
            }

            // Destruir la barrita después de que el sonido termine
            Destroy(gameObject, 0.2f); // Se destruye con un pequeño retraso para que el sonido se escuche
        }
    }
}
