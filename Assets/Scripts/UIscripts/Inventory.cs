using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static bool inventoryActivated = false;
    [SerializeField] private GameObject inventoryBaseGO; //필요한 컴포넌트
    [SerializeField] private GameObject slotsParentGO;
    private Slot[] _slots;

    public Slot[] GetSlots()
    {
        return _slots;
    }

    [SerializeField] private Item[] items;
    
    public void LoadToInven(int _arrayNum, string _itemName, int _itemNum)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if(items[i].itemName == _itemName) _slots[_arrayNum].AddItem(items[i], _itemNum);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        _slots = slotsParentGO.GetComponentsInChildren<Slot>();
    }

    // Update is called once per frame
    void Update()
    {
        TryOpenInventory();
    }

    private void TryOpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryActivated = !inventoryActivated;

            if (inventoryActivated) OpenInventory();
            else CloseInventory();
        }
    }

    private void CloseInventory()
    {
        GameManager.isOpenInventory = false;
        inventoryBaseGO.SetActive(false);
    }

    private void OpenInventory()
    {
        GameManager.isOpenInventory = true;
        inventoryBaseGO.SetActive(true);
    }

    public void AcquireItem(Item _item, int _count = 1)
    {
        if (Item.ItemType.Equipment != _item.itemType)
        {
            foreach (var t in _slots)
            {
                if (t.item != null)
                {
                    if (t.item.itemName == _item.itemName)
                    {
                        t.SetSlotCount(_count);
                        return;
                    }
                }
            }
        }

        foreach (var t in _slots)
        {
            if (t.item == null)
            {
                t.AddItem(_item, _count);
                return;
            }
        }
    }
}
