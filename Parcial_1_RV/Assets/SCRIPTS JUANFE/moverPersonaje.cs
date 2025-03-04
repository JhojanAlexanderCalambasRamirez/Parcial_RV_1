using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moverPersonaje : MonoBehaviour
{

    public KeyCode moverIzq;
    public KeyCode moverDer;

    public float horizontalVel = 0;
    public int numeroLinea = 2;
    public string controlLock = "n";


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody>().velocity = new Vector3(horizontalVel, 0, 6f);

        if ((Input.GetKeyDown(moverIzq)) && (numeroLinea > 1) && (controlLock == "n"))
        {
            horizontalVel = -2;
            StartCoroutine(detenerSlide());
            numeroLinea -= 1;
            controlLock = "y";
        }


        if ((Input.GetKeyDown(moverDer)) && (numeroLinea < 3) && (controlLock == "n"))
        {
            horizontalVel = 2;
            StartCoroutine(detenerSlide());
            numeroLinea += 1;
            controlLock = "y";
        }
    }

    IEnumerator detenerSlide()
    {
        yield return new WaitForSeconds(.5f);
        horizontalVel = 0;
        controlLock = "n";
    }
}
