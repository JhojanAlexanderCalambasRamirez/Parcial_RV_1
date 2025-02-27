using UnityEngine;

public class Jugador : MonoBehaviour
{
    public float velocidad = 5f;
    public float velocidadRotacion = 100f;

    void Update()
    {
        if (!UIManager.instancia.EstaEnJuego()) return;

        float movimientoHorizontal = Input.GetAxis("Horizontal") * velocidad * Time.deltaTime;
        float movimientoVertical = Input.GetAxis("Vertical") * velocidad * Time.deltaTime;

        transform.Translate(movimientoHorizontal, 0, movimientoVertical);
    }
}
