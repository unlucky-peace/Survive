using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.AI;

public class Animal : MonoBehaviour
{
    [SerializeField] protected String name = "";
    [SerializeField] protected int hp = 0;
    [SerializeField] protected float walkSpeed = 0f;
    [SerializeField] protected float runSpeed = 0f;
    protected Vector3 destination;

    protected bool _isWalking = false;
    protected bool _isActtig = false;
    protected bool _isRunning = false;
    protected bool _isDead = false;

    [SerializeField] protected float walkTime = 0f;
    [SerializeField] protected float waitTime = 0f;
    [SerializeField] protected float runTime = 0f;
    protected float curTime = 0f;

    protected Animator _anim;
    protected Rigidbody _rigidbody;
    protected BoxCollider _boxCollider;
    protected AudioSource _audioSource;
    protected NavMeshAgent _nav;

    [SerializeField] protected AudioClip[] normalSound;
    [SerializeField] protected AudioClip hurtSound;
    [SerializeField] protected AudioClip deadSound;
    
    void Start()
    {
        _anim = GetComponentInChildren<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _boxCollider = GetComponent<BoxCollider>();
        _audioSource = GetComponent<AudioSource>();
        _nav = GetComponent<NavMeshAgent>();
        curTime = waitTime;
        _isActtig = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isDead)
        {
            ElapseTime();
            Move();
        }
    }

    protected void ElapseTime()
    {
        if (_isActtig)
        {
            curTime -= Time.deltaTime;
            if (curTime <= 0)
            {
                //다음 랜덤 행동
                ReSet();
            }
        }
    }



    protected void Move()
    {
        if (_isWalking || _isRunning)
        {
            //이부분 오류 왜 forward가 X축이지?
            //if(_isWalking) _rigidbody.MovePosition(transform.position + transform.right * walkSpeed * Time.deltaTime);
            _nav.SetDestination(transform.position + destination * 5f);
        }
    }
    
    
    protected virtual void ReSet()
    {
        _isWalking = false; _isActtig = true; _isRunning = false;
        _nav.speed = walkSpeed;
        _nav.ResetPath();
        _anim.SetBool("Walk", _isWalking);
        _anim.SetBool("Run", _isRunning);
        destination.Set(Random.Range(-0.2f, 0.2f), 0f, Random.Range(0.5f, 1f));
    }

    
    protected void TryWalk()
    {
        Debug.Log("걷기");
        _isWalking = true;
        _nav.speed = walkSpeed;
        _anim.SetBool("Walk", _isWalking);
        curTime = walkTime;
    }

    public virtual void Damage(int _dmg, Vector3 _targetPos)
    {
        if (!_isDead)
        {
            hp -= _dmg;
            if (hp <= 0)
            {
                Dead();
                return;
            }
            PlaySE(hurtSound);
            _anim.SetTrigger("Hurt");
        }
    }

    protected void Dead()
    {
        PlaySE(deadSound);
        _isRunning = false;
        _isWalking = false;
        _isDead = true;
        _anim.SetTrigger("Dead");
    }

    protected void RandomSound()
    {
        int _random = Random.Range(0, 3); //일상 사운드 3개
        PlaySE(normalSound[_random]);
    }

    protected void PlaySE(AudioClip _clip)
    {
        _audioSource.clip = _clip;
        _audioSource.Play();
    }
}
