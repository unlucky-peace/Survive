using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private GunController _gunController;
    private Gun gun;
    
    //필요하면 HUD 호출
    [SerializeField] private GameObject bulletHUDOnOff;

    [SerializeField] private Text[] bulletT;
 
    
    void Update()
    {
        CheckBullet();
    }

    private void CheckBullet()
    {
        gun = _gunController.GetGun();
        bulletT[0].text = gun.carryBulletCount.ToString();
        bulletT[1].text = gun.reloadBulletCount.ToString();
        bulletT[2].text = gun.remainBulletCount.ToString();
    }
}
