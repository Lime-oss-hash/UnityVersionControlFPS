using System;
using System.Collections;
using UnityEngine;

namespace Scripts.Weapon
{
    public class AssualtRifle : Firearms
    {
        private IEnumerator reloadAmmoCheckerCoroutine;
        private IEnumerator doAimCoroutine;

        protected override void Start()
        {
            base.Start();
            reloadAmmoCheckerCoroutine = CheckReloadAmmoAnimationEnd();
            doAimCoroutine = DoAim();
        }


        protected override void Shooting()
        {
            if (CurrentAmmo <= 0) return;
            if (!IsAllowShooting()) return;
            MuzzleParticle.Play();
            CurrentAmmo -= 1;

            GunAnimator.Play("Fire", IsAiming ? 1 : 0, 0);

            FirearmsShootingAudioSource.clip = FirearmsAudioData.ShootingAudio;
            FirearmsShootingAudioSource.Play();

            CreateBullet();
            CasingParticle.Play();
            LastFireTime = Time.time;
        }

        protected override void Reload()
        {
            GunAnimator.SetLayerWeight(2, 1);
            GunAnimator.SetTrigger(CurrentAmmo > 0 ? "ReloadLeft" : "ReloadOutOf");

            FirearmsReloadAudioSource.clip =
                CurrentAmmo > 0
                    ? FirearmsAudioData.ReloadLeft
                    : FirearmsAudioData.ReloadOutOf;

            FirearmsReloadAudioSource.Play();

            if (reloadAmmoCheckerCoroutine == null)
            {
                reloadAmmoCheckerCoroutine = CheckReloadAmmoAnimationEnd();
                StartCoroutine(reloadAmmoCheckerCoroutine);
            }
            else
            {
                StopCoroutine(reloadAmmoCheckerCoroutine);
                reloadAmmoCheckerCoroutine = null;
                reloadAmmoCheckerCoroutine = CheckReloadAmmoAnimationEnd();
                StartCoroutine(reloadAmmoCheckerCoroutine);
            }
        }

        protected override void Aim()
        {
            GunAnimator.SetBool("Aim", IsAiming);
            if (doAimCoroutine == null)
            {
                doAimCoroutine = DoAim();
                StartCoroutine(doAimCoroutine);
            }
            else
            {
                StopCoroutine(doAimCoroutine);
                doAimCoroutine = null;
                doAimCoroutine = DoAim();
                StartCoroutine(doAimCoroutine);
            }
        }


        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                DoAttack();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                Reload();
            }

            if (Input.GetMouseButtonDown(1))
            {
                //TODO:瞄准
                IsAiming = true;
                Aim();
            }

            if (Input.GetMouseButtonUp(1))
            {
                //TODO:退出瞄准
                IsAiming = false;
                Aim();
            }
        }


        private void CreateBullet()
        {
            GameObject tmp_Bullet = Instantiate(BulletPrefab, MuzzlePoint.position, MuzzlePoint.rotation);
            var tmp_BulletScript = tmp_Bullet.AddComponent<Bullet>();
            tmp_BulletScript.ImpactPrefab = BulletImpactPrefab;
            tmp_BulletScript.ImpactAudioData = ImpactAudioData;
            tmp_BulletScript.BulletSpeed = 100;
        }

        private IEnumerator CheckReloadAmmoAnimationEnd()
        {
            while (true)
            {
                yield return null;
                GunStateInfo = GunAnimator.GetCurrentAnimatorStateInfo(2);
                if (GunStateInfo.IsTag("ReloadAmmo"))
                {
                    if (GunStateInfo.normalizedTime >= 0.9f)
                    {
                        int tmp_NeedAmmoCount = AmmoInMag - CurrentAmmo;
                        int tmp_RemainingAmmo = CurrentMaxAmmoCarried - tmp_NeedAmmoCount;
                        if (tmp_RemainingAmmo <= 0)
                        {
                            CurrentAmmo += CurrentMaxAmmoCarried;
                        }
                        else
                        {
                            CurrentAmmo = AmmoInMag;
                        }

                        CurrentMaxAmmoCarried = tmp_RemainingAmmo <= 0 ? 0 : tmp_RemainingAmmo;

                        yield break;
                    }
                }
            }
        }

        private IEnumerator DoAim()
        {
            while (true)
            {
                yield return null;

                float tmp_CurrentFOV = 0;
                EyeCamera.fieldOfView =
                    Mathf.SmoothDamp(EyeCamera.fieldOfView,
                        IsAiming ? 26 : OriginFOV,
                        ref tmp_CurrentFOV,
                        Time.deltaTime * 2);
            }
        }
    }
}