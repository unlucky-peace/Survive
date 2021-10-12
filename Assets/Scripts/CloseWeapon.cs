using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseWeapon : MonoBehaviour
{
    public string closeWeaponName = ""; //근접 무기 이름
    
    //웨폰 유형
    public bool isHand;
    public bool isAxe;
    public bool isPickaxe;
    
    public float attackRange = 0f;
    public int damage = 0;
    public float workSpeed = 0f; //작업속도
    public float attackDelay = 0f; //공격 딜레이
    public float attackDelayA = 0f; //공격 활성화 시점
    public float attackDelayB = 0f; //공격 비활성화 시점

    public Animator anim;
    
}
