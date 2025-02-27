using UnityEngine;

public class Obstaculo : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            float danio = Random.Range(10f, 20f);
            Debug.Log($"❌ Colisión con Obstáculo detectada. Daño recibido: {danio}");
            ControladorSlider.instancia.ReducirEnergia(danio);
        }
    }
}
