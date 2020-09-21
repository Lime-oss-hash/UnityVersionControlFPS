using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FPMovement : MonoBehaviour
{
    public float WalkSpeed;
    public float SprintingSpeed;
    public float JumpHeight;
    public float CrouchHeight;
    public float Gravity;

    private Transform characterTransform;
    private Rigidbody characterRigidbody;
    private CapsuleCollider capsuleCollider;
    private float currentSpeed;
    private float originHeight;

    private bool isGrounded=true;
    private bool isCrouched;

    private void Start()
    {
        characterTransform = transform;
        characterRigidbody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        originHeight = capsuleCollider.height;
    }

    private void FixedUpdate()
    {
        if (isGrounded)
        {
            var tmp_Horizontal = Input.GetAxis("Horizontal");
            var tmp_Vertical = Input.GetAxis("Vertical");
            var tmp_IsSprinting = Input.GetKey(KeyCode.LeftShift);
            var tmp_IsCrouchKeyDown = Input.GetKeyDown(KeyCode.C);
            if (tmp_IsCrouchKeyDown)
            {
                if (isCrouched)
                {
                    StartCoroutine(DoCrouch(originHeight));
                    isCrouched = false;
                }
                else
                {
                    StartCoroutine(DoCrouch(CrouchHeight));
                    isCrouched = true;
                }
            }


            currentSpeed = tmp_IsSprinting ? SprintingSpeed : WalkSpeed;


            var tmp_CurrentDirection = new Vector3(tmp_Horizontal, 0, tmp_Vertical);
            tmp_CurrentDirection = characterTransform.TransformDirection(tmp_CurrentDirection);
            tmp_CurrentDirection *= currentSpeed;

            var tmp_CurrentVelocity = characterRigidbody.velocity;
            var tmp_VelocityChange = tmp_CurrentDirection - tmp_CurrentVelocity;
            tmp_VelocityChange.y = 0;

            characterRigidbody.AddForce(tmp_VelocityChange, ForceMode.VelocityChange);

            if (Input.GetButtonDown("Jump"))
            {
                characterRigidbody.velocity = new Vector3(tmp_CurrentVelocity.x, CalculateJumpHeightSpeed(),
                    tmp_CurrentVelocity.z);
            }
        }
        else
        {
            characterRigidbody.AddForce(new Vector3(0, -Gravity * characterRigidbody.mass, 0));
        }
    }


    private IEnumerator DoCrouch(float _targetHeight)
    {
        while (Math.Abs(capsuleCollider.height - _targetHeight) > 0.05f)
        {
            capsuleCollider.height =
                Mathf.Lerp(capsuleCollider.height, _targetHeight, Time.deltaTime * 10);
            yield return null;
        }
    }

    private float CalculateJumpHeightSpeed()
    {
        return Mathf.Sqrt(2 * Gravity * JumpHeight);
    }


//    private void OnCollisionStay(Collision _other)
//    {
//        isGrounded = true;
//    }
//
//    private void OnCollisionExit(Collision _other)
//    {
//        isGrounded = false;
//    }
}