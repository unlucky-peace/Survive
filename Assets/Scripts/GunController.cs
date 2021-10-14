using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class GunController : MonoBehaviour
{
    public static bool isActivate = false; //활성화
    
    private bool _isReload = false;
    private bool _isFineSightMode = false;
    
    private float _currentFireRate = 0f; //연사속도
    private Vector3 _originPos; //본래 포지션 값
    
    private RaycastHit _hit; //충돌 정보
    [SerializeField] private LayerMask layerMask;
    
    private Camera _camera;
    [SerializeField] private Gun gun;
    private AudioSource _audioSource;
    [SerializeField] private GameObject hitEffectPrefab; // 피격 이펙트
    private Crosshair _crosshair;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _crosshair = FindObjectOfType<Crosshair>();
        _originPos = Vector3.zero;
        _camera = Camera.main;
    }

    void Update()
    {
        if (isActivate && !Inventory.inventoryActivated)
        {
            GunFireRateCalc();
            TryFire();
            TryReload();
            TryFineSight();
        }
    }
    
    //발사시도
    private void TryFire()
    {
        if (Input.GetButton("Fire1") && _currentFireRate <= 0 && !_isReload)
        {
            Fire();
        }
    }
    
    //발사전 계산
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
    
    //발사 후 계산
    private void Shoot()
    {
        _crosshair.FireAnimation();
        gun.remainBulletCount--;
        _currentFireRate = gun.fireRate; //연사속도 재계산
        PlaySE(gun.fireSound);
        gun.muzzleFlash.Play();
        
        Hit();
        
        StopAllCoroutines();
        StartCoroutine(RetroActionCoroutine());
        Debug.Log("히히발싸");
    }
    
    //반동
    IEnumerator RetroActionCoroutine()
    {
        Vector3 recoliBack = new Vector3(gun.retroActionForce, _originPos.y, _originPos.z);
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
            gun.transform.localPosition = _originPos;
            //반동
            while (gun.transform.localPosition.x <= gun.retroActionForce - 0.02f)
            {
                gun.transform.localPosition = Vector3.Lerp(gun.transform.localPosition, recoliBack, 0.4f);
                yield return null;
            }

            while (gun.transform.localPosition != _originPos)
            {
                gun.transform.localPosition = Vector3.Lerp(gun.transform.localPosition, _originPos, 0.1f);
                yield return null;
            }
        }
    }
    
    //반동계산
    private void GunFireRateCalc()
    {
        if (_currentFireRate > 0)
        {
            _currentFireRate -= Time.deltaTime;
        }
    }

    private void Hit()
    {
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward +
            new Vector3(Random.Range(-_crosshair.GetAccuracy() - gun.accurancy, _crosshair.GetAccuracy() + gun.accurancy),
                Random.Range(-_crosshair.GetAccuracy() - gun.accurancy, _crosshair.GetAccuracy() + gun.accurancy), 0),
            out _hit, gun.attackRange, layerMask))
        {
            GameObject effect = Instantiate(hitEffectPrefab, _hit.point, Quaternion.LookRotation(_hit.normal));
            Destroy(effect, 2f);
        }
    }
    
    private void PlaySE(AudioClip _clip)
    {
        _audioSource.clip = _clip;
        _audioSource.Play();
    }
    
    //재장전시도
    private void TryReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && !_isReload && gun.remainBulletCount < gun.reloadBulletCount)
        {
            CancleFineSight();
            StartCoroutine(Reload());
        }
    }

    //재장전
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

    public void CancleReload()
    {
        if (_isReload)
        {
            StopAllCoroutines();
            _isReload = false;
        }
    }
    
    //정조준시도
    private void TryFineSight()
    {
        if (Input.GetButtonDown("Fire2") && !_isReload)
        {
            FineSight();
        }
    }

    //정조준
    private void FineSight()
    {
        _isFineSightMode = !_isFineSightMode;
        gun.anim.SetBool("FineSightMode", _isFineSightMode);
        _crosshair.FinSightanimation(_isFineSightMode);
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
    
    //정조준 해제
    public void CancleFineSight()
    {
        if (_isFineSightMode)
        {
            FineSight();
        }
    }
    
    //정조준 애니메이션
    IEnumerator FineSightCoroutine()
    {
        while (gun.transform.localPosition != gun.fineSightOriginPos)
        {
            gun.transform.localPosition = Vector3.Lerp(gun.transform.localPosition, gun.fineSightOriginPos, 0.2f);
            yield return null;
        }
    }
    
    //정조준 해제 애니메이션
    IEnumerator FineSightOutCoroutine()
    {
        while (gun.transform.localPosition != _originPos)
        {
            gun.transform.localPosition = Vector3.Lerp(gun.transform.localPosition, _originPos, 0.2f);
            yield return null;
        }
    }

    public Gun GetGun()
    {
        return gun;
    }

    public bool GetFineSightMode()
    {
        return _isFineSightMode;
    }

    public void GunChange(Gun _gun)
    {
        if (WeaponManager.currentWeapon != null)
        {
            WeaponManager.currentWeapon.gameObject.SetActive(false);
        }

        gun = _gun;
        WeaponManager.currentWeapon = gun.GetComponent<Transform>();
        WeaponManager.currentWeaponAnimator = gun.anim;
        
        gun.transform.localPosition = Vector3.zero;
        gun.gameObject.SetActive(true);
        isActivate = true;
    }
}
