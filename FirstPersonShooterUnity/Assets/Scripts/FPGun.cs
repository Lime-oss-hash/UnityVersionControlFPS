using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPGun : MonoBehaviour
{
    public Transform Muzzle;
    public Transform Casing;
    public GameObject BulletPrefab;
    public Camera GunCamera;

    public float FireRate;

    public Animator charactorAnimator;
    private float LastTime;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if ((Time.time - LastTime > 1 / FireRate))
            {
                charactorAnimator.Play("fire", 0, 0);

                GameObject tmp_Bullet = Instantiate(BulletPrefab, Muzzle.position, Muzzle.rotation);
                Rigidbody tmp_Rigidbody = tmp_Bullet.AddComponent<Rigidbody>();
                tmp_Rigidbody.rotation = Quaternion.LookRotation(Muzzle.forward);
                tmp_Rigidbody.velocity = Muzzle.forward * 100;
                LastTime = Time.time;
            }
        }

        if (Input.GetMouseButton(1))
        {
            float tmp_CurrentVelocity = 0;
            GunCamera.fieldOfView =
                Mathf.SmoothDamp(GunCamera.fieldOfView, 20, ref tmp_CurrentVelocity, Time.deltaTime * 2.5f);
        }
    }
}