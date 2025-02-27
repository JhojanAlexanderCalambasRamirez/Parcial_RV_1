using UnityEngine;

public class Barrita : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("✅ Colisión con Barrita detectada. Aumentando energía y puntaje...");
            UIManager.instancia.RecogerBarrita();  // Llamar a UIManager para actualizar puntaje
            Destroy(gameObject);
        }
    }
}
