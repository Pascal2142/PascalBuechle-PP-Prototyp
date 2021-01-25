using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // used to detect inputs
    private InputHandler inputH;

    [Header("Set player movement and rotation speed")]
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float rotationSpeed;
    [Space]

    [Header("Check if u want the Player to rotate towards the mouse position")]
    [SerializeField]
    private bool rotateToMouse;

    [SerializeField]
    private Camera cam;
    


    private void Awake()
    {
        inputH = GetComponent<InputHandler>();
    }

    // Update is called once per frame
    private void Update()
    {
        var targetVec = inputH.inputvector;
        var movementVec = MoveTowardTarget(targetVec);


        if (!rotateToMouse) { 
        RotateToTarget(movementVec);
        }
        else
        {
            RotateToMouse();
        }
    }

    // Optional, only used if bool rotateTomouse = true
    // convert mouse position to world position
    // rotates player towards mouse position
    private void RotateToMouse()
    {
        Ray ray = cam.ScreenPointToRay(inputH.mousePosition);

        if(Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance: 300f))
        {
            var target = hitInfo.point;
            target.y = transform.position.y;
            transform.LookAt(target);
        }
    }

    // Move the player toward target
    // return the target vector
    private Vector3 MoveTowardTarget(Vector3 targetVector)
    {
        var speed = moveSpeed * Time.deltaTime;
        // adjust movement to camera rotation
        targetVector = Quaternion.Euler(0, cam.gameObject.transform.eulerAngles.y, 0) * targetVector;
        var targetPosition = transform.position + targetVector * speed;
        
        transform.position = targetPosition;
        Debug.DrawLine(transform.position, targetPosition, Color.red);
        return targetVector;
    }

    // Rotate the player GameObjekt toward movement vector
    private void RotateToTarget(Vector3 movementVector)
    {
        if(movementVector.magnitude == 0) {
            return;
        }
        var rotation = Quaternion.LookRotation(movementVector);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed);
    }
}
