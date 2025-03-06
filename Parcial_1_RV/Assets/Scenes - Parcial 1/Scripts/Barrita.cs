using UnityEngine;

public class Barrita : MonoBehaviour
{
    public float velocidadRotacion = 50f;
    public float amplitudMovimiento = 0.2f;
    public float velocidadMovimiento = 2f;

    private Vector3 posicionInicial;

    void Start()
    {
        posicionInicial = transform.position;
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
            Destroy(gameObject);
        }
    }
}
