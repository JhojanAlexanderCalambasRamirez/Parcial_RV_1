using UnityEngine;

public class Obstaculo : MonoBehaviour
{
    private Renderer renderObstaculo;
    private Color colorOriginal;

    void Start()
    {
        renderObstaculo = GetComponent<Renderer>();
        colorOriginal = renderObstaculo.material.color;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Jugador colision� con Obst�culo. Aplicando da�o.");
            UIManager.instancia.ColisionObstaculo();
            ActivarEfecto();
        }
    }

    public void ActivarEfecto()
    {
        Color nuevoColor = colorOriginal;
        nuevoColor.a = 0.5f;
        renderObstaculo.material.color = nuevoColor;
    }
}
