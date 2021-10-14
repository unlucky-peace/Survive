using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class Pig : WeekAnimal
{
    protected override void ReSet()
    {
        base.ReSet();
        RandomAction();
    }

    private void Wait()
    {
        Debug.Log("대기");
        curTime = waitTime;
    }
    
    private void Eat()
    {
        Debug.Log("풀뜯기");
        _anim.SetTrigger("Eat");
        curTime = waitTime;
    }
    
    private void Peek()
    {
        Debug.Log("두리번");
        _anim.SetTrigger("Peek");
        curTime = waitTime;
    }
    
    private void RandomAction()
    {
        RandomSound();
        int _random = Random.Range(0, 4);
        if (_random == 0) Wait();
        else if (_random == 1) Eat();
        else if (_random == 2) Peek();
        else if (_random == 3) TryWalk();

    }
}
