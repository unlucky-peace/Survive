using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField] private int hp; //바위의 제거 시간
    [SerializeField] private float destroyTime = 0f; //파편 제거 시간
    [SerializeField] private SphereCollider col;
    
    [SerializeField] private GameObject goRock; // 돌
    [SerializeField] private GameObject goDebris; // 조각
    [SerializeField] private GameObject goEffectPrefab;

    [SerializeField] private int cnt; //아이템 등장 개수
    [SerializeField] private GameObject rockItemPrefab;

    private String strikeSound = "Pickaxe_strike";
    private String destroySound = "Rock_destroy";

    public void Mining()
    {
        SoundManager.instance.PlaySE(strikeSound);
        var eff = Instantiate(goEffectPrefab, col.bounds.center, Quaternion.identity);
        hp--;
        if (hp <= 0)Destruction();
        Destroy(eff, destroyTime);
    }

    private void Destruction()
    {
        SoundManager.instance.PlaySE(destroySound);
        for (int i = 0; i < cnt; i++)
        {
            Instantiate(rockItemPrefab, transform.position, Quaternion.identity);
        }
        col.enabled = false;
        Destroy(goRock);
        goDebris.SetActive(true);
        Destroy(goDebris, destroyTime);
    }
}
