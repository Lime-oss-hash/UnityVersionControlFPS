using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPCharacterControllerMovement : MonoBehaviour
{
    private CharacterController characterController;

    private Vector3 movementDirection;
    private Transform characterTransform;


    public float MovementSpeed;
    public float Gravity = 9.8f;
    public float JumpHeight;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        characterTransform = transform;
    }


    private void Update()
    {
        if (characterController.isGrounded)
        {
            var tmp_Horizontal = Input.GetAxis("Horizontal");
            var tmp_Vertical = Input.GetAxis("Vertical");
            movementDirection =
                characterTransform.TransformDirection(new Vector3(tmp_Horizontal, 0, tmp_Vertical));

            if (Input.GetButtonDown("Jump"))
            {
                movementDirection.y = JumpHeight;
            }
        }
        
        movementDirection.y -= Gravity * Time.deltaTime;
        characterController.Move(MovementSpeed * Time.deltaTime * movementDirection);
    }
}