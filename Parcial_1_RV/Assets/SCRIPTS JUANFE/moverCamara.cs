using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moverCamara : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 6f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
