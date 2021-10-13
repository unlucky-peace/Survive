using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusController : MonoBehaviour
{
    //체력
    [SerializeField] private int hp;
    private int curHp;
    
    //스테미너
    [SerializeField] private int sp;
    private int curSp;
    [SerializeField] private int spInc; //스테미너 증가 수치 
    [SerializeField] private int spRechargeTime; //스테미너 회복 딜레이
    private float curSPRechargeTime; //현재 시간
    private bool spUsed; //스테미너 소모 여부
    
    //방어력
    [SerializeField] private int dp;
    private int curDp;
    
    //배고픔
    [SerializeField] private int hungry;
    private int curHungry;
    [SerializeField] private int hungryDecTime; //일정 시간
    private float curHungryTime; //현재 시간
    
    //목마름
    [SerializeField] private int thirsty;
    private int curThirsty;
    [SerializeField] private int thirstyDecTime; //일정 시간
    private float curThirstyTime; //현재 시간

    //만족도
    [SerializeField] private int satisfy;
    private int curSatisfy;
    
    //참조
    [SerializeField] private Image[] _images;

    private const int HP = 0, DP = 1, SP = 2, HUNGRY = 3, THIRSTY = 4, SATISFY = 5;

    
    void Start()
    {
        curHp = hp;
        curDp = dp;
        curHungry = hungry;
        curSp = sp;
        curThirsty = thirsty;
        curSatisfy = satisfy;
    }
    
    void Update()
    {
        Hungry();
        Thirsty();
        SPRechargeTime();
        SPRecover();
        GagueUpdate();
    }

    private void Hungry()
    {
        if (curHungry > 0)
        {
            if (curHungryTime <= hungryDecTime) curHungryTime += Time.deltaTime;
            else
            {
                curHungry--;
                curHungryTime = 0;
            }
            
        }else
        {
            Debug.Log("배고픔 수치 0");
        }
    }

    private void Thirsty()
    {
        if (curThirsty > 0)
        {
            if (curThirstyTime <= thirstyDecTime) curThirstyTime += Time.deltaTime;
            else
            {
                curThirsty--;
                curThirstyTime = 0;
            }
        }else
        {
            Debug.Log("목마름 수치 0");
        }
    }

    public void DecStamina(int _count)
    {
        spUsed = true;
        curSPRechargeTime = 0;

        if (curSp - _count > 0)
        {
            curSp -= _count;
        }
        else curSp = 0;
    }

    public int GetCurSP()
    {
        return curSp;
    }

    private void SPRechargeTime()
    {
        if (spUsed)
        {
            if (curSPRechargeTime < spRechargeTime) curSPRechargeTime += Time.deltaTime;
            else spUsed = false;
        }
    }

    private void SPRecover()
    {
        if (!spUsed && curSp < sp)
        {
            curSp += spInc;
        }
    }
    
    public void IncSP(int _count)
    {
        if (curSp + _count < sp)
        {
            curSp += _count;
        }
        else curSp = sp;
    }


    public void IncHP(int _count)
    {
        if (curHp + _count < hp)
        {
            curHp += _count;
        }
        else curHp = hp;
    }

    public void DecHP(int _count)
    {
        if (curDp > 0)
        {
            DecDP(_count);
            return;
        }
        curHp -= _count;
        if (curHp <= 0)
        {
            Debug.Log("캐릭터의 hp가 0이 되었습니다");
        }
    }
    
    public void IncDP(int _count)
    {
        if (curDp + _count < dp)
        {
            curDp += _count;
        }
        else curDp = dp;
    }

    public void DecDP(int _count)
    {
        curDp -= _count;
        if (curDp <= 0)
        {
            Debug.Log("캐릭터의 dp가 0이 되었습니다");
        }
    }
    
    public void IncHungry(int _count)
    {
        if (curHungry + _count < hungry)
        {
            curHungry += _count;
        }
        else curHungry = hungry;
    }
    
    public void DecHungry(int _count)
    {
        if (curHungry - _count < 0)
        {
            curHungry = 0;
        }
        else curHungry -= _count;
    }
    
    public void IncThirsty(int _count)
    {
        if (curThirsty + _count < thirsty)
        {
            curThirsty += _count;
        }
        else curThirsty = thirsty;
    }
    
    public void DecThirsty(int _count)
    {
        if (curThirsty - _count < 0)
        {
            curThirsty = 0;
        }
        else curThirsty -= _count;
    }

    private void GagueUpdate()
    {
        _images[HP].fillAmount = (float) curHp / hp;
        _images[DP].fillAmount = (float) curDp / dp;
        _images[SP].fillAmount = (float) curSp / sp;
        _images[HUNGRY].fillAmount = (float) curHungry / hungry;
        _images[THIRSTY].fillAmount = (float) curThirsty / hungry;
        _images[SATISFY].fillAmount = (float) curSatisfy / satisfy;
    }
}
