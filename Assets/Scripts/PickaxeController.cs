using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeController : CloseWeaponController
{
    public static bool isActivate = true;
    void Update()
    {
        if (isActivate)
        {
            TryAttack();
        }
    }

    private void Awake()
    {
        WeaponManager.currentWeapon = curCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnimator = curCloseWeapon.anim;
    }

    protected override IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject())
            {
                //충돌
                isSwing = false;
                Debug.Log(hit.transform.name);
                if (hit.transform.CompareTag("Rock")) hit.transform.GetComponent<Rock>().Mining();
                else if (hit.transform.CompareTag("WeekAnimal"))
                {
                    SoundManager.instance.PlaySE("Animal_hit");
                    hit.transform.GetComponent<WeekAnimal>().Damage(curCloseWeapon.damage, transform.position);
                }
                else if (hit.transform.CompareTag("StrongAnimal"))
                {
                    SoundManager.instance.PlaySE("Animal_hit");
                    //hit.transform.GetComponent<StrongAnimal>().Damage(curCloseWeapon.damage, transform.position);
                }
            }

            yield return null;
        }
    }

    public override void CloseWeaponChange(CloseWeapon _closeWeapon)
    {
        base.CloseWeaponChange(_closeWeapon);
        isActivate = true;
    }
}
