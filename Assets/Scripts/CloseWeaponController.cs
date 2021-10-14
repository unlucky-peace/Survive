using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CloseWeaponController : MonoBehaviour
{
    //미완성 클래스 추상클래스

    [SerializeField] protected CloseWeapon curCloseWeapon; //현재 장착된 Hand형 타입 무기
    
    protected bool isAttack = false;
    protected bool isSwing = false;
    
    protected RaycastHit hit;

    [SerializeField] protected LayerMask layerMask;


    // Update is called once per frame


    protected void TryAttack()
    {
        if (!Inventory.inventoryActivated)
        {
            if (Input.GetButton("Fire1"))
            {
                if (!isAttack)
                {
                    StartCoroutine(AttackCoroutine());
                }
            }
        }
    }

    protected IEnumerator AttackCoroutine()
    {
        isAttack = true;
        curCloseWeapon.anim.SetTrigger("Attack");

        yield return new WaitForSeconds(curCloseWeapon.attackDelayA);
        isSwing = true;
        
        //공격 활성화 시점
        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(curCloseWeapon.attackDelayB);
        isSwing = false;

        yield return new WaitForSeconds(curCloseWeapon.attackDelay - curCloseWeapon.attackDelayA - curCloseWeapon.attackDelayB);
        isAttack = false;
    }

    protected abstract IEnumerator HitCoroutine();

    protected bool CheckObject()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit, curCloseWeapon.attackRange, layerMask))
        {
            return true;
        }

        return false;
    }
    
    //완성함수 + 추가편집 가능
    public virtual void CloseWeaponChange(CloseWeapon _closeWeapon)
    {
        if (WeaponManager.currentWeapon != null)
        {
            WeaponManager.currentWeapon.gameObject.SetActive(false);
        }

        curCloseWeapon = _closeWeapon;
        WeaponManager.currentWeapon = curCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnimator = curCloseWeapon.anim;
        
        curCloseWeapon.transform.localPosition = Vector3.zero;
        curCloseWeapon.gameObject.SetActive(true);
    }
}
