using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotToolTip : MonoBehaviour
{
    [SerializeField] private GameObject baseGO;

    [SerializeField] private Text itemNameTxt;
    [SerializeField] private Text itemdescTxt;
    [SerializeField] private Text htwTxt;

    public void ShowToolTip(Item _item, Vector3 _pos)
    {
        baseGO.SetActive(true);
        _pos += new Vector3(baseGO.GetComponent<RectTransform>().rect.width * 0.5f,-baseGO.GetComponent<RectTransform>().rect.height, 0);
        baseGO.transform.position = _pos;
        itemNameTxt.text = _item.itemName;
        itemdescTxt.text = _item.itemDesc;
        if (_item.itemType == Item.ItemType.Equipment) htwTxt.text = "우클릭 - 장착";
        else if (_item.itemType == Item.ItemType.Used) htwTxt.text = "우클릭 - 소모";
        else htwTxt.text = "";
    }

    public void HideToolTip()
    {
        baseGO.SetActive(false);
    }
}
