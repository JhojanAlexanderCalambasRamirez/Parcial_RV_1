using UnityEngine;

public class Jugador : MonoBehaviour
{
    public float velocidad = 5f;
    public float velocidadRotacion = 100f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!UIManager.instancia.EstaEnJuego()) return;

        float movimientoHorizontal = Input.GetAxis("Horizontal") * velocidad * Time.deltaTime;
        float movimientoVertical = Input.GetAxis("Vertical") * velocidad * Time.deltaTime;

        transform.Translate(movimientoHorizontal, 0, movimientoVertical);

        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(Vector3.up, -velocidadRotacion * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(Vector3.up, velocidadRotacion * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Barrita"))
        {
            Debug.Log("Colisión con Barrita detectada.");
            UIManager.instancia.RecogerBarrita();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Obstaculo"))
        {
            Debug.Log("Colisión con Obstáculo detectada.");
            UIManager.instancia.ColisionObstaculo();
            other.GetComponent<Obstaculo>()?.ActivarEfecto();
        }
    }
}
