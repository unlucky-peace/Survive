using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GunController : MonoBehaviour
{

    private bool _isReload = false;
    private bool _isFineSightMode = false;

    private float currentFireRate = 0f;
    [SerializeField] private Vector3 originPos;

    [SerializeField] private Gun gun;
    private AudioSource _audioSource;

    
    

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        GunFireRateCalc();
        TryFire();
        TryReload();
        TryFineSight();
    }

    private void TryFire()
    {
        if (Input.GetButton("Fire1") && currentFireRate <= 0 && !_isReload)
        {
            Fire();
        }
    }

    private void Fire()
    {
        if (!_isReload)
        {
            if (gun.remainBulletCount > 0)
            {
                Shoot();
            }
            else
            {
                StartCoroutine(Reload());
            
            }
        }

    }
    
    private void Shoot()
    {
        gun.remainBulletCount--;
        currentFireRate = gun.fireRate; //연사속도 재계산
        PlaySE(gun.fireSound);
        gun.muzzleFlash.Play();
        
        StopAllCoroutines();
        StartCoroutine(RetroActionCoroutine());
        Debug.Log("히히발싸");
    }

    IEnumerator RetroActionCoroutine()
    {
        Vector3 recoliBack = new Vector3(gun.retroActionForce, originPos.y, originPos.z);
        Vector3 retroActionRecoilBack = new Vector3(gun.retroActionForce, gun.fineSightOriginPos.y,
            gun.fineSightOriginPos.z);
        if (_isFineSightMode)
        {
            gun.transform.localPosition = gun.fineSightOriginPos;
            //반동
            while (gun.transform.localPosition.x <= gun.retroActionFineSightForece - 0.02f)
            {
                gun.transform.localPosition = Vector3.Lerp(gun.transform.localPosition, retroActionRecoilBack, 0.4f);
                yield return null;
            }

            while (gun.transform.localPosition != gun.fineSightOriginPos)
            {
                gun.transform.localPosition = Vector3.Lerp(gun.transform.localPosition, gun.fineSightOriginPos, 0.1f);
                yield return null;
            }
        }
        else
        {
            gun.transform.localPosition = originPos;
            //반동
            while (gun.transform.localPosition.x <= gun.retroActionForce - 0.02f)
            {
                gun.transform.localPosition = Vector3.Lerp(gun.transform.localPosition, recoliBack, 0.4f);
                yield return null;
            }

            while (gun.transform.localPosition != originPos)
            {
                gun.transform.localPosition = Vector3.Lerp(gun.transform.localPosition, originPos, 0.1f);
                yield return null;
            }
        }
    }
    
    private void GunFireRateCalc()
    {
        if (currentFireRate > 0)
        {
            currentFireRate -= Time.deltaTime;
        }
    }

    private void PlaySE(AudioClip _clip)
    {
        _audioSource.clip = _clip;
        _audioSource.Play();
    }
    
    private void TryReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && !_isReload && gun.remainBulletCount < gun.reloadBulletCount)
        {
            CancleFineSight();
            StartCoroutine(Reload());
        }
    }

    IEnumerator Reload()
    {
        
        if (gun.carryBulletCount > 0)
        {
            _isReload = true;
            gun.anim.SetTrigger("Reload");
            yield return new WaitForSeconds(gun.reloadTime);

            gun.carryBulletCount += gun.remainBulletCount;
            gun.remainBulletCount = 0;
            
            if (gun.carryBulletCount >= gun.reloadBulletCount)
            {
                gun.remainBulletCount = gun.reloadBulletCount;
                gun.carryBulletCount -= gun.reloadBulletCount;
            }
            else
            {
                gun.remainBulletCount = gun.carryBulletCount;
                gun.carryBulletCount = 0;
            }
            
            _isReload = false;
        }
        else
        {
            Debug.Log("총알없음");
        }

    }
    
    private void TryFineSight()
    {
        if (Input.GetButtonDown("Fire2") && !_isReload)
        {
            FineSight();
        }
    }

    private void FineSight()
    {
        _isFineSightMode = !_isFineSightMode;
        gun.anim.SetBool("FineSightMode", _isFineSightMode);
        if (_isFineSightMode)
        {
            StopAllCoroutines();
            StartCoroutine(FineSightCoroutine());
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(FineSightOutCoroutine());
        }
    }

    public void CancleFineSight()
    {
        if (_isFineSightMode)
        {
            FineSight();
        }
    }

    IEnumerator FineSightCoroutine()
    {
        while (gun.transform.localPosition != gun.fineSightOriginPos)
        {
            gun.transform.localPosition = Vector3.Lerp(gun.transform.localPosition, gun.fineSightOriginPos, 0.2f);
            yield return null;
        }
    }
    
    IEnumerator FineSightOutCoroutine()
    {
        while (gun.transform.localPosition != originPos)
        {
            gun.transform.localPosition = Vector3.Lerp(gun.transform.localPosition, originPos, 0.2f);
            yield return null;
        }
    }
}
