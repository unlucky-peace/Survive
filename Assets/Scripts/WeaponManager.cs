using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(GunController))]
public class WeaponManager : MonoBehaviour
{
    public static bool isChange = false; //정적 변수, 중복 교체 수행을 막아주기 위함

    [SerializeField] private float _changeWeaponDelayTime = 0f;
    [SerializeField] private float _changeWeaponEndDelayTime = 0f;

    [SerializeField] private Gun[] guns;
    [SerializeField] private CloseWeapon[] hands;
    [SerializeField] private CloseWeapon[] axes;
    [SerializeField] private CloseWeapon[] pickaxes;
    
    //무기 접근이 쉽도록 딕셔너리로
    private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();
    private Dictionary<string, CloseWeapon> handDictionary = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, CloseWeapon> axeDictionary = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, CloseWeapon> pickaxeDictionary = new Dictionary<string, CloseWeapon>();
    
    //컴포넌트
    [SerializeField] private GunController _gunController;
    [SerializeField] private HandController _handController;
    [SerializeField] private AxeController _axeController;
    [SerializeField] private PickaxeController _pickaxeController;
    
    [SerializeField] private string currentWeaponType; //현재 무기와 타입
    public static Transform currentWeapon; //현재 무기 위치
    public static Animator currentWeaponAnimator; //현재 무기 애니메이션

    void Start()
    {
        for (int i = 0; i < guns.Length; i++)
        {
            gunDictionary.Add(guns[i].gunName, guns[i]);
        }
        
        for (int i = 0; i < hands.Length; i++)
        {
            handDictionary.Add(hands[i].closeWeaponName, hands[i]);
        }
        
        for (int i = 0; i < axes.Length; i++)
        {
            axeDictionary.Add(axes[i].closeWeaponName, axes[i]);
        }
        
        for (int i = 0; i < pickaxes.Length; i++)
        {
            pickaxeDictionary.Add(pickaxes[i].closeWeaponName, pickaxes[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isChange)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                StartCoroutine(ChangeWeaponCoroutine("HAND", "맨손"));

            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                StartCoroutine(ChangeWeaponCoroutine("GUN", "SubMachineGun1"));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                StartCoroutine(ChangeWeaponCoroutine("AXE", "Axe"));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                StartCoroutine(ChangeWeaponCoroutine("PICKAXE", "Pickaxe"));
            }
        }
    }

    public IEnumerator ChangeWeaponCoroutine(string _type, string _name)
    {
        isChange = true;
        currentWeaponAnimator.SetTrigger("Weapon_Out");

        yield return new WaitForSeconds(_changeWeaponDelayTime);

        CanclePreWeaponAction();
        WeaponChange(_type, _name);
        
        yield return new WaitForSeconds(_changeWeaponEndDelayTime);

        currentWeaponType = _type;
        isChange = false;
    }

    private void WeaponChange(string _type, string _name)
    {
        if (_type == "GUN")
        {
            _gunController.GunChange(gunDictionary[_name]);
        }
        else if (_type == "HAND")
        {
            _handController.CloseWeaponChange(handDictionary[_name]);
        }
        else if (_type == "AXE")
        {
            _axeController.CloseWeaponChange(axeDictionary[_name]);
        }
        else if (_type == "PICKAXE")
        {
            _axeController.CloseWeaponChange(pickaxeDictionary[_name]);
        }
    }

    private void CanclePreWeaponAction()
    {
        switch (currentWeaponType)
        {
            case "GUN" :
                _gunController.CancleFineSight();
                _gunController.CancleReload();
                GunController.isActivate = false;
                break;
            case "HAND" :
                HandController.isActivate = false;
                break;
            case "AXE" :
                AxeController.isActivate = false;
                break;
            case "PICKAXE" :
                PickaxeController.isActivate = false;
                break;
        }
    }

    IEnumerator WeaponInCoroutine()
    {
        isChange = true;
        currentWeaponAnimator.SetTrigger("Weapon_Out");

        yield return new WaitForSeconds(_changeWeaponDelayTime);
        currentWeapon.gameObject.SetActive(false);
    }
    
    public void WeaponOut()
    {
        isChange = false;
        currentWeapon.gameObject.SetActive(true);
    }

}
