using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    // Vectors to store inputs and the mouse position
    public Vector3 inputvector { get; private set; }
    public Vector3 mousePosition { get; private set; }


    // Update is called once per frame
    // check for inputs and set values in the vectors
    void Update()
    {
        var h = Input.GetAxisRaw("Horizontal");
        var v = Input.GetAxisRaw("Vertical");

        inputvector = new Vector3(h, 0f, v).normalized;

        mousePosition = Input.mousePosition;
    }
}
