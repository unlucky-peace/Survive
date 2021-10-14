using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeekAnimal : Animal
{
    public void Run(Vector3 _targetPos)
    {
        destination = new Vector3(transform.position.x - _targetPos.x, 0f, transform.position.z - _targetPos.z)
            .normalized;
        _nav.speed = runSpeed;
        curTime = runTime;
        _isWalking = false;
        _isRunning = true;
        _anim.SetBool("Run", _isRunning);
    }

    public override void Damage(int _dmg, Vector3 _targetPos)
    {
        base.Damage(_dmg, _targetPos);
        if(!_isDead) Run(_targetPos);
    }
}
