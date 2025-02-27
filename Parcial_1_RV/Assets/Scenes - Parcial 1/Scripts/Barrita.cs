using UnityEngine;

public class Barrita : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Barrita recogida por el jugador.");
            UIManager.instancia.RecogerBarrita();
            Destroy(gameObject);
        }
    }
}
