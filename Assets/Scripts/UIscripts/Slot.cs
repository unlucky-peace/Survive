using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Item item;
    public int itemCount; //아이템의 개수
    public Image itemImage; //아이템 이미지

    private ItemEffDB _itemEffDB;
    [SerializeField] private Text textCount;
    [SerializeField] private GameObject countImage;


    private void Awake()
    {
        _itemEffDB = FindObjectOfType<ItemEffDB>();
    }

    //이미지 투명도 조절
    private void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }
    
    //아이템 추가
    public void AddItem(Item _item, int _count = 1)
    {
        item = _item;
        itemCount = _count;
        itemImage.sprite = item.itemImg;
        
        countImage.SetActive(true);
        textCount.text = itemCount.ToString();

        if (item.itemType != Item.ItemType.Equipment)
        {
            countImage.SetActive(true);
            textCount.text = itemCount.ToString();
        }
        else
        {
            countImage.SetActive(false);
            textCount.text = "";
        }
        SetColor(1);
    }
    
    //아이템 개수 조정
    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        textCount.text = itemCount.ToString();

        if (itemCount <= 0)
        {
            ClearSlot();
        }
    }
    
    //초기화
    private void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);
        textCount.text = itemCount.ToString();
        countImage.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (item != null)
            {
                _itemEffDB.UseItem(item);
                if(item.itemType == Item.ItemType.Used) SetSlotCount(-1);
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            DragSlot.instance.dragSlot = this;
            DragSlot.instance.DragSetImage(itemImage);
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DragSlot.instance.SetColor(0);
        DragSlot.instance.dragSlot = null;
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");
        if (DragSlot.instance.dragSlot != null) ChangeSlot();
        
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(item != null) _itemEffDB.ShowToolTip(item, transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _itemEffDB.HideToolTip();
    }
    
    private void ChangeSlot()
    {
        Item _tempItem = item;
        int _tempItemCount = itemCount;
        
        AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);

        if (_tempItem != null)
        {
            DragSlot.instance.dragSlot.AddItem(_tempItem, _tempItemCount);
        }
        else
        {
            DragSlot.instance.dragSlot.ClearSlot();
        }
    }

}
