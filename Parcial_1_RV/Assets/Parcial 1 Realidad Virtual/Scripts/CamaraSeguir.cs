using UnityEngine;

/// <summary>
/// Clase `CamaraSeguir` encargada de aplicar un sutil movimiento a la cámara
/// mientras sigue al jugador, generando un efecto de oscilación en el eje Y.
/// - Guarda la posición inicial de la cámara.
/// - Aplica un pequeño movimiento vertical (arriba y abajo) usando una función seno.
/// - Solo se activa cuando el juego está en curso.
/// </summary>
public class CamaraSeguir : MonoBehaviour
{
    // ======================== 📌 CONFIGURACIÓN DEL MOVIMIENTO 📌 ========================

    private Vector3 posicionInicial; // 🔹 Almacena la posición inicial de la cámara

    [Header("Parámetros de Movimiento")]
    public float amplitud = 0.04f; // 🔹 Cantidad de movimiento en Y (oscilación)
    public float frecuencia = 1.7f; // 🔹 Velocidad del movimiento oscilante

    /// <summary>
    /// Método `Start()`.
    /// - Guarda la posición inicial de la cámara al iniciar el juego.
    /// </summary>
    void Start()
    {
        posicionInicial = transform.localPosition; // 📌 Guarda la posición inicial en relación al jugador
    }

    /// <summary>
    /// Método `LateUpdate()`.
    /// - Se ejecuta después de `Update()` para asegurarse de que la cámara se mueva de manera sincronizada.
    /// - Aplica un ligero movimiento oscilante en el eje Y.
    /// - Se asegura de que el efecto solo ocurra si el juego está en curso.
    /// </summary>
    void LateUpdate()
    {
        // 🚫 Si el juego no está en curso, no hacer nada
        if (!UIManager.instancia.EstaEnJuego()) return;

        // 🔄 Movimiento oscilante en Y con efecto suave
        float movimientoY = Mathf.Sin(Time.time * frecuencia) * amplitud;
        transform.localPosition = posicionInicial + new Vector3(0, movimientoY, 0);
    }
}
