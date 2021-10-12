using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private float gunAccuracy; //크로스헤어 상태에 따른 총의 정확도

    [SerializeField] private GameObject _go_CrossHairHUD; //맨손 이런건 크로스헤어 필요없ㅂ음
    [SerializeField] private GunController _gunController;


    public void WalkingAnimation(bool _flag)
    {
        WeaponManager.currentWeaponAnimator.SetBool("Walk", _flag);
        _animator.SetBool("Walking", _flag);
    }

    public void RunningAnimation(bool _flag)
    {
        WeaponManager.currentWeaponAnimator.SetBool("Run", _flag);
        _animator.SetBool("Running", _flag);
    }
    
    public void JumpAnimation(bool _flag)
    {
        
        _animator.SetBool("Running", _flag);
    }
    
    public void CrouchingAnimation(bool _flag)
    {
        _animator.SetBool("Crouching", _flag);
    }
    
    public void FinSightanimation(bool _flag)
    {
        _animator.SetBool("FineSight", _flag);
    }

    public void FireAnimation()
    {
        if (_animator.GetBool("Walking")) _animator.SetTrigger("Walk_Fire");
        else if(_animator.GetBool("Crouching")) _animator.SetTrigger("Crouch_Fire");
        else _animator.SetTrigger("Idle_Fire");
    }

    public float GetAccuracy()
    {
        if (_animator.GetBool("Walking")) gunAccuracy = 0.05f;
        else if (_animator.GetBool("Crouching")) gunAccuracy = 0.015f;
        else if (_gunController.GetFineSightMode()) gunAccuracy = 0.001f;
        else gunAccuracy = 0.025f;
            
        return  gunAccuracy;
    }
    
    void Update()
    {
        
    }
}
