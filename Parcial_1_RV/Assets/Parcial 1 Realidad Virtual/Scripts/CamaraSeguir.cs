using UnityEngine;

public class CamaraSeguir : MonoBehaviour
{
    private Vector3 posicionInicial;
    public float amplitud = 0.02f; // Pequeño movimiento en Y
    public float frecuencia = 1.5f; // Velocidad del movimiento

    void Start()
    {
        posicionInicial = transform.localPosition; // Guarda la posición inicial en el jugador
    }

    void LateUpdate()
    {
        if (!UIManager.instancia.EstaEnJuego()) return;

        // Movimiento sutil de la cámara en Y
        float movimientoY = Mathf.Sin(Time.time * frecuencia) * amplitud;
        transform.localPosition = posicionInicial + new Vector3(0, movimientoY, 0);
    }
}
