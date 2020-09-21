using System;
using UnityEngine;

namespace Scripts.Weapon
{
    public abstract class Firearms : MonoBehaviour, IWeapon
    {
        public GameObject BulletPrefab;

        public Camera EyeCamera;

        public Transform MuzzlePoint;
        public Transform CasingPoint;

        public ParticleSystem MuzzleParticle;
        public ParticleSystem CasingParticle;


        public AudioSource FirearmsShootingAudioSource;
        public AudioSource FirearmsReloadAudioSource;
        public FirearmsAudioData FirearmsAudioData;
        public ImpactAudioData ImpactAudioData;
        public GameObject BulletImpactPrefab;

        public float FireRate;

        public int AmmoInMag = 30;
        public int MaxAmmoCarried = 120;

        protected int CurrentAmmo;
        protected int CurrentMaxAmmoCarried;
        protected float LastFireTime;
        protected Animator GunAnimator;
        protected AnimatorStateInfo GunStateInfo;
        protected float OriginFOV;
        protected bool IsAiming;


        protected virtual void Start()
        {
            CurrentAmmo = AmmoInMag;
            CurrentMaxAmmoCarried = MaxAmmoCarried;
            GunAnimator = GetComponent<Animator>();
            OriginFOV = EyeCamera.fieldOfView;
        }


        public void DoAttack()
        {
            Shooting();
        }


        protected abstract void Shooting();
        protected abstract void Reload();

        protected abstract void Aim();

        protected bool IsAllowShooting()
        {
            return Time.time - LastFireTime > 1 / FireRate;
        }
    }
}