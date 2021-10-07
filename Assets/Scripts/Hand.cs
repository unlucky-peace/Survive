using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public string handName = ""; //너클, 맨손 구분
    public float attackRange = 0f;
    public int damage = 0;
    public float workSpeed = 0f; //작업속도
    public float attackDelay = 0f; //공격 딜레이
    public float attackDelayA = 0f; //공격 활성화 시점
    public float attackDelayB = 0f; //공격 비활성화 시점

    public Animator anim;
    
}
