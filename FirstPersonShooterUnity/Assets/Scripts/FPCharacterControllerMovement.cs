using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FPCharacterControllerMovement : MonoBehaviour
{
    private CharacterController characterController;
    private Animator characterAnimator;
    private Vector3 movementDirection;
    private Transform characterTransform;
    private float velocity;


    private bool isCrouched;
    private float originHeight;

    public float SprintingSpeed;
    public float WalkSpeed;

    public float SprintingSpeedWhenCrouched;
    public float WalkSpeedWhenCrouched;

    public float Gravity = 9.8f;
    public float JumpHeight;
    public float CrouchHeight = 1f;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        characterAnimator = GetComponentInChildren<Animator>();
        characterTransform = transform;
        originHeight = characterController.height;
    }


    private void Update()
    {
        float tmp_CurrentSpeed = WalkSpeed;
        if (characterController.isGrounded)
        {
            var tmp_Horizontal = Input.GetAxis("Horizontal");
            var tmp_Vertical = Input.GetAxis("Vertical");
            movementDirection =
                characterTransform.TransformDirection(new Vector3(tmp_Horizontal, 0, tmp_Vertical)).normalized;
            if (isCrouched)
            {
                tmp_CurrentSpeed = Input.GetKey(KeyCode.LeftShift) ? SprintingSpeedWhenCrouched : WalkSpeedWhenCrouched;
            }
            else
            {
                tmp_CurrentSpeed = Input.GetKey(KeyCode.LeftShift) ? SprintingSpeed : WalkSpeed;
            }

            if (Input.GetButtonDown("Jump"))
            {
                movementDirection.y = JumpHeight;
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                var tmp_CurrentHeight = isCrouched ? originHeight : CrouchHeight;
                StartCoroutine(DoCrouch(tmp_CurrentHeight));
                isCrouched = !isCrouched;
            }


            characterAnimator.SetFloat("Velocity",
                tmp_CurrentSpeed * movementDirection.normalized.magnitude,
                0.25f,
                Time.deltaTime);
        }

        movementDirection.y -= Gravity * Time.deltaTime;
        var tmp_Movement = tmp_CurrentSpeed * Time.deltaTime * movementDirection;
        characterController.Move(tmp_Movement);
    }


    private IEnumerator DoCrouch(float _target)
    {
        float tmp_CurrentHeight = 0;
        while (Mathf.Abs(characterController.height - _target) > 0.1f)
        {
            yield return null;
            characterController.height =
                Mathf.SmoothDamp(characterController.height, _target,
                    ref tmp_CurrentHeight, Time.deltaTime * 5);
        }
    }
}