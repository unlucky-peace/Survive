using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool canPlayerMove = true;
    public static bool isOpenInventory = false; //인벤토리 활성화
    public static bool isOpenCraftManual = false;
    public static bool isNight = false;
    public static bool isWater = false;
    public static bool isPause = false;
    private bool flag = false;

    private WeaponManager _weaponManager;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; //커서 락 모드에 들어있는 기능
        _weaponManager = FindObjectOfType<WeaponManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpenInventory || isOpenCraftManual || isPause)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            canPlayerMove = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            canPlayerMove = true;
        }

        if (isWater == true)
        {
            if (!flag)
            {
                _weaponManager.StartCoroutine("WeaponInCoroutine");
                flag = true;
            }
        }
        else
        {
            if (flag)
            {
                _weaponManager.WeaponOut();
                flag = false;
            }
        }
    }
}
