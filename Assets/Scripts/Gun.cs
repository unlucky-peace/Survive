using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public string gunName = ""; //총의 이름

    public float attackRange = 0f;
    public float accurancy = 0f; //반동
    public float fireRate = 0f; //연사속도
    public float reloadTime = 0f;
    public int damage = 0;

    public int reloadBulletCount = 0; // 한 탄창에 몇발
    public int remainBulletCount = 0; // 탄창에 남아있는 총알 
    public int maxBulletCount = 0; // 최대 소유가능한 총알
    public int carryBulletCount = 0; // 현재 소유하고 있는 총알

    public float retroActionForce; //반동 세기
    public float retroActionFineSightForece; //정조준시 반동
    public Vector3 fineSightOriginPos;

    public Animator anim;
    public ParticleSystem muzzleFlash;
    public AudioClip fireSound;
}
