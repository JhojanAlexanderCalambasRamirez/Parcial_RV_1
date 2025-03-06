using UnityEngine;

public class Barrita : MonoBehaviour
{
    public AudioSource sonidoRecoger;  // Referencia al AudioSource

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("✅ Colisión con Barrita detectada. Aumentando energía...");

            // 🔊 Reproduce el sonido ANTES de destruir el objeto
            if (sonidoRecoger != null)
            {
                sonidoRecoger.Play();
            }

            UIManager.instancia.RecogerBarrita();

            // Destruir el objeto con un pequeño retraso para permitir que suene
            Destroy(gameObject, 0.5f);
        }
    }
}
