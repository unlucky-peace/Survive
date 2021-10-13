using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemEffect
{
    public string itemName; //아이템 이름(키값)
    [Tooltip("HP, SP, DP, HUNGRY, THIRSTY, SATISFY 만 선택가능합니다")]
    public string[] part; //부위
    public int[] num; //수치
}
public class ItemEffDB : MonoBehaviour
{
    [SerializeField] private ItemEffect[] itemEffects;
    private const string HP = "HP", SP = "SP", DP = "DP", HUNGRY = "HUNGRY", THIRSTY = "THIRSTY", SATISFY = "SATISFY";
    
    
    [SerializeField] private StatusController _playerStatus; //플레이어 스테이터스 컴포넌트
    [SerializeField] private WeaponManager _weaponManager;
    [SerializeField] private SlotToolTip _slotToolTip;
    public void UseItem(Item _item)
    {
        if (_item.itemType == Item.ItemType.Equipment)
        {
            StartCoroutine(_weaponManager.ChangeWeaponCoroutine(_item.weaponType, _item.itemName));
        }
        else if (_item.itemType == Item.ItemType.Used)
        {
            for (int i = 0; i < itemEffects.Length; i++)
            {
                if (itemEffects[i].itemName == _item.itemName)
                {
                    for (int j = 0; j < itemEffects[i].part.Length; j++)
                    {
                        switch (itemEffects[i].part[j])
                        {
                            case HP:
                                _playerStatus.IncHP(itemEffects[i].num[j]);
                                break;
                            case DP:
                                _playerStatus.IncDP(itemEffects[i].num[j]);
                                break;
                            case SP:
                                _playerStatus.IncSP(itemEffects[i].num[j]);
                                break;
                            case HUNGRY:
                                _playerStatus.IncHungry(itemEffects[i].num[j]);
                                break;
                            case THIRSTY:
                                _playerStatus.IncThirsty(itemEffects[i].num[j]);
                                break;
                            case SATISFY:
                                break;
                            default:
                                Debug.Log("잘못된 Status를 회복 시키려 한다");
                                break;
                        }
                        Debug.Log(_item.itemName + " 을 소모하였습니다");
                    }
                    return;
                }
            }
            Debug.Log("ItemEffDB에 일치하는 아이템 이름 없음");
        }
    }

    public void ShowToolTip(Item _item, Vector3 _pos)
    {
        _slotToolTip.ShowToolTip(_item, _pos);
    }

    public void HideToolTip()
    {
        _slotToolTip.HideToolTip();
    }
}
