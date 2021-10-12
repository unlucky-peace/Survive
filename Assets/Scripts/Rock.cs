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
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip miningSE;
    [SerializeField] private AudioClip miningSE2;

    public void Mining()
    {
        _audioSource.clip = miningSE;
        _audioSource.Play();
        var eff = Instantiate(goEffectPrefab, col.bounds.center, Quaternion.identity);
        hp--;
        if (hp <= 0)Destruction();
        Destroy(eff, destroyTime);
    }

    private void Destruction()
    {
        _audioSource.clip = miningSE2;
        _audioSource.Play();
        col.enabled = false;
        Destroy(goRock);
        goDebris.SetActive(true);
        Destroy(goDebris, destroyTime);
    }
}
