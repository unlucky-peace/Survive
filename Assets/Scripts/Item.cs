using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//게임 오브젝트에 붙이지 않아도
[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class Item : ScriptableObject
{
    public string itemName = "";
    [TextArea]
    public string itemDesc = "";
    public ItemType itemType;
    public Sprite itemImg;
    public GameObject itemPrefabs;

    public string weaponType; //무기 유형

    public enum ItemType
    {
        Equipment, Used, Ingredient, ETC
    }
}
